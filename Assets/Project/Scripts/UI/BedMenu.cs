using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BedMenu : MonoBehaviour
{
    public RectTransform menu;
    [SerializeField] private Animator menuAnimator;
       [SerializeField] private AudioClip openSound, closeSound;
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
        menuAnimator.SetBool("IsOpened", true);
         SoundManager.instance.PlaySound(openSound, 0.1f, SoundType.System);
    }
    public void Close()
    {
        if (!isOpened) return;

        isOpened = false;
        StartCoroutine(IECloseAnim());
        SoundManager.instance.PlaySound(closeSound, 0.1f, SoundType.System);
    }
    IEnumerator IECloseAnim()
    {
        menuAnimator.SetBool("IsOpened", false);
        yield return new WaitForSeconds(0.25f);
        menu.gameObject.SetActive(false);
    }
    public void OpenFurnitureBox()
    {
        FurnitureBox.instance.Open();
        Close();
    }
    public void QuitGame(){
        SaveManger.Instance.Save();
        Application.Quit();
    }
}
