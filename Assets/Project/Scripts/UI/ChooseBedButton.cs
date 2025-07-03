using TMPro;
using UnityEngine;
public class ChooseBedButton : LocalizableMB
{
    public TextMeshProUGUI text;
    private BedSelectionMenu bedSelectionMenu;
    private BedSkin bed;
    public void Set(BedSelectionMenu bedSelectionMenu, BedSkin bed)
    {
        this.bedSelectionMenu = bedSelectionMenu;
        this.bed = bed;
        text.text = bed.BedName;
    }
    public void Press()
    {
        bedSelectionMenu.OpenBed(bed);
    }
       public override void OnLanguageChanged()
    {
        if(bed != null)
            text.text = bed.BedName;
    }
}