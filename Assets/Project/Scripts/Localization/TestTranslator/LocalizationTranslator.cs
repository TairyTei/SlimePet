#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationTranslator : EditorWindow
{
    private string sourceLanguage = "ru";
    private string targetLanguage = "en";
    private string sourceFolder = "Assets/Localization/Russian";
    private string targetFolder = "Assets/Localization/English";

    [MenuItem("Tools/Localization Translator")]
    public static void ShowWindow()
    {
        GetWindow<LocalizationTranslator>("Localization Translator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Translation Settings", EditorStyles.boldLabel);
        sourceLanguage = EditorGUILayout.TextField("Source Language", sourceLanguage);
        targetLanguage = EditorGUILayout.TextField("Target Language", targetLanguage);
        sourceFolder = EditorGUILayout.TextField("Source Folder", sourceFolder);
        targetFolder = EditorGUILayout.TextField("Target Folder", targetFolder);

        if (GUILayout.Button("Translate All Files"))
        {
            TranslateAllFiles();
        }
    }

    private void TranslateAllFiles()
    {
        if (!Directory.Exists(sourceFolder))
        {
            Debug.LogError($"Source folder not found: {sourceFolder}");
            return;
        }

        // Создаем целевую папку если ее нет
        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        // Получаем все txt файлы рекурсивно
        string[] files = Directory.GetFiles(sourceFolder, "*.txt", SearchOption.AllDirectories);
        int translatedCount = 0;

        foreach (string sourceFile in files)
        {
            // Получаем относительный путь
            string relativePath = sourceFile.Substring(sourceFolder.Length);
            string targetFile = Path.Combine(targetFolder, relativePath);

            // Создаем папки если их нет
            string targetDirectory = Path.GetDirectoryName(targetFile);
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            // Читаем исходный файл
            string sourceText = File.ReadAllText(sourceFile);
            
            // Пропускаем пустые файлы
            if (string.IsNullOrWhiteSpace(sourceText))
                continue;

            // Переводим текст
            string translatedText = Translator.TranslateGoogle(sourceText, targetLanguage);

            // Сохраняем перевод
            File.WriteAllText(targetFile, translatedText);
            translatedCount++;

            // Обновляем прогресс бар
            float progress = (float)translatedCount / files.Length;
            EditorUtility.DisplayProgressBar(
                "Translating files...", 
                $"{translatedCount}/{files.Length} {Path.GetFileName(sourceFile)}", 
                progress);
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        Debug.Log($"Successfully translated {translatedCount} files to {targetLanguage}");
    }
}
#endif