using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuideBook : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;
    [SerializeField] private GameObject[] pages;
    [SerializeField] private AudioClip switchPageSound;
    [SerializeField] private TMP_Dropdown languageChoise;
    private int currentPage = 0;

    void Start()
    {
        DrawLanguageDropdown();
        if (SaveManger.IsItFirstLaunch)
        {
            Open();
        }
        LocalizationSystem.OnLanguageChanged += UpdateLanguageDropDown;
    }

    public void Open()
    {
        OpenPage(0);
        menu.SetActive(true);
    }
    public void Close()
    {
        menu.SetActive(false);
    }
    private void OpenPage(int index)
    {
        if (index > pages.Length) return;

        currentPage = index;
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }

        bool previousButtonActive = currentPage != 0;
        previousButton.SetActive(previousButtonActive);

        bool nextButtoActive = currentPage < pages.Length - 1;
        nextButton.SetActive(nextButtoActive);
    }
    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
            OpenPage(currentPage + 1);

        if (switchPageSound != null)
        {
            SoundManager.instance.PlaySound(switchPageSound, 0, SoundType.System);
        }
    }
    public void PreviousPage()
    {
        if (currentPage > 0)
            OpenPage(currentPage - 1);

        if (switchPageSound != null)
        {
            SoundManager.instance.PlaySound(switchPageSound, 0, SoundType.System);
        }
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
    public void ChangeLanguage()
    {
        string langID = LocalizationSystem.GetAvailableLanguages()[languageChoise.value].id;
        LocalizationSystem.SetLanguage(langID);
    }
    void OnDestroy()
    {
        LocalizationSystem.OnLanguageChanged -= UpdateLanguageDropDown;
    }
}
