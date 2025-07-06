using UnityEngine;

[System.Serializable]
public class Monologue
{
    [TextArea(10, 60)]
    public string text;
    public string textPath;
    public string Text { get; private set; }
    public const float time = 3;

    public Monologue(string text)
    {
        this.text = text;
    }
    [ContextMenu("UpdateTextVariable")]
    public void UpdateVar()
    {
        //ruText = text;
    }
    public void OnLanguageChanged()
    {
        Text = ExternalTextLoader.LoadText(textPath);
    }
}