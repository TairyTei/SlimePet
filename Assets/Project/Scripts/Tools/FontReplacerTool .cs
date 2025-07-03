#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System;

public class FontReplacerTool : EditorWindow
{
    private TMP_FontAsset sourceFont;
    private TMP_FontAsset targetFont;
    private bool includeInactive = true;
    private bool includePrefabs = true;
    private bool showResults = true;

    [MenuItem("Tools/Font Replacer")]
    public static void ShowWindow()
    {
        GetWindow<FontReplacerTool>("Font Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Font Replacement Settings", EditorStyles.boldLabel);

        sourceFont = (TMP_FontAsset)EditorGUILayout.ObjectField(
            "Current Font (Optional)",
            sourceFont,
            typeof(TMP_FontAsset),
            false
        );

        targetFont = (TMP_FontAsset)EditorGUILayout.ObjectField(
            "New Font",
            targetFont,
            typeof(TMP_FontAsset),
            true
        );

        EditorGUILayout.Space();
        includeInactive = EditorGUILayout.Toggle("Include Inactive Objects", includeInactive);
        includePrefabs = EditorGUILayout.Toggle("Include Prefabs", includePrefabs);
        showResults = EditorGUILayout.Toggle("Show Results", showResults);

        EditorGUILayout.Space();

        if (GUILayout.Button("Replace Fonts in Current Scene"))
        {
            if (targetFont == null)
            {
                Debug.LogError("Please select a target font!");
                return;
            }

            ReplaceFontsInScene();
        }

        if (GUILayout.Button("Replace Fonts in All Scenes"))
        {
            if (targetFont == null)
            {
                Debug.LogError("Please select a target font!");
                return;
            }

            ReplaceFontsInAllScenes();
        }
    }

    private void ReplaceFontsInScene()
    {
        if (targetFont == null) return;

        int replacedCount = 0;
        TextMeshProUGUI[] textComponents = GameObject.FindObjectsOfType<TextMeshProUGUI>(includeInactive);

        foreach (TextMeshProUGUI textComp in textComponents)
        {
            // Если указан sourceFont, заменяем только его
            if (sourceFont != null && textComp.font != sourceFont) continue;

            Undo.RecordObject(textComp, "Change TMP Font");
            textComp.font = targetFont;
            replacedCount++;

            // Обработка префабов
            if (includePrefabs && PrefabUtility.IsPartOfAnyPrefab(textComp))
            {
                GameObject prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(textComp);
                PrefabUtility.ApplyPrefabInstance(prefabRoot, InteractionMode.UserAction);
            }
        }

        if (showResults)
        {
            Debug.Log($"Replaced fonts on {replacedCount} TextMeshPro components in current scene");
        }
    }

    private void ReplaceFontsInAllScenes()
    {
        if (targetFont == null) return;

        int totalReplaced = 0;
        string[] scenePaths = GetAllScenePaths();

        foreach (string scenePath in scenePaths)
        {
            EditorUtility.DisplayProgressBar(
                "Processing Scenes",
                $"Scanning: {System.IO.Path.GetFileNameWithoutExtension(scenePath)}",
                (float)Array.IndexOf(scenePaths, scenePath) / scenePaths.Length
            );

            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(scenePath);
            int sceneReplaced = 0;

            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject root in rootObjects)
            {
                TextMeshProUGUI[] textComponents = root.GetComponentsInChildren<TextMeshProUGUI>(includeInactive);

                foreach (TextMeshProUGUI textComp in textComponents)
                {
                    if (sourceFont != null && textComp.font != sourceFont) continue;

                    Undo.RecordObject(textComp, "Change TMP Font");
                    textComp.font = targetFont;
                    sceneReplaced++;
                }
            }

            if (sceneReplaced > 0)
            {
                EditorSceneManager.SaveScene(scene);
                totalReplaced += sceneReplaced;

                if (showResults)
                {
                    Debug.Log($"Replaced {sceneReplaced} fonts in scene: {scene.name}");
                }
            }
        }

        EditorUtility.ClearProgressBar();

        if (showResults)
        {
            Debug.Log($"Total fonts replaced across all scenes: {totalReplaced}");
        }
    }

    private string[] GetAllScenePaths()
    {
        List<string> scenePaths = new List<string>();

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                scenePaths.Add(scene.path);
            }
        }
        return scenePaths.ToArray();
    }
}
#endif