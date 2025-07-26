using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetMenu : MonoBehaviour, IInitializable
{
    public RectTransform menu;
    [SerializeField] private float autoCloseIn = 10;
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
    private IEnumerator currentAnimCoroutine;
    private IEnumerator autoClose;

    public void Initialize()
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

        if (pet.TaskCount != 0 && !pet.GetActiveTask().IsOpenMenuAllowed) return;

        BedMenu.instance.Close();

        if (currentAnimCoroutine != null)
            StopCoroutine(currentAnimCoroutine);

        menu.transform.position = (Vector2)Camera.main.WorldToScreenPoint(pet.transform.position) + offset;
        Options.SetActive(true);
        FoodList.SetActive(false);
        menu.gameObject.SetActive(true);
        pet.AddIdleRequest("PetMenu");
        isOpened = true;
        UpdateStatSliders();
        optionsAnimator.SetBool("IsOpened", true);
        variablesAnimator.SetBool("IsOpened", true);
        StartAutoClose();
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
        StartAutoClose();
    }
    public void Close()
    {
        if (!isOpened) return;

        currentAnimCoroutine = IECloseAnim();
        StartCoroutine(currentAnimCoroutine);

        pet.RemoveIdleRequest("PetMenu");
        isOpened = false;
        if (autoClose != null)
        {
            StopCoroutine(autoClose);
        }
        SoundManager.instance.PlaySound(closeSound, 0.1f, SoundType.System);
    }
    IEnumerator IECloseAnim()
    {
        optionsAnimator.SetBool("IsOpened", false);
        variablesAnimator.SetBool("IsOpened", false);
        yield return new WaitForSeconds(0.25f);
        menu.gameObject.SetActive(false);
    }
    public void StartAutoClose()
    {
        if (autoClose != null)
        {
            StopCoroutine(autoClose);
        }
        autoClose = IAutoClose();
        StartCoroutine(autoClose);
    }
    IEnumerator IAutoClose()
    {
        yield return new WaitForSeconds(autoCloseIn);
        Close();
    }
}
