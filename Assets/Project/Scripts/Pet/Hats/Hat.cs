using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hat", menuName = "My/Hats/Hat")]
public class Hat : LocalizableSO
{
    public string hatID;
    public string HatName { get; private set; }
    public string namePath;
    //public GameObject hatPrefab;
    public Sprite hatSprite;
    public int price;
    public bool isVisible = true;
    public bool isGift = false;
    public List<MonologueRule> monologues = new List<MonologueRule>();
    public override void OnLanguageChanged()
    {
        HatName = ExternalTextLoader.LoadText(namePath);
    }
}