using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetCustomizationMenu : MonoBehaviour
{
    public Image petImage;
    public Image hatImage;
    public TMP_InputField nameField;
    public TextMeshProUGUI dateOfBirth;

    public TextMeshProUGUI currentLevel;
    public TextMeshProUGUI nextLevel;
    public Slider levelBar;

    void Start()
    {
        SkinManger.instance.OnSkinChanged += DrawSkins;
        DrawSkins();
    }

    public void Open()
    {
        PetStats stats = Pet.Instance.Stats;

        nameField.text = Pet.Instance.petName;

        DateTime createDate = Pet.Instance.CreateDate;
        int totalDays = (int)(DateTime.Now - createDate).TotalDays;

        int years = Mathf.FloorToInt(totalDays / 365);
        int month = Mathf.FloorToInt(totalDays % 365 / 30);
        int days = Mathf.FloorToInt(totalDays % 365 % 30);

        string text = "";
        if (years != 0) text += years + "y / ";
        if (month != 0) text += month + "mo / ";
        text += days + "d";
        dateOfBirth.text = text;

        currentLevel.text = stats.Level + "";
        nextLevel.text = (stats.Level + 1) + "";
        int prevExp = PetStats.CalculateExpirienceFromLvl(stats.Level);
        int currExp = stats.Expirience;
        int requiredExp = PetStats.CalculateExpirienceFromLvl(stats.Level + 1);
        levelBar.value = (float)(currExp - prevExp) / (float)(requiredExp - prevExp);
    }
    public void ChangeName()
    {

        string newName = nameField.text;
        if (!string.IsNullOrEmpty(newName) && newName.Length <= 20)
        {
            PlayerPrefs.SetString("PetName", newName);
            Pet.Instance.petName = newName;
        }
        else
        {
        
        }
    }
    private void DrawSkins()
    {
        SkinManger sm = SkinManger.instance;
        petImage.sprite = sm.GetSprite(petImage.sprite.name);
        Sprite hatSprite = sm.allHats.hats[sm.ActiveHatID].hatSprite;
        if (hatSprite != null)
        {
            hatImage.sprite = hatSprite;
            ImageSpriteResizer.Resize(hatImage);
            hatImage.gameObject.SetActive(true);
        }
        else
        {
            hatImage.gameObject.SetActive(false);
        }
    }
}
