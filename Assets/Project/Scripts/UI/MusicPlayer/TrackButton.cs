using System;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackButton : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI timeText;
    private MusicPlayer musicPlayer;
    public AudioClip AudioClip { get; private set; }
    public bool IsEnabled { get; private set; }
    public void Set(MusicPlayer musicPlayer, AudioClip audioClip)
    {
        this.musicPlayer = musicPlayer;
        this.AudioClip = audioClip;
        IsEnabled = PlayerPrefs.GetInt("IsSoundTrack" + AudioClip.name + "Enabled") == 1;
        title.text = audioClip.name;
        TimeSpan timeSpan = TimeSpan.FromSeconds(audioClip.length);
        timeText.text = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        toggle.isOn = IsEnabled;
    }
    public void EnableToggle()
    {
        IsEnabled = toggle.isOn;
        PlayerPrefs.SetInt("IsSoundTrack" + AudioClip.name + "Enabled", IsEnabled ? 1 : 0);
    }
    public void Up()
    {
        musicPlayer.MoveUp(AudioClip);
    }
    public void Down()
    {
        musicPlayer.MoveDown(AudioClip);
    }
    public void PlayThis()
    {
        musicPlayer.PlayTrack(AudioClip);
    }
}