using UnityEngine;

[System.Serializable]
public class Monologue
{
    [TextArea(10, 60)]
    public string text;
    public string textPath;
    public string Text { get; private set; }
    public float time;

    public Monologue(string text, float time)
    {
        this.text = text;
        this.time = time;
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