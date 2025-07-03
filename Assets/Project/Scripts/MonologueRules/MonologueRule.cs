
using System.Diagnostics;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Monologue", menuName = "My/MonologueRules/Monologue", order = 0)]
public class MonologueRule : LocalizableSO
{
    public Monologue monologue;
    public virtual bool Condition()
    {
        return true;
    }
    public override void OnLanguageChanged()
    {
        monologue.OnLanguageChanged();
    }
    [ContextMenu("OpenTxt")]
    public void OpenPath()
    {
        string filePath = Path.Combine(Application.dataPath, "Localization", "russian", monologue.textPath + ".txt");
        if (!File.Exists(filePath))
        {
            UnityEngine.Debug.Log("dsasd");
            return;
        }
        Process.Start(new ProcessStartInfo(filePath) 
        {
            UseShellExecute = true
        });
    }
}