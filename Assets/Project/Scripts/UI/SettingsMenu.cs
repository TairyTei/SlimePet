using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour, IInitializable
{
    private int[] availableFramerate = { -1, 120, 60, 30 };
    public GameObject menu;
    public TMP_Dropdown languageChoise;
    public TMP_Dropdown framerateChoise;
    public Toggle launchWithWindows;
    public Toggle petCatchesCursor;
    public Slider musicVolume, mainVolume, systemVolume;
    public event Action OnSettingsUpdated;
    [SerializeField] private MusicPlayer musicPlayer;
    public GameObject[] tabs;

    public static SettingsMenu Instance { get; private set; }

    public void Initialize()
    {
        Instance = this;
        LoadSettings();
        DrawLanguageDropdown();
    }
    private void LoadSettings()
    {
        Settings settings = SettingsManager.insntace.Settings;
        petCatchesCursor.isOn = PlayerPrefs.GetInt("DoesPetCatchCursor") == 1 ? true : false;
        //sound settings
        musicVolume.value = settings.musicVolume;
        mainVolume.value = settings.mainVolume;
        systemVolume.value = settings.systemVolume;
        framerateChoise.value = Array.IndexOf(availableFramerate, settings.frameRate);
        launchWithWindows.isOn = AutoStartManager.IsAutoStartEnabled();
    }
    public void ApplySettings()
    {
        Settings settings = SettingsManager.insntace.Settings;

        //sound settings
        settings.musicVolume = musicVolume.value;
        PlayerPrefs.SetFloat("MusicVolume", settings.musicVolume);
        settings.mainVolume = mainVolume.value;
        PlayerPrefs.SetFloat("MainVolume", settings.mainVolume);
        settings.systemVolume = systemVolume.value;
        PlayerPrefs.SetFloat("SystemVolume", settings.systemVolume);

        OnSettingsUpdated.Invoke();
        SettingsManager.insntace.UpdateSettings();
    }
    public void OpenTab(int tabIndex)
    {
        if (tabIndex >= tabs.Length) return;

        for (int i = 0; i < tabs.Length; i++)
        {
            bool isCurrent = i == tabIndex ? true : false;
            tabs[i].SetActive(isCurrent);
        }
    }
    public void ReturnThePetToBed()
    {
        Pet pet = Pet.Instance;
        pet.ClearTasks();
        Task sleepTask = new SleepTask("Sleep", -1, false);
        pet.AddTask(sleepTask);
        pet.transform.position = Bed.instance.transform.position;
    }
    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Open()
    {
        OpenTab(0);
        BedMenu.instance.Close();
        menu.SetActive(true);
    }
    public void Close()
    {
        menu.SetActive(false);
        ApplySettings();
    }

    public void ChangeTargetFramerate()
    {

        int frameRate = -1;
        if (availableFramerate.Length > framerateChoise.value)
        {
            frameRate = availableFramerate[framerateChoise.value];
        }
        SettingsManager.insntace.SetFrameRate(frameRate);

    }
    public void SetAutostart()
    {
        AutoStartManager.SetAutoStart(launchWithWindows.isOn);
    }
    public void DoesPetCatchCursor()
    {
        PlayerPrefs.SetInt("DoesPetCatchCursor", petCatchesCursor.isOn ? 1 : 0);
    }
    public void DrawLanguageDropdown()
    {
        LanguageProfile[] availableLanguages = LocalizationSystem.GetAvailableLanguages();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (LanguageProfile lang in availableLanguages)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(lang.name);
            options.Add(option);
        }
        languageChoise.ClearOptions();
        languageChoise.AddOptions(options);
        languageChoise.value = Array.IndexOf(LocalizationSystem.GetAvailableLanguages(), LocalizationSystem.CurrentLanguage);
    }
    public void ChangeLanguage()
    {
        string langID = LocalizationSystem.GetAvailableLanguages()[languageChoise.value].id;
        LocalizationSystem.SetLanguage(langID);
    }
}
