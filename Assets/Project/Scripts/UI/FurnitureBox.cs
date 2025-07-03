using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureBox : MonoBehaviour
{
    public GameObject menu;
    public TextMeshProUGUI nameOfFurniture;
    public TextMeshProUGUI descriptionOfFurniture;
    public GameObject priceOfFurniturePanel;
    public TextMeshProUGUI priceOfFurniture;
    public TextMeshProUGUI buttonText;
    public Image furnitureImage;
    public Transform furnitureListParent;
    public GameObject furnitureButtonPrefab;

    [SerializeField] private SwimmingPool swimmingPool;
    public SwimmingPool SwimmingPool {get{ return swimmingPool; }}

    public AudioClip buySound;
    public AudioClip cancelSound;
    
    [SerializeField]
    private List<RoomObject> allFurniture = new List<RoomObject>();
    public List<RoomObject> AllFurniture {get{ return allFurniture; }}
    private List<RoomObject> furnitureInRoom = new List<RoomObject>();
    private RoomObject openedFurniture;
    private int openedFurnitureID;
    private bool isOpenedFurnitureBought;
    private bool isOpened = false;
    public static FurnitureBox instance;

    void Start()
    {
        instance = this;
    }

    void Update()
    {

    }
    public void Open(){
        if (isOpened)
        {
            Close();
            return;
        }
        UpdateFurnitureList();
        OpenFurniture(allFurniture[0]);
        BedMenu.instance.Close();
        menu.SetActive(true);
    }
    public void Close(){
        menu.SetActive(false);
    }
    private void UpdateFurnitureList(){
        foreach(Transform child in furnitureListParent){
            Destroy(child.gameObject);
        }
        for(int i = 0; i < allFurniture.Count; i++){
            GameObject createdButton = Instantiate(furnitureButtonPrefab, furnitureListParent);
            ChooseFunitureButton button = createdButton.GetComponent<ChooseFunitureButton>();
            button.Set(this, allFurniture[i]);
        }
    }
    public void OpenFurniture(RoomObject roomObject)
    {
        openedFurniture = roomObject;
        openedFurnitureID = allFurniture.IndexOf(roomObject);
        isOpenedFurnitureBought = PlayerPrefs.GetInt("IsFurniture" + openedFurnitureID + "Bought") == 1;

        nameOfFurniture.text = roomObject.nameOfObject;
        descriptionOfFurniture.text = roomObject.descriptionOfObject;
        furnitureImage.sprite = roomObject.icon;
        bool isThere = furnitureInRoom.Contains(roomObject);

        if (isOpenedFurnitureBought)
        {
            priceOfFurniturePanel.SetActive(false);
            buttonText.text = isThere ? "Put away" : "Put";
        }
        else
        {
            buttonText.text = "Buy";
            priceOfFurniture.text = openedFurniture.price + "c";
            priceOfFurniturePanel.SetActive(true);
        } 
    }
    public void Put(RoomObject roomObject)
    {
        Vector2 randomPos = WindowBorderCollider.instance.GetRandomPointInRoom();
        roomObject.transform.position = randomPos;
        roomObject.gameObject.SetActive(true);
        roomObject.Put();
        if (!furnitureInRoom.Contains(roomObject))
            AddFurnitureToRoom(roomObject);
    }
    public void FurnitureButton()
    {
        if (isOpenedFurnitureBought)
        {
            if (furnitureInRoom.Contains(openedFurniture))
            {
                openedFurniture.PutAway();
                openedFurniture.gameObject.SetActive(false);
                RemoveFurnitureFromRoom(openedFurniture);
            }
            else
            {
                Put(openedFurniture);
            }
        }
        else
        {
            PetStats stats = Pet.Instance.Stats;
            if (stats.Coins < openedFurniture.price)
            {
                SoundManager.instance.PlaySound(cancelSound, 0, SoundType.System);
                return;
            }
            stats.Coins -= openedFurniture.price;
            PlayerPrefs.SetInt("Coins", stats.Coins);
            PlayerPrefs.SetInt("IsFurniture" + openedFurnitureID + "Bought", 1);
            SoundManager.instance.PlaySound(buySound, 0, SoundType.System);
        }
        OpenFurniture(openedFurniture);
    }
    public void AddFurnitureToRoom(RoomObject furniture)
    {
        furnitureInRoom.Add(furniture);
        PlayerPrefs.SetInt("Is" + furniture.nameOfObject + "InRoom", 1);
    }
    public void RemoveFurnitureFromRoom(RoomObject furniture){
        if (furnitureInRoom.Contains(furniture))
        {
            furnitureInRoom.Remove(furniture);
            PlayerPrefs.SetInt("Is" + furniture.nameOfObject + "InRoom", 0);
        }
    }
    public RoomObject[] GetObjectsInRoom(){
        return furnitureInRoom.ToArray();
    }
    public RoomObject[] GetObjectsWithTag(string tag){
        List<RoomObject> objects = new List<RoomObject>();

        foreach (RoomObject obj in furnitureInRoom)
        {
            if(obj.tags.Contains(tag))
                objects.Add(obj);
        }
        return objects.ToArray();
    }
}
