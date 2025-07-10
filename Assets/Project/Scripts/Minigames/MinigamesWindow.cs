using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamesWindow : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private MinigameManager minigameManager;

    public void Open()
    {
        PetMenu.instance.Close();
        menu.SetActive(true);
    }
    public void Close()
    {
        menu.SetActive(false);
    }
    public void StartVictimMinigame()
    {
        Task victim = new MinigameVictim("Victim minigame", -1, minigameManager);
        Pet.Instance.AddTask(victim, 0);
        Pet.Instance.CancelBreak();
        Close();

        SayShit();
    }
    public void StartHideAndSeekMinigame()
    {
        Task hideAndSeek = new MinigameHideAndSeek("Hide and seek minigame", -1, minigameManager);
        Pet.Instance.AddTask(hideAndSeek, 0);
        Pet.Instance.CancelBreak();
        Close();

        SayShit();
    }
    public void AimMinigame()
    {
        Task aim = new MinigameAim("Aim minigame", -1, minigameManager);
        Pet.Instance.AddTask(aim, 0);
        Pet.Instance.CancelBreak();
        Close();

        SayShit();
    }

    private void SayShit()
    {
        if (Random.Range(0, 10) <= 2)
        {
            MonologueWindow mw = MonologueWindow.instance;
            mw.RandomMonologueFromList(mw.PlayMonologues);
        }
    }
}
