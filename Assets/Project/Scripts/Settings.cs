using UnityEngine;

[System.Serializable]
public class Settings
{
    public string language = "russian";

    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float mainVolume = 1f;
    [Range(0f, 1f)]
    public float systemVolume = 1f;
    public int frameRate = -1;
    public bool displayStoryMark = true;
    public bool pinWindowOnTop = true;
}
