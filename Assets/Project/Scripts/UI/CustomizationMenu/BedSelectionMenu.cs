using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BedSelectionMenu : MonoBehaviour
{
    public TextMeshProUGUI nameOfBed;
    public GameObject priceOfBedPanel;
    public TextMeshProUGUI priceOfBed;
    public TextMeshProUGUI buttonText;
    public Image bedImage;
    public Transform bedListParent;
    public GameObject bedButtonPrefab;
    public AllBeds allBeds;
    public AudioClip buySound;
    public AudioClip cancelSound;
    private BedSkin openedBed;
    private int openedBedID;
    private bool isOpenedBedBought;
     private string PickedText {get{ return ExternalTextLoader.LoadText("Customization/Picked"); }}
    private string PickText {get{ return ExternalTextLoader.LoadText("Customization/Pick"); }}
    private string BuyText {get{ return ExternalTextLoader.LoadText("Customization/Buy"); }}
    void Start()
    {
        UpdateBedList();
    }
     public void OpenBed(BedSkin bed)
    {
        openedBed = bed;
        openedBedID = allBeds.beds.IndexOf(bed);
        if (SkinManger.instance.GetActiveBed() == bed)
        {
            buttonText.text = PickedText;
        }
        else
        {
            isOpenedBedBought = (openedBedID == 0 || PlayerPrefs.GetInt("IsBed" + openedBedID + "Bought") == 1);
            buttonText.text = isOpenedBedBought ? PickText : BuyText;
        }

        if (isOpenedBedBought)
        {
            priceOfBedPanel.SetActive(false);
        }
        else
        {
            priceOfBed.text = bed.price + "c";
            priceOfBedPanel.SetActive(true);
        }

        nameOfBed.text = bed.BedName;
        if (bed.bedSprite != null)
        {
            bedImage.sprite = bed.bedSprite;
            bedImage.gameObject.SetActive(true);
        }
        else
        {
            bedImage.gameObject.SetActive(false);
        }
    }
      public void UpdateBedList(){
        foreach(Transform child in bedListParent){
            Destroy(child.gameObject);
        }
        bool isSomeBedOpened = false;
        for (int i = 0; i < allBeds.beds.Count; i++)
        {
            GameObject createdButton = Instantiate(bedButtonPrefab, bedListParent);
            ChooseBedButton button = createdButton.GetComponent<ChooseBedButton>();
            button.Set(this, allBeds.beds[i]);

            if (!isSomeBedOpened)
            {
                OpenBed(allBeds.beds[i]);
                isSomeBedOpened = true;
            }
        }
        RectTransform rt = bedListParent.GetComponent<RectTransform>();
        Vector2 scale = new Vector2(rt.transform.localScale.x, 80 * allBeds.beds.Count);
        //rt.rect.Set(rt.rect.x, rt.rect.y, scale.x, scale.y);
        rt.sizeDelta = scale;
    }
   
    public void PickButton(){
        if (isOpenedBedBought)
        {
            if (SkinManger.instance.GetActiveBed() == openedBed)
            {
                SoundManager.instance.PlaySound(cancelSound, 0, SoundType.System);
                return;
            }
            int id = allBeds.beds.IndexOf(openedBed);
            SkinManger.instance.SetBed(id);
            OpenBed(openedBed);
        }
        else
        {
            if (Pet.Instance.Stats.Coins < openedBed.price)
            {
                SoundManager.instance.PlaySound(cancelSound, 0, SoundType.System);
                return;
            }
            
            Pet.Instance.Stats.Coins -= openedBed.price;
            PlayerPrefs.SetInt("Coins", Pet.Instance.Stats.Coins);
            PlayerPrefs.SetInt("IsBed" + openedBedID + "Bought", 1);
            OpenBed(openedBed);
            SoundManager.instance.PlaySound(buySound, 0, SoundType.System);
        }
    }
}
