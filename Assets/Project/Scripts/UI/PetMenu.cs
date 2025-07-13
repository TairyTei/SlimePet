using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetMenu : MonoBehaviour
{
    public RectTransform menu;
    [SerializeField] private Animator optionsAnimator;
    [SerializeField] private Animator variablesAnimator;
    [SerializeField] private AudioClip openSound, closeSound;
    public GameObject Options;
    public GameObject FoodList;
    public BuyFoodButton[] foodButtons;
    public Slider satietySlider;
    public Slider cleanlinessSlider;
    public Slider loveSlider;
    public Slider cheerfulnessSlider;

    public Pet pet;
    public Vector2 offset = new Vector2(0, 20);
    private bool isOpened;
    public bool IsOpened { get { return isOpened; } }
    public static PetMenu instance;

    void Start()
    {
        instance = this;
    }
    private void Update()
    {

        if (isOpened)
        {
            menu.transform.position = (Vector2)Camera.main.WorldToScreenPoint(pet.transform.position) + offset;
        }
    }
    public void UpdateStatSliders()
    {
        if (!isOpened) return;

        if (satietySlider != null)
            satietySlider.value = pet.Stats.Satiety;
        if (cleanlinessSlider != null)
            cleanlinessSlider.value = pet.Stats.Cleanliness;
        if (loveSlider != null)
            loveSlider.value = pet.Stats.Love;
        if (cheerfulnessSlider != null)
            cheerfulnessSlider.value = pet.Stats.Cheerfulness;
    }
    public void Open()
    {
        if (isOpened)
        {
            Close();
            return;
        }
        BedMenu.instance.Close();

        menu.transform.position = (Vector2)Camera.main.WorldToScreenPoint(pet.transform.position) + offset;
        Options.SetActive(true);
        FoodList.SetActive(false);
        menu.gameObject.SetActive(true);
        pet.AddIdleRequest("PetMenu");
        isOpened = true;
        UpdateStatSliders();
        optionsAnimator.SetBool("IsOpened", true);
        variablesAnimator.SetBool("IsOpened", true);
        SoundManager.instance.PlaySound(openSound, 0.1f, SoundType.System);
    }
    public void OpenFoodList()
    {
        Options.SetActive(false);
        foreach (BuyFoodButton button in foodButtons)
        {
            button.UpdateButton();
        }
        FoodList.SetActive(true);
    }
    public void Close()
    {
        if (!isOpened) return;
        
        StartCoroutine(IECloseAnim());
        pet.RemoveIdleRequest("PetMenu");
        isOpened = false;
        SoundManager.instance.PlaySound(closeSound, 0.1f, SoundType.System);
    }
    IEnumerator IECloseAnim()
    {
        optionsAnimator.SetBool("IsOpened", false);
        variablesAnimator.SetBool("IsOpened", false);
        yield return new WaitForSeconds(0.25f);
        menu.gameObject.SetActive(false);
    }
}
