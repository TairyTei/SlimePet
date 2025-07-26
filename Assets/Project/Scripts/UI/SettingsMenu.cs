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
    [SerializeField] private List<DifficultyLevel> difficultyLevels = new List<DifficultyLevel>();
    public GameObject menu;
    public TMP_Dropdown difficultyChoise;
    public TMP_Dropdown languageChoise;
    public TMP_Dropdown framerateChoise;
    public Toggle displayStoryMark;
    public Toggle petCatchesCursor;
    public Toggle pinWindowOnTop;
    public Slider musicVolume, mainVolume, systemVolume;
    [SerializeField] private MusicPlayer musicPlayer;
    public GameObject[] tabs;

    public static SettingsMenu Instance { get; private set; }

    public void Initialize()
    {
        Instance = this;
        LoadSettings();
        DrawLanguageDropdown();

        LocalizationSystem.OnLanguageChanged += UpdateLanguageDropDown;
    }
    private void LoadSettings()
    {
        Settings settings = SettingsManager.instance.Settings;
        petCatchesCursor.isOn = PlayerPrefs.GetInt("DoesPetCatchCursor") == 1 ? true : false;
        //sound settings
        musicVolume.value = settings.musicVolume;
        mainVolume.value = settings.mainVolume;
        systemVolume.value = settings.systemVolume;
        framerateChoise.SetValueWithoutNotify(Array.IndexOf(availableFramerate, settings.frameRate));
        DrawDifficultyDropdown();
        displayStoryMark.SetIsOnWithoutNotify(settings.displayStoryMark);
        Debug.Log("~" + settings.pinWindowOnTop);
        pinWindowOnTop.SetIsOnWithoutNotify(settings.pinWindowOnTop);
    }
    public void ApplySoundSettings()
    {
        SettingsManager.instance.SetSoundSettings(musicVolume.value, mainVolume.value, systemVolume.value);
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
        ApplySoundSettings();
    }

    public void ChangeTargetFramerate()
    {

        int frameRate = -1;
        if (availableFramerate.Length > framerateChoise.value)
        {
            frameRate = availableFramerate[framerateChoise.value];
        }
        SettingsManager.instance.SetFrameRate(frameRate);
    }
    public void ChangePinWindowOnTop()
    {
        SettingsManager.instance.SetPinWindowOnTop(pinWindowOnTop.isOn);
    }
    public void ChangeDifficulty()
    {
        float newDifficulty = difficultyLevels[difficultyChoise.value].value;
        SettingsManager.instance.SetDifficultyMultiplier(newDifficulty);
    }
    public void SetDisplayStoryMark()
    {
        SettingsManager.instance.SetDisplayStoryMark(displayStoryMark.isOn);
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
        UpdateLanguageDropDown();
    }
    public void UpdateLanguageDropDown()
    {
        languageChoise.value = Array.IndexOf(LocalizationSystem.GetAvailableLanguages(), LocalizationSystem.CurrentLanguage);
    }
    private void DrawDifficultyDropdown()
    {
        difficultyChoise.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        int currentIndex = 0;
        for (int i = 0; i < difficultyLevels.Count; i++)
        {
            string text = ExternalTextLoader.LoadText(difficultyLevels[i].namePath);
            options.Add(new TMP_Dropdown.OptionData(text));
            if (SettingsManager.instance.Settings.difficultyMultiplier == difficultyLevels[i].value)
            {
                currentIndex = i;
            }
        }

        difficultyChoise.AddOptions(options);
        difficultyChoise.SetValueWithoutNotify(currentIndex);
    }
    public void ChangeLanguage()
    {
        string langID = LocalizationSystem.GetAvailableLanguages()[languageChoise.value].id;
        LocalizationSystem.SetLanguage(langID);
    }
    public void OpenEditor()
    {
        SaveManger.Instance.Save();
        SceneManager.LoadScene("PixelEditor");
    }
    void OnDestroy()
    {
        LocalizationSystem.OnLanguageChanged -= UpdateLanguageDropDown;
    }
}
[Serializable]
public class DifficultyLevel
{
    public string namePath;
    public float value; 
}
