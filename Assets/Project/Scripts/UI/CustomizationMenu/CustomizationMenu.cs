using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CustomizationMenu : MonoBehaviour
{
    public GameObject menu;
    public TextMeshProUGUI coinCount;
    public PetCustomizationMenu petCustomizationMenu;
    public HatSelectionMenu hatSelectionMenu;
    public BedSelectionMenu bedSelectionMenu;
    public GameObject[] tabs;

    void Start()
    {
        Pet.Instance.Stats.OnCoinsUpdated += UpdateCoins;
    }
    public void OpenTab(int tabIndex)
    {
        if (tabIndex >= tabs.Length) return;

        for (int i = 0; i < tabs.Length; i++)
        {
            bool isCurrent = i == tabIndex ? true : false;
            tabs[i].SetActive(isCurrent);
        }
    }
    public void Open()
    {
        if (menu.activeSelf)
        {
            Close();
            return;
        }

        OpenTab(0);
        BedMenu.instance.Close();

        petCustomizationMenu.Open();

        Hat activeHat = SkinManger.instance.GetActiveHat();
        hatSelectionMenu.OpenHat(activeHat);
        BedSkin activeBed = SkinManger.instance.GetActiveBed();
        bedSelectionMenu.OpenBed(activeBed);
        UpdateCoins();
        menu.SetActive(true);
        //hatSelectionMenu.UpdateHatList();
        //bedSelectionMenu.UpdateBedList();
    }
    public void Close()
    {
        menu.SetActive(false);
    }
    public void UpdateCoins()
    {
        PetStats stats = Pet.Instance.Stats;
        coinCount.text = stats.Coins + "c";
    }
    void OnDestroy()
    {
        Pet.Instance.Stats.OnCoinsUpdated -= UpdateCoins;
    }
}
