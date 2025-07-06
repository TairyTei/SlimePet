using System;
using UnityEngine;

public class SkinManger : MonoBehaviour
{
    public AllHats allHats;
    public AllBeds allBeds;
    public AllPetSkins allPetSkins;
    int activeHatID;
    public int ActiveHatID { get { return activeHatID; } }
    int activeBedID;
    private int activePetSkinID;
    public int ActivePetSkinID { get { return activePetSkinID; } }
    public SpriteRenderer hatSprite;
    public SpriteRenderer bedSprite;
    public static SkinManger instance;
    public event Action OnSkinChanged;
    void Start()
    {
        instance = this;
        LoadSkins();
    }
    public void LoadSkins()
    {
        int hatId = PlayerPrefs.GetInt("PickedHatID");
        SetHat(hatId);

        int bedSkinID = PlayerPrefs.GetInt("PickedBedID");
        SetBed(bedSkinID);

        int petSkinID = PlayerPrefs.GetInt("PickedPetSkinID");
        SetPetSkin(petSkinID);
    }
    public void SetHat(int id)
    {
        PlayerPrefs.SetInt("PickedHatID", id);
        activeHatID = id;
        Hat hat = allHats.hats[id];
        if (hat.hatSprite != null)
        {
            hatSprite.sprite = hat.hatSprite;
            hatSprite.gameObject.SetActive(true);
        }
        else
        {
            hatSprite.gameObject.SetActive(false);
        }
        if (OnSkinChanged != null)
            OnSkinChanged.Invoke();
    }
    public void SetBed(int id)
    {
        PlayerPrefs.SetInt("PickedBedID", id);
        activeBedID = id;
        BedSkin bed = allBeds.beds[id];
        bedSprite.sprite = bed.bedSprite;
    }
    public void SetPetSkin(int id)
    {
        PlayerPrefs.SetInt("PickedPetSkinID", id);
        activePetSkinID = id;
        PetSkin skin = allPetSkins.skins[id];

        //Pet.Instance.Animator.Rebind();
        Pet.Instance.Animator.Update(0f);
        if (OnSkinChanged != null)
            OnSkinChanged.Invoke();
    }
    public Sprite GetSprite(string spriteName)
    {
        foreach (var sprite in allPetSkins.skins[ActivePetSkinID].animationSprites)
        {
            if (sprite.name == spriteName)
                return sprite;
        }
        return null;
    }
    public Hat GetActiveHat()
    {
        if (activeHatID > allHats.hats.Count - 1)
        {
            SetHat(0);
        }
        return allHats.hats[activeHatID];
    }
    public BedSkin GetActiveBed()
    {
        if (activeBedID > allBeds.beds.Count - 1)
        {
            SetBed(0);
        }
        return allBeds.beds[activeBedID];
    }
    public PetSkin GetActivePetSkin(){
        if(activePetSkinID > allPetSkins.skins.Count - 1){
            SetPetSkin(0);
        }
        return allPetSkins.skins[activePetSkinID];
    }
}