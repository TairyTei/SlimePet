using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllBeds", menuName = "My/Beds/AllBeds", order = 0)]
public class AllBeds : ScriptableObject {
    public List<BedSkin> beds = new List<BedSkin>();
}