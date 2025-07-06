using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private Settings settings;
    public Settings Settings {get{return settings;} set{settings = value;}}
    public static SettingsManager insntace;
    void Awake()
    {
        insntace = this;
        LoadSettings();
    }
    private void LoadSettings()
    {
        settings.musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        settings.mainVolume = PlayerPrefs.GetFloat("MainVolume");
        settings.systemVolume = PlayerPrefs.GetFloat("SystemVolume");
    }
    void Start()
    {
        
    }
    public void UpdateSettings(){
        
    }

    public void SetDisplay(int index)
    {
        if (index >= 0 && index < Display.displays.Length)
        {
            settings.display = index;
            UpdateSettings();
        }
        else
        {
            Debug.LogError("Недопустимый индекс дисплея");
        }
    }
}
