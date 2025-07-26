using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour, IInitializable
{
    [SerializeField]
    private Settings settings;
    public Settings Settings {get{return settings;} set{settings = value;}}
    public static SettingsManager instance;
     public event Action OnSettingsUpdated;
    public void Initialize()
    {
        instance = this;
        LoadSettings();
    }
    private void LoadSettings()
    {
        if (SaveManger.IsItFirstLaunch)
        {
            FirstLaunch();
            return;
        }
        settings.musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        settings.mainVolume = PlayerPrefs.GetFloat("MainVolume");
        settings.systemVolume = PlayerPrefs.GetFloat("SystemVolume");
        settings.frameRate = PlayerPrefs.GetInt("TargetFrameRate");
        Application.targetFrameRate = settings.frameRate;
        settings.displayStoryMark = PlayerPrefs.GetInt("DisplayStoryMark") == 1;
        bool isPinned = PlayerPrefs.GetInt("PinWindowOnTop") == 1;
        SetPinWindowOnTop(isPinned);
        settings.difficultyMultiplier = PlayerPrefs.GetFloat("DifficultyMultiplier");
    }
    private void FirstLaunch()
    {
        PlayerPrefs.SetFloat("MusicVolume", settings.musicVolume);
        PlayerPrefs.SetFloat("MainVolume", settings.mainVolume);
        PlayerPrefs.SetFloat("SystemVolume", settings.systemVolume);
        PlayerPrefs.SetInt("TargetFrameRate", settings.frameRate);
        PlayerPrefs.SetInt("DisplayStoryMark", settings.displayStoryMark ? 1 : 0);
        PlayerPrefs.SetInt("PinWindowOnTop", settings.pinWindowOnTop ? 1 : 0);
        PlayerPrefs.SetFloat("DifficultyMultiplier", settings.difficultyMultiplier);
    }
    void Start()
    {
        
    }
    public void SetFrameRate(int frameRate)
    {
        PlayerPrefs.SetInt("TargetFrameRate", frameRate);
        settings.frameRate = frameRate;
        Application.targetFrameRate = frameRate;
    }
    public void SetPinWindowOnTop(bool isPinned)
    {
        PlayerPrefs.SetInt("PinWindowOnTop", isPinned ? 1 : 0);
        settings.pinWindowOnTop = isPinned;
        TransparentWindow.Instance.SetTopMost(isPinned);
    }
    public void SetDisplayStoryMark(bool isOn)
    {
        PlayerPrefs.SetInt("DisplayStoryMark", isOn ? 1 : 0);
        settings.displayStoryMark = isOn;
        OnSettingsUpdated?.Invoke();


    }
    public void SetSoundSettings(float music, float main, float system)
    {
        settings.musicVolume = music;
        PlayerPrefs.SetFloat("MusicVolume", settings.musicVolume);
        settings.mainVolume = main;
        PlayerPrefs.SetFloat("MainVolume", settings.mainVolume);
        settings.systemVolume = system;
        PlayerPrefs.SetFloat("SystemVolume", settings.systemVolume);
        
        if (OnSettingsUpdated != null)
            OnSettingsUpdated.Invoke();
    }
    public void SetDifficultyMultiplier(float difficulty)
    {
        settings.difficultyMultiplier = difficulty;
        PlayerPrefs.SetFloat("DifficultyMultiplier", difficulty);
    }
}
