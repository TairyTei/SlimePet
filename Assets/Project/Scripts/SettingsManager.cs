using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour, IInitializable
{
    [SerializeField]
    private Settings settings;
    public Settings Settings {get{return settings;} set{settings = value;}}
    public static SettingsManager insntace;
    public void Initialize()
    {
        insntace = this;
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
        settings.pinWindowOnTop = PlayerPrefs.GetInt("PinWindowOnTop") == 1;
    }
    private void FirstLaunch()
    {
        PlayerPrefs.SetFloat("MusicVolume", settings.musicVolume);
        PlayerPrefs.SetFloat("MainVolume", settings.mainVolume);
        PlayerPrefs.SetFloat("SystemVolume", settings.systemVolume);
        PlayerPrefs.SetInt("TargetFrameRate", settings.frameRate);
        PlayerPrefs.SetInt("DisplayStoryMark", settings.displayStoryMark ? 1 : 0);
        PlayerPrefs.SetInt("PinWindowOnTop", settings.pinWindowOnTop ? 1 : 0);
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
    public void UpdateSettings()
    {

    }
}
