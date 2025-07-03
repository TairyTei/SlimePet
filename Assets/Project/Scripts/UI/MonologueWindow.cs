using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class MonologueWindow : MonoBehaviour {
    public GameObject monologueWindow;
    public TextMeshProUGUI monologueText;
    public List<MonologueRule> idleMonologues = new List<MonologueRule>();
    public List<MonologueRule> hungerMonologues = new List<MonologueRule>();
    public AudioClip petVoice;
    public AudioClip petGrumpyVoice;
    public AudioClip petSadVoice;
    private bool isActive = false;
    public static MonologueWindow instance;
    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if(isActive){
            Vector2 pos = Camera.main.WorldToScreenPoint(Pet.Instance.transform.position);
            monologueWindow.transform.position = pos;
        }
    }
    private IEnumerator MonologueCoroutine(Monologue monologue){

        monologueText.text = VariableReplacer.Replace(monologue.Text);
        monologueWindow.SetActive(true);
        isActive = true;

        switch (Pet.Instance.Stats.Happiness)
            {
                default:
                    SoundManager.instance.PlaySound(petVoice, 0.3f, SoundType.Main);
                    break;
                case float n when (n >= 2.5f && n <= 5):
                     SoundManager.instance.PlaySound(petGrumpyVoice, 0.3f, SoundType.Main);
                    break;
                case float n when (n < 2.5):
                     SoundManager.instance.PlaySound(petSadVoice, 0.2f, SoundType.Main);
                    break;           
            }
       

        yield return new WaitForSeconds(monologue.time);
        isActive = false;
        monologueWindow.SetActive(false);
    }
    public void CreateMonologue(Monologue monologue){
        if(SkinManger.instance.GetActiveHat().hatID == "Gag") return;
        StopCoroutine("MonologueCoroutine");
        StartCoroutine(MonologueCoroutine(monologue));
    }
    private void RandomMonologueFromList(List<MonologueRule> monologues){
        MonologueRule monologueRule = null;
        do{
            int rand = Random.Range(0, monologues.Count);
            if(monologues[rand].Condition())
                monologueRule = monologues[rand];
        } while(monologueRule == null);
        CreateMonologue(monologueRule.monologue);
    }
    public void RandomIdleMonologue(){
        RandomMonologueFromList(idleMonologues);
    }
     public void RandomHungerMonologue(){
        RandomMonologueFromList(hungerMonologues);
    }
    public void RandomHatMonologue(){
        Hat hat = SkinManger.instance.GetActiveHat();
        RandomMonologueFromList(hat.monologues);
    }
}