using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public GameObject menu;
    public TMP_Dropdown languageChoise;
    public Toggle launchWithWindows;
    public Toggle petCatchesCursor;
    public Slider musicVolume, mainVolume, systemVolume;
    public TMP_Dropdown displayChoise;
    [SerializeField] private MusicPlayer musicPlayer;
    public GameObject[] tabs;

    void Awake()
    {
        LocalizationSystem.OnLanguagesScanned += DrawLanguageDropdown;
    }
    private void UpdateSettings()
    {
        Settings settings = SettingsManager.insntace.Settings;
        petCatchesCursor.isOn = PlayerPrefs.GetInt("DoesPetCatchCursor") == 1 ? true : false;
        //sound settings
        musicVolume.value = settings.musicVolume;
        mainVolume.value = settings.mainVolume;
        systemVolume.value = settings.systemVolume;
        //screenChoise
        Display[] displays = Display.displays;
        displayChoise.options.Clear();
        for (int i = 0; i < displays.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData("Display " + (i + 1));
            displayChoise.options.Add(option);
        }
    }
    private void ApplySettings()
    {
        Settings settings = SettingsManager.insntace.Settings;

        //sound settings
        settings.musicVolume = musicVolume.value;
        PlayerPrefs.SetFloat("MusicVolume", settings.musicVolume);
        musicPlayer.SetVolume(settings.musicVolume);
        settings.mainVolume = mainVolume.value;
        PlayerPrefs.SetFloat("MainVolume", settings.mainVolume);
        settings.systemVolume = systemVolume.value;
        PlayerPrefs.SetFloat("SystemVolume", settings.systemVolume);
        
        SettingsManager.insntace.Settings.display = displayChoise.value;
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
        UpdateSettings();
        menu.SetActive(true);
        DrawLanguageDropdown();
    }
    public void Close()
    {
        menu.SetActive(false);
        ApplySettings();
    }
    public void ChangeDisplay()
    {
        int display = displayChoise.value;
        Monologue monologue = new Monologue("Теперь я на " + (display + 1) + " мониторе", 5f);
        MonologueWindow.instance.CreateMonologue(monologue);
        TransparentWindow.Instance.MoveToMonitor(display);
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
    public void DoesPetCatchCursor()
    {
        PlayerPrefs.SetInt("DoesPetCatchCursor", petCatchesCursor.isOn ? 1 : 0);
    }
}
