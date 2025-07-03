#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;

public class LocalizationTools : EditorWindow
{
    [MenuItem("Tools/Localization/Generate Language Profiles")]
    static void GenerateProfiles()
    {
        string locPath = Path.Combine(Application.streamingAssetsPath, "Localization");

        foreach (string langDir in Directory.GetDirectories(locPath))
        {
            string profilePath = Path.Combine(langDir, "languageProfile.json");

            if (!File.Exists(profilePath))
            {
                LanguageProfile profile = new LanguageProfile
                {
                    name = Path.GetFileName(langDir),
                    id = Path.GetFileName(langDir).ToLower()
                };

                string json = JsonUtility.ToJson(profile);
                File.WriteAllText(profilePath, json);
            }
        }

        AssetDatabase.Refresh();
    }
    [MenuItem("Tools/Localization/CreateMonologues")]
    static void CreateMonologues()
    {
        foreach (MonologueRule monologue in Resources.FindObjectsOfTypeAll<MonologueRule>())
        {
            string monologueText = "";
            if (monologue.monologue != null)
            {
                monologueText = monologue.monologue.text;
                Debug.Log(monologueText);
            }
            string fullPath = AssetDatabase.GetAssetPath(monologue);
            string relativePath = GetRelativePath(fullPath);

            monologue.monologue.textPath = Path.Combine("Monologues", relativePath);

            string newFile = Path.Combine(Application.streamingAssetsPath, "Localization", "russian", "Monologues", relativePath + ".txt");
            Debug.Log(newFile);
            string directory = Path.GetDirectoryName(newFile);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!File.Exists(newFile))
            {
                File.WriteAllText(newFile, monologueText);
            }
            EditorUtility.SetDirty(monologue);
            UnityEditor.AssetDatabase.Refresh();
        }
    }
    static string GetRelativePath(string fullPath)
    {
        int monologuesIndex = fullPath.IndexOf("Monologues/");
        if (monologuesIndex < 0) return string.Empty;

        string relative = fullPath.Substring(monologuesIndex + "Monologues/".Length);

        return Path.ChangeExtension(relative, null);
    }
    [MenuItem("Tools/Localization/CreateHats")]
    static void CreateHats()
    {
        foreach (Hat hat in Resources.FindObjectsOfTypeAll<Hat>())
        {
            hat.hatID = hat.name;
            hat.namePath = Path.Combine("Hats", hat.name);
            string newFile = Path.Combine(Application.streamingAssetsPath, "Localization", "russian", "Hats", hat.name + ".txt");
            if (!File.Exists(newFile))
            {
                string directory = Path.GetDirectoryName(newFile);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                if (!File.Exists(newFile))
                {
                    File.WriteAllText(newFile, hat.name);
                }
            }
            EditorUtility.SetDirty(hat);
            UnityEditor.AssetDatabase.Refresh();
        }
    }
    [MenuItem("Tools/Localization/CreateBeds")]
    static void CreateBeds()
    {
        foreach (BedSkin bed in Resources.FindObjectsOfTypeAll<BedSkin>())
        {
            bed.bedID = bed.name;
            bed.namePath = Path.Combine("Beds", bed.name);
            string newFile = Path.Combine(Application.streamingAssetsPath, "Localization", "russian", "Beds", bed.bedID + ".txt");
            if (!File.Exists(newFile))
            {
                string directory = Path.GetDirectoryName(newFile);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                if (!File.Exists(newFile))
                {
                    File.WriteAllText(newFile, bed.name);
                }
            }
            EditorUtility.SetDirty(bed);
            UnityEditor.AssetDatabase.Refresh();
        }
    }
}
#endif