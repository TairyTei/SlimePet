using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManger
{
    private static SaveManger instance;
    public static SaveManger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveManger();
            }
            return instance;
        }
    }
    private static bool isItirstLaunch = true;
    public static bool IsItFirstLaunch { get { return isItirstLaunch; } }
     public static void Save()
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
    public static void Load()
    {
        Pet pet = Pet.Instance;
        PetStats stats = pet.Stats;

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

        stats.Coins = PlayerPrefs.GetInt("Coins");
        stats.Satiety = PlayerPrefs.GetFloat("Satiety");
        stats.Cleanliness = PlayerPrefs.GetFloat("Cleanlines");
        stats.Love = PlayerPrefs.GetFloat("Love");
        stats.Boredom = PlayerPrefs.GetFloat("Boredom");
        stats.Cheerfulness = PlayerPrefs.GetFloat("Cheerfulness");
        stats.Happiness = PlayerPrefs.GetFloat("Happiness");
        stats.Expirience = PlayerPrefs.GetInt("Expirience");
        stats.StoriesQueue = PlayerPrefs.GetInt("StoriesInQueue");
        
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
    private static void LoadFurniture()
    {
        FurnitureBox furnitureBox = FurnitureBox.instance;
        List<RoomObject> roomObjects = furnitureBox.AllFurniture;

        foreach (RoomObject obj in roomObjects)
        {
            bool isInRoom = PlayerPrefs.GetInt("Is" + obj.nameOfObject + "InRoom") == 1;
            if (!isInRoom) continue;

            furnitureBox.AddFurnitureToRoom(obj);

            float xPos = PlayerPrefs.GetFloat("XPosOf" + obj.nameOfObject);
            float yPos = PlayerPrefs.GetFloat("YPosOf" + obj.nameOfObject);

            obj.transform.position = new Vector2(xPos, yPos);

            obj.gameObject.SetActive(true);
            obj.Put();
            
        }
    }
    private static void SaveFurniture()
    {
         
        FurnitureBox furnitureBox = FurnitureBox.instance;
        List<RoomObject> roomObjects = furnitureBox.AllFurniture;

        foreach (RoomObject obj in roomObjects)
        {
            bool isInRoom = furnitureBox.GetObjectsInRoom().ToList().Contains(obj);
            if (isInRoom)
            {
                PlayerPrefs.SetFloat("XPosOf" + obj.nameOfObject, obj.transform.position.x);
                PlayerPrefs.SetFloat("YPosOf" + obj.nameOfObject, obj.transform.position.y);
            }
        }
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