using System.IO;
using UnityEngine;

public static class ExternalTextLoader
{
    public static string LoadText(string relativePath)
    {
        string currLang = LocalizationSystem.CurrentLanguage.id;
        string fullPath = Path.Combine(Application.streamingAssetsPath, "Localization", currLang, relativePath + ".txt");
        //Debug.Log(fullPath);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"Text file not found: {fullPath}");
            return "MISSING_TEXT :D";
        }
        
        return File.ReadAllText(fullPath);
    }

#if UNITY_EDITOR
    public static void CreateTextFileIfMissing(string relativePath, string defaultContent = "")
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, "Localization", relativePath);
        string directory = Path.GetDirectoryName(fullPath);
        
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        
        if (!File.Exists(fullPath))
            File.WriteAllText(fullPath, defaultContent);
    }
#endif
}