using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PetSkin", menuName = "My/PetSkins/PetAllSkins", order = 0)]
public class AllPetSkins : ScriptableObject {
    public List<PetSkin> skins = new List<PetSkin>();
}