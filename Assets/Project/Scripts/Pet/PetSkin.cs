using UnityEngine;

[CreateAssetMenu(fileName = "PetSkin", menuName = "My/PetSkins/PetSkin", order = 1)]
public class PetSkin : ScriptableObject
{
    public Sprite[] animationSprites;
    public MonologueRule[] monologues;
}