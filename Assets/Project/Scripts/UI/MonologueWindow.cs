using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class MonologueWindow : MonoBehaviour, IInitializable
{
    public GameObject monologueWindow;
    public TextMeshProUGUI monologueText;
    public List<MonologueRule> IdleMonologues = new List<MonologueRule>();
    public List<MonologueRule> HungerMonologues = new List<MonologueRule>();
    public List<MonologueRule> FeedMonologues = new List<MonologueRule>();
    public List<MonologueRule> CleanMonologues = new List<MonologueRule>();
    public List<MonologueRule> PatMonologues = new List<MonologueRule>();
    public List<MonologueRule> PlayMonologues = new List<MonologueRule>();
    public List<MonologueRule> SendToBedMonologues = new List<MonologueRule>();


    [SerializeField] private List<NameCheck_MonologueRule> nameMonologues = new List<NameCheck_MonologueRule>();
    private Dictionary<string, List<MonologueRule>> nameMonologueDict = new Dictionary<string, List<MonologueRule>>();

    public void Initialize()
    {
        LoadNameMonologues();
    }
    private void LoadNameMonologues()
    {

        foreach (var nameRule in nameMonologues)
        {
            if (string.IsNullOrEmpty(nameRule.name))
            {
                Debug.LogWarning("Found monologue rule with empty name, skipping.");
                continue;
            }

            if (!nameMonologueDict.TryGetValue(nameRule.requiredName, out var monologueList))
            {
                monologueList = new List<MonologueRule>();
                monologueList.Add(nameRule);
                nameMonologueDict[nameRule.requiredName] = monologueList;
            }
            else
            {
                nameMonologueDict[nameRule.requiredName].Add(nameRule);
            }
        }
    }

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
        if (isActive)
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(Pet.Instance.transform.position);
            monologueWindow.transform.position = pos;
        }
    }
    private IEnumerator MonologueCoroutine(Monologue monologue)
    {

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


        yield return new WaitForSeconds(Monologue.time);
        isActive = false;
        monologueWindow.SetActive(false);
    }
    public void CreateMonologue(Monologue monologue)
    {
        if (SkinManger.instance.GetActiveHat().hatID == "Gag") return;
        StopCoroutine("MonologueCoroutine");
        StartCoroutine(MonologueCoroutine(monologue));
    }
    public void RandomMonologueFromList(List<MonologueRule> monologues)
    {
        if (monologues == null || monologues.Count == 0)
        {
            return;
        }

        int attempts = 20;
        MonologueRule monologueRule = null;
        do
        {
            attempts--;
            int rand = Random.Range(0, monologues.Count);
            if (monologues[rand].Condition())
                monologueRule = monologues[rand];
        } while (monologueRule == null || attempts > 0);
        CreateMonologue(monologueRule.monologue);
    }
    // Все монологи с учетом выбранных скинов, шляп
    public void SaySomething()
    {
        List<MonologueRule> combined = IdleMonologues
        .Concat(AvailableHatMonologues())
        .Concat(AvailableNameMonologues())
        .Concat(AvailablePetSkinMonologues())
        .ToList();

        RandomMonologueFromList(combined);
    }
    public void RandomHungerMonologue()
    {
        RandomMonologueFromList(HungerMonologues);
    }
    public List<MonologueRule> AvailableHatMonologues()
    {
        Hat hat = SkinManger.instance.GetActiveHat();
        return hat.monologues;
    }
    public List<MonologueRule> AvailablePetSkinMonologues()
    {
        PetSkin skin = SkinManger.instance.GetActivePetSkin();
        return skin.monologues.ToList();
    }
    public List<MonologueRule> AvailableNameMonologues()
    {
        string petName = Pet.Instance.petName;
        List<MonologueRule> monologues;
        if (nameMonologueDict.TryGetValue(petName, out monologues))
        {
            return monologues;
        }
        else
        {
            return new List<MonologueRule>();
        }
    }
}