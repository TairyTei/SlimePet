using UnityEngine;

[CreateAssetMenu(fileName = "Bed", menuName = "My/Beds/Bed")]
public class BedSkin : LocalizableSO
{
    public string bedID;
    public string BedName { get; private set; }
    public string namePath;
    public int price;
    public Sprite bedSprite;
    public override void OnLanguageChanged()
    {
        BedName = ExternalTextLoader.LoadText(namePath);
    }
}