using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HatSelectionMenu : MonoBehaviour
{
    //Hats
    public TextMeshProUGUI nameOfHat;
    public GameObject priceOfHatPanel;
    public TextMeshProUGUI priceOfHat;
    public TextMeshProUGUI buttonText;
    public Image hatImage;
    public Transform hatListParent;
    public GameObject hatButtonPrefab;
    public AllHats allHats;
    public AudioClip buySound;
    public AudioClip cancelSound;
    private Hat openedHat;
    private int openedHatID;
    private bool isOpenedHatBought;
    private string PickedText {get{ return ExternalTextLoader.LoadText("Customization/Picked"); }}
    private string PickText {get{ return ExternalTextLoader.LoadText("Customization/Pick"); }}
    private string BuyText {get{ return ExternalTextLoader.LoadText("Customization/Buy"); }}
    void Start()
    {
        UpdateHatList();
    }
     public void OpenHat(Hat hat)
    {
        openedHat = hat;
        openedHatID = allHats.hats.IndexOf(hat);
        if (SkinManger.instance.GetActiveHat() == hat)
        {
            buttonText.text = PickedText;
        }
        else
        {
            isOpenedHatBought = (openedHatID == 0 || PlayerPrefs.GetInt("IsHat" + openedHatID + "Bought") == 1);
            buttonText.text = isOpenedHatBought || openedHat.price <= 0 ? PickText : BuyText;
        }

        if (isOpenedHatBought)
        {
            priceOfHatPanel.SetActive(false);
        }
        else
        {
            priceOfHat.text = hat.price + "c";
            priceOfHatPanel.SetActive(true);
        }
        nameOfHat.text = hat.HatName;

        if (hat.hatSprite != null)
        {
            hatImage.sprite = hat.hatSprite;
            hatImage.gameObject.SetActive(true);
        }
        else
        {
            hatImage.gameObject.SetActive(false);
        }
    }
      public void UpdateHatList(){
        foreach(Transform child in hatListParent){
            Destroy(child.gameObject);
        }
        bool isSomeHatOpened = false;
        for (int i = 0; i < allHats.hats.Count; i++)
        {
            if (!allHats.hats[i].isVisible && PlayerPrefs.GetInt("IsHat" + i + "Bought") != 1)
                continue;
            GameObject createdButton = Instantiate(hatButtonPrefab, hatListParent);
            ChooseHatButton button = createdButton.GetComponent<ChooseHatButton>();
            button.Set(this, allHats.hats[i]);
            if (!isSomeHatOpened)
            {
                OpenHat(allHats.hats[i]);
                isSomeHatOpened = true;
            }
        }
    }

    public void PickButton()
    {
        if (isOpenedHatBought || openedHat.price <= 0)
        {
            if (SkinManger.instance.GetActiveHat() == openedHat)
            {
                SoundManager.instance.PlaySound(cancelSound, 0, SoundType.System);
                return;
            }
            int id = allHats.hats.IndexOf(openedHat);
            SkinManger.instance.SetHat(id);
            OpenHat(openedHat);
        }
        else
        {
            if (Pet.Instance.Stats.Coins < openedHat.price)
            {
                SoundManager.instance.PlaySound(cancelSound, 0, SoundType.System);
                return;
            }
            Pet.Instance.Stats.Coins -= openedHat.price;
            PlayerPrefs.SetInt("Coins", Pet.Instance.Stats.Coins);
            PlayerPrefs.SetInt("IsHat" + openedHatID + "Bought", 1);
            OpenHat(openedHat);
            SoundManager.instance.PlaySound(buySound, 0, SoundType.System);
        }
    }
}
