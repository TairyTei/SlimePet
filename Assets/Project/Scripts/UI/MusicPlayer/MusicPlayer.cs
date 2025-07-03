using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Slider trackProgress;
    [SerializeField] private GameObject trackButtonPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private List<AudioClip> trackList = new List<AudioClip>();
    private int currentTrack = 0;
    private List<TrackButton> buttons = new List<TrackButton>();
    [SerializeField] private AudioSource source;
    [SerializeField] private Animator radioAnimator;
    private bool isPause = false;
    private bool isOpened = false;
    void Start()
    {
        DrawTrackList();
        source.volume = SettingsManager.insntace.Settings.musicVolume;
        PlayTrack(PlayerPrefs.GetInt("CurrentSoundTrack"));
    }

    void Update()
    {
        if (source.isPlaying)
        {

        }
        else if (!source.isPlaying && !isPause) Next();
    }
    public void Open()
    {
        if (isOpened)
        {
            Close();
            return;
        }
        DrawUpdate();
        menu.SetActive(true);
        isOpened = true;
        StartCoroutine(EverySecondUpdate());
        
    }
    public void Close()
    {
        menu.SetActive(false);
        isOpened = false;
        StopCoroutine("EverySecondUpdate");
    }
    public void SetVolume(float volume)
    {
        source.volume = volume;
    }
    private void SortButtons()
    {
        List<(Transform transform, int id)> childList = new List<(Transform, int)>();

        foreach (Transform child in parent)
        {
            TrackButton button = child.GetComponent<TrackButton>();
            if (button != null && button.AudioClip != null)
            {
                int id = trackList.IndexOf(button.AudioClip);
                if (id >= 0)
                {
                    childList.Add((child, id));
                }
            }
        }

        childList.Sort((a, b) => a.id.CompareTo(b.id));

        for (int i = 0; i < childList.Count; i++)
        {
            childList[i].transform.SetSiblingIndex(i);
        }
    }
    private void DrawTrackList()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
        buttons.Clear();

        for (int i = 0; i < trackList.Count; i++)
        {
            GameObject buttonObj = Instantiate(trackButtonPrefab, parent);
            TrackButton button = buttonObj.GetComponent<TrackButton>();
            button.Set(this, trackList[i]);
            buttons.Add(button);
        }
    }
    private TrackButton GetButtonOfTrack(int trackID)
    {
        AudioClip track = trackList[trackID];
        foreach (TrackButton button in buttons)
        {
            if (button.AudioClip == track)
            {
                return button;
            }
        }
        return null;
    }
    private void DrawUpdate()
    {
        trackProgress.value = source.time / source.clip.length;
        TimeSpan timeSpan = TimeSpan.FromSeconds(source.time);
        string time = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        TimeSpan fullTimeSpan = TimeSpan.FromSeconds(source.clip.length);
        string fullTime = $"{fullTimeSpan.Minutes:00}:{fullTimeSpan.Seconds:00}";
        timeText.text = time + " / " + fullTime;
    }
    IEnumerator EverySecondUpdate()
    {
        yield return new WaitForSeconds(1f);
        DrawUpdate();
        StartCoroutine(EverySecondUpdate());
    }
    public void Next()
    {
        for (int i = 0; i < trackList.Count; i++)
        {
            int nextID = (currentTrack + (i + 1)) % trackList.Count;
            if (GetButtonOfTrack(nextID).IsEnabled)
            {
                PlayTrack(nextID);
                return;
            }
        }


    }
    public void PlayTrack(int id)
    {
        if (trackList.Count < id) return;
        currentTrack = id;
        PlayerPrefs.SetInt("CurrentSoundTrack", id);
        source.clip = trackList[id];
        source.Play();
        title.text = trackList[id].name;
        radioAnimator.SetBool("Dance", true);
    }
    public void PlayTrack(AudioClip audioClip)
    {
        if (!trackList.Contains(audioClip)) return;

        int id = trackList.IndexOf(audioClip);
        PlayTrack(id);
    }
    public void PauseButton()
    {
        isPause = !isPause;
        if (isPause)
        {
            source.Pause();
            radioAnimator.SetBool("Dance", false);
        }
        else
        {
            source.Play();
            radioAnimator.SetBool("Dance", true);
        }
    }
    public void MoveUp(AudioClip clip)
    {
        int index = trackList.IndexOf(clip);
        int upperIndex;
        if (index > 0)
            upperIndex = index - 1;
        else
            upperIndex = trackList.Count - 1;


        (trackList[upperIndex], trackList[index]) = (trackList[index], trackList[upperIndex]);

        SortButtons();

        if (currentTrack == index) currentTrack--;
        else if (currentTrack == index - 1) currentTrack++;
    }
    public void MoveDown(AudioClip clip)
    {
        int index = trackList.IndexOf(clip);
        int upperIndex = (index + 1) % trackList.Count;

        (trackList[upperIndex], trackList[index]) = (trackList[index], trackList[upperIndex]);

        SortButtons();

        if (currentTrack == index) currentTrack--;
        else if (currentTrack == index - 1) currentTrack++;
    }
    public void ScrollTrack()
    {
        source.time = source.clip.length * trackProgress.value;
    }
}
