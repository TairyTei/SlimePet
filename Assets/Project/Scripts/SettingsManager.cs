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
        settings.musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        settings.mainVolume = PlayerPrefs.GetFloat("MainVolume");
        settings.systemVolume = PlayerPrefs.GetFloat("SystemVolume");
        settings.frameRate = PlayerPrefs.GetInt("TargetFrameRate");
        Application.targetFrameRate = settings.frameRate;
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
    public void UpdateSettings()
    {

    }
}
