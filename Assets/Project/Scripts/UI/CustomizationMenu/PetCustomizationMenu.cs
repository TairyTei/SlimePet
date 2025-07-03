using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetCustomizationMenu : MonoBehaviour
{
    public TMP_InputField nameField;
    public TextMeshProUGUI dateOfBirth;
    public TextMeshProUGUI coinCount;

    public TextMeshProUGUI currentLevel;
    public TextMeshProUGUI nextLevel;
    public Slider levelBar;

    public void Open()
    {
        PetStats stats = Pet.Instance.Stats;

        nameField.text = Pet.Instance.petName;
        coinCount.text = stats.Coins + "c";

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
}
