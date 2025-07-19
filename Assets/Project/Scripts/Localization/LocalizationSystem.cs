using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public static class LocalizationSystem
{
    private static List<LanguageProfile> availableLanguages = new List<LanguageProfile>();
    private static LanguageProfile currentLanguage;
    public static LanguageProfile CurrentLanguage {get{ return currentLanguage; }}
    public const string standardLanguage = "russian";
    
    public static event Action OnLanguageChanged;
     public static event Action OnLanguagesScanned;
    
    public static void Initialize()
    {
        ScanForLanguages();
        string savedLang = PlayerPrefs.GetString("CurrentLanguage");
        if (savedLang != "")
        {
            SetLanguage(savedLang);
        }
        else
        {
            SetLanguage(standardLanguage);
        }
    }

    public static void ScanForLanguages()
    {
        availableLanguages.Clear();

        string localizationPath = Path.Combine(Application.streamingAssetsPath, "Localization");
        if (!Directory.Exists(localizationPath))
        {
            Debug.LogError("Localization folder not found!");
            return;
        }

        foreach (string langFolder in Directory.GetDirectories(localizationPath))
        {
            string profilePath = Path.Combine(langFolder, "languageProfile.json");

            if (File.Exists(profilePath))
            {
                string json = File.ReadAllText(profilePath);
                LanguageProfile profile = JsonUtility.FromJson<LanguageProfile>(json);
                availableLanguages.Add(profile);
            }
        }

        Debug.Log($"Found {availableLanguages.Count} languages");
        
        OnLanguagesScanned?.Invoke();
    }
     public static LanguageProfile[] GetAvailableLanguages()
    {
        return availableLanguages.ToArray();
    }
     public static bool SetLanguage(string textID)
    {
        LanguageProfile profile = availableLanguages.Find(x => x.id == textID);
        if (profile != null)
        {
            currentLanguage = profile;
            PlayerPrefs.SetString("CurrentLanguage", textID);
            OnLanguageChanged?.Invoke();

            RefreshAllLocalizables();
            return true;
        }
        return false;
    }

    private static void RefreshAllLocalizables()
    {
        foreach (var localizable in Resources.FindObjectsOfTypeAll<LocalizableSO>())
        {
            localizable.OnLanguageChanged();
        }
        foreach (var localizable in Resources.FindObjectsOfTypeAll<LocalizableMB>())
        {
            localizable.OnLanguageChanged();
        }
    }
}