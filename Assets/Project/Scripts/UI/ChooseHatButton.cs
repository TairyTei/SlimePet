using TMPro;
using UnityEngine;
public class ChooseHatButton : LocalizableMB
{
    public TextMeshProUGUI text;
    public GameObject giftIcon;
    private HatSelectionMenu hatSelectionMenu;
    private Hat hat;
    public void Set(HatSelectionMenu hatSelectionMenu, Hat hat)
    {
        this.hatSelectionMenu = hatSelectionMenu;
        this.hat = hat;
        text.text = hat.HatName;
        giftIcon.SetActive(hat.isGift);
    }
    public void Press()
    {
        hatSelectionMenu.OpenHat(hat);
    }
    public override void OnLanguageChanged()
    {
        if(hat != null)
            text.text = hat.HatName;
    }
}