using UnityEngine;

[System.Serializable]
public class Settings
{
    public string language = "russian";

    [Range(0f, 1f)]
    public float musicVolume = 0.25f;
    [Range(0f, 1f)]
    public float mainVolume = 0.25f;
    [Range(0f, 1f)]
    public float systemVolume = 0.25f;
    public int frameRate = -1;
    public bool displayStoryMark = true;
    public bool pinWindowOnTop = true;
}
