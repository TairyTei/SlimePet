using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private Sprite aimCursorSprite;
    public Sprite AimCursorSprite { get { return aimCursorSprite; } }
    [SerializeField] private AudioClip hitSound;
    public AudioClip HitSound { get { return hitSound; } }
    [SerializeField] private AudioClip missSound;
    public AudioClip MissSound { get { return missSound; } }
    [SerializeField] private AudioClip recordSound;
    public AudioClip RecordSound { get { return recordSound; } }
    [SerializeField] private GameObject hideAndSeekUI;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI recordText;

    public void SetActive_HideAndSeekUI(bool active)
    {
        hideAndSeekUI.SetActive(active);
    }
    public void SetValue_TimeSlider(float value)
    {
        timeSlider.value = value;
    }
    public void SetActive_MinigameUI(bool active)
    {
        minigameUI.SetActive(active);
    }
    public void SetValue_Score(int value)
    {
        scoreText.text = value.ToString();
    }
    public void SetValue_Record(int value)
    {
        recordText.text = value.ToString();
    }
}
