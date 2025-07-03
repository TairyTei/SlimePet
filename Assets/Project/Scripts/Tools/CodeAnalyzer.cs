#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class CodeAnalyzer : EditorWindow
{
    private string folderPath = "Assets/Scripts";
    private int totalScripts;
    private int totalLines;
    private int totalChars;
    private bool includeComments = true;
    private bool includeEmptyLines = true;
    private Vector2 scrollPosition;
    private Dictionary<string, ScriptStats> scriptStats = new Dictionary<string, ScriptStats>();

    [MenuItem("Tools/Code Analyzer")]
    public static void ShowWindow()
    {
        GetWindow<CodeAnalyzer>("Code Analyzer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Code Analysis Settings", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);
        if (GUILayout.Button("Browse", GUILayout.Width(80)))
        {
            string path = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");
            if (!string.IsNullOrEmpty(path))
            {
                folderPath = path.Replace(Application.dataPath, "Assets");
            }
        }
        EditorGUILayout.EndHorizontal();

        includeComments = EditorGUILayout.Toggle("Include Comments", includeComments);
        includeEmptyLines = EditorGUILayout.Toggle("Include Empty Lines", includeEmptyLines);

        EditorGUILayout.Space();

        if (GUILayout.Button("Analyze Codebase"))
        {
            AnalyzeCodebase();
        }

        if (totalScripts > 0)
        {
            EditorGUILayout.Space();
            GUILayout.Label($"Results for: {folderPath}", EditorStyles.boldLabel);
            GUILayout.Label($"Total Scripts: {totalScripts}");
            GUILayout.Label($"Total Lines: {totalLines}");
            GUILayout.Label($"Total Characters: {totalChars:N0}");

            EditorGUILayout.Space();
            GUILayout.Label("Script Details:", EditorStyles.boldLabel);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (var stat in scriptStats)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(Path.GetFileName(stat.Key), GUILayout.Width(200));
                GUILayout.Label($"Lines: {stat.Value.Lines}", GUILayout.Width(100));
                GUILayout.Label($"Chars: {stat.Value.Characters}", GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private void AnalyzeCodebase()
    {
        totalScripts = 0;
        totalLines = 0;
        totalChars = 0;
        scriptStats.Clear();

        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"Directory not found: {folderPath}");
            return;
        }

        string fullPath = Path.Combine(Application.dataPath, folderPath.Replace("Assets/", ""));
        string[] files = Directory.GetFiles(fullPath, "*.cs", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            if (file.EndsWith(".meta")) continue;

            int fileLines = 0;
            int fileChars = 0;
            string relativePath = "Assets" + file.Replace(Application.dataPath, "");

            using (StreamReader reader = new StreamReader(file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!includeComments && IsCommentLine(line)) continue;
                    if (!includeEmptyLines && string.IsNullOrWhiteSpace(line)) continue;

                    fileLines++;
                    fileChars += line.Length;
                }
            }

            scriptStats.Add(relativePath, new ScriptStats(fileLines, fileChars));
            totalScripts++;
            totalLines += fileLines;
            totalChars += fileChars;
        }

        Debug.Log($"Analysis complete!\nScripts: {totalScripts}\nLines: {totalLines}\nChars: {totalChars}");
    }

    private bool IsCommentLine(string line)
    {
        string trimmed = line.Trim();
        return trimmed.StartsWith("//") ||
               trimmed.StartsWith("/*") ||
               trimmed.StartsWith("*");
    }

    private class ScriptStats
    {
        public int Lines { get; }
        public int Characters { get; }

        public ScriptStats(int lines, int characters)
        {
            Lines = lines;
            Characters = characters;
        }
    }
}
#endif