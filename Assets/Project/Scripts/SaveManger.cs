using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManger : MonoBehaviour, IInitializable
{
    [SerializeField] private float autosaveRate;
    [SerializeField] private Bed bed;
    public static SaveManger Instance { get; private set; }
    private static bool isItirstLaunch = true;
    public static bool IsItFirstLaunch { get { return isItirstLaunch; } }

    public void Initialize()
    {
        Instance = this;
        Load();
        StartCoroutine(Autosave());
    }

    private IEnumerator Autosave()
    {
        while (true)
        {
            yield return new WaitForSeconds(autosaveRate);
            Save();
        }
    }

     public void Save()
    {
        Pet pet = Pet.Instance;
        PetStats stats = pet.Stats;

        PlayerPrefs.SetInt("LaunchedEarlier", 1);

        PlayerPrefs.SetFloat("PetPosX", pet.transform.position.x);
        PlayerPrefs.SetFloat("PetPosY", pet.transform.position.y);

        PlayerPrefs.SetInt("Coins", stats.Coins);
        PlayerPrefs.SetFloat("Satiety", stats.Satiety);
        PlayerPrefs.SetFloat("Cleanlines", stats.Cleanliness);
        PlayerPrefs.SetFloat("Love", stats.Love);
        PlayerPrefs.SetFloat("Boredom", stats.Boredom);
        PlayerPrefs.SetFloat("Cheerfulness", stats.Cheerfulness);
        PlayerPrefs.SetFloat("Happiness", stats.Happiness);
        PlayerPrefs.SetInt("Expirience", stats.Expirience);

        System.DateTime date = System.DateTime.Now;
        PlayerPrefs.SetInt("SaveYear", date.Year);
        PlayerPrefs.SetInt("SaveMonth", date.Month);
        PlayerPrefs.SetInt("SaveDay", date.Day);
        PlayerPrefs.SetInt("SaveHour", date.Hour);
        PlayerPrefs.SetInt("SaveMinute", date.Minute);

        SaveFurniture();
        Debug.Log("Game saved");
    }
    public void Load()
    {
        Pet pet = Pet.Instance;
        PetStats petStats = pet.Stats;

        isItirstLaunch = PlayerPrefs.GetInt("LaunchedEarlier") == 1 ? false : true;

        if (isItirstLaunch)
        {
            FirstLaunch();
            return;
        }

        float petPosX = PlayerPrefs.GetFloat("PetPosX");
        float petPosY = PlayerPrefs.GetFloat("PetPosY");
        pet.transform.position = new Vector2(petPosX, petPosY);

        pet.petName = PlayerPrefs.GetString("PetName");

        int createYear = PlayerPrefs.GetInt("CreateYear");
        int createMonth = PlayerPrefs.GetInt("CreateMonth");
        int createDay = PlayerPrefs.GetInt("CreateDay");
        pet.CreateDate = new DateTime(createYear, createMonth, createDay);

        petStats.Coins = PlayerPrefs.GetInt("Coins");
        petStats.Satiety = PlayerPrefs.GetFloat("Satiety");
        petStats.Cleanliness = PlayerPrefs.GetFloat("Cleanlines");
        petStats.Love = PlayerPrefs.GetFloat("Love");
        petStats.Boredom = PlayerPrefs.GetFloat("Boredom");
        petStats.Cheerfulness = PlayerPrefs.GetFloat("Cheerfulness");
        petStats.Happiness = PlayerPrefs.GetFloat("Happiness");
        petStats.Expirience = PlayerPrefs.GetInt("Expirience");
        petStats.Level = PetStats.CalculateLevelFromExp(petStats.Expirience);
        petStats.StoriesQueue = PlayerPrefs.GetInt("StoriesInQueue");
        
        System.DateTime now = System.DateTime.Now;

        int saveYear = PlayerPrefs.GetInt("SaveYear");
        int saveMonth = PlayerPrefs.GetInt("SaveMonth");
        int saveDay = PlayerPrefs.GetInt("SaveDay");
        int saveHour = PlayerPrefs.GetInt("SaveHour");
        int saveMinute = PlayerPrefs.GetInt("SaveMinute");
        System.DateTime saveDate = new DateTime(saveYear, saveMonth, saveDay, saveHour, saveMinute, 0);
        int difference = (int)(now - saveDate).TotalMinutes;

        LoadFurniture();

        Debug.Log("С последнего сохранения прошло " + difference + " минут");
    }
    private void LoadFurniture()
    {
        FurnitureBox furnitureBox = FurnitureBox.instance;
        List<RoomObject> roomObjects = furnitureBox.AllFurniture;

        foreach (RoomObject obj in roomObjects)
        {

            bool isInRoom = PlayerPrefs.GetInt("Is" + obj.id + "InRoom") == 1;

            Debug.Log(obj.id);
            Debug.Log(isInRoom);
            Debug.Log(PlayerPrefs.GetInt("Is" + obj.id + "InRoom"));
            if (!isInRoom) continue;

            furnitureBox.AddFurnitureToRoom(obj);

            float xPos = PlayerPrefs.GetFloat("XPosOf" + obj.id);
            float yPos = PlayerPrefs.GetFloat("YPosOf" + obj.id);

            obj.transform.position = new Vector2(xPos, yPos);

            obj.gameObject.SetActive(true);
            obj.Put();

        }
        Vector2 bedPosition = new Vector2(PlayerPrefs.GetFloat("XPosOfBed"), PlayerPrefs.GetFloat("YPosOfBed"));
        bed.transform.position = bedPosition;
    }
    private void SaveFurniture()
    {
        FurnitureBox furnitureBox = FurnitureBox.instance;
        List<RoomObject> roomObjects = furnitureBox.AllFurniture;

        foreach (RoomObject obj in roomObjects)
        {
            bool isInRoom = furnitureBox.GetObjectsInRoom().ToList().Contains(obj);
            if (isInRoom)
            {
                PlayerPrefs.SetFloat("XPosOf" + obj.id, obj.transform.position.x);
                PlayerPrefs.SetFloat("YPosOf" + obj.id, obj.transform.position.y);
            }
        }
        PlayerPrefs.SetFloat("XPosOfBed", bed.transform.position.x);
        PlayerPrefs.SetFloat("YPosOfBed", bed.transform.position.y);
    }
    
    private static void FirstLaunch()
    {
        Pet pet = Pet.Instance;
        pet.CreateDate = System.DateTime.Now;
        //createDate = new System.DateTime(2007, 12, 5);
        PlayerPrefs.SetInt("CreateYear", pet.CreateDate.Year);
        PlayerPrefs.SetInt("CreateMonth", pet.CreateDate.Month);
        PlayerPrefs.SetInt("CreateDay", pet.CreateDate.Day);

        PlayerPrefs.SetInt("DoesPetCatchCursor", 1);
    }
}