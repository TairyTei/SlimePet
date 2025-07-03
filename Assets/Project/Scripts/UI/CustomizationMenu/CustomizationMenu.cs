using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CustomizationMenu : MonoBehaviour
{
    public GameObject menu;
    public PetCustomizationMenu petCustomizationMenu;
    public HatSelectionMenu hatSelectionMenu;
    public BedSelectionMenu bedSelectionMenu;
    public GameObject[] tabs;

    public void OpenTab(int tabIndex){
        if(tabIndex >= tabs.Length) return;

        for(int i = 0; i < tabs.Length; i++){
            bool isCurrent = i == tabIndex ? true : false;
            tabs[i].SetActive(isCurrent);
        }
    }
    public void Open(){
        if (menu.activeSelf)
        {
            Close();
            return;
        }

        OpenTab(0);
        BedMenu.instance.Close();
        menu.SetActive(true);

        petCustomizationMenu.Open();
        //hatSelectionMenu.UpdateHatList();
        //bedSelectionMenu.UpdateBedList();
    }
    public void Close(){
        menu.SetActive(false);
    }
}
