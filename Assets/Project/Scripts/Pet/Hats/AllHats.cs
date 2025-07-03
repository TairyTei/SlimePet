using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AllHats", menuName = "My/Hats/AllHats", order = 0)]
public class AllHats : ScriptableObject
{
    public List<Hat> hats = new List<Hat>();

#if UNITY_EDITOR
    [ContextMenu("Sort Hats by Price (Ascending)")]
    private void SortHatsByPriceAscending()
    {
        hats = hats.OrderBy(hat => hat.price).ToList();
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
    }
    #endif
}