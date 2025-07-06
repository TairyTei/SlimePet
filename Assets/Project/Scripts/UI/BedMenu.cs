using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BedMenu : MonoBehaviour
{
    public RectTransform menu;
    public GameObject Options;
    public Vector2 offset = new Vector2(0, 20);
    public bool isOpened;
    private Pet pet;
    public static BedMenu instance;
    
    void Start()
    {
        instance = this;
        pet = Pet.Instance;    
    }
    private void Update()
    {

        if (isOpened)
        {
            menu.transform.position = (Vector2)Camera.main.WorldToScreenPoint(Bed.instance.transform.position) + offset;
        }
    }
    public void Open()
    {
        if (isOpened)
        {
            Close();
            return;
        }
	    PetMenu.instance.Close();

         menu.transform.position = (Vector2)Camera.main.WorldToScreenPoint(Bed.instance.transform.position) + offset;
         Options.SetActive(true);
         menu.gameObject.SetActive(true);
         isOpened = true;
         
    }
    public void Close()
    {
        if (!isOpened) return;
        
        menu.gameObject.SetActive(false);
        isOpened = false;
    }
    public void OpenFurnitureBox(){
        FurnitureBox.instance.Open();
        Close();
    }
    public void QuitGame(){
        SaveManger.Save();
        Application.Quit();
    }
}
