using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyFoodButton : MonoBehaviour {
    [SerializeField] private FoodObject foodPrefab;

    private bool isItBought;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject lockPanel;
    [SerializeField] private TextMeshProUGUI priceText;


    public void UpdateButton()
    {
        icon.sprite = foodPrefab.icon;
        
        isItBought = PlayerPrefs.GetInt("IsFoodBought_" + foodPrefab.name) == 1 ? true : false;
        if (foodPrefab.price <= 0) isItBought = true;

        lockPanel.SetActive(!isItBought);
        if (!isItBought)
            priceText.text = foodPrefab.price + "c";
    }

    public void Press()
    {
        if (isItBought)
        {
            Spawn();
        }
        else
        {
            Buy();
        }
    }

    private void Buy()
    {
        if (Pet.Instance.Stats.Coins >= foodPrefab.price)
        {
            Pet.Instance.Stats.Coins -= foodPrefab.price;
            PlayerPrefs.SetInt("IsFoodBought_" + foodPrefab.name, 1);
        }
        UpdateButton();
    }
    private void Spawn()
    {
        RoomObject[] allFood = FurnitureBox.instance.GetObjectsWithTag("food");
        if (allFood.Length >= 3)
        {
            FurnitureBox.instance.RemoveFurnitureFromRoom(allFood[0]);
            Destroy(allFood[0].gameObject);
        }
        //Vector2 randPoint = WindowBorderCollider.instance.GetRandomPointInRoom();
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        FoodObject foodObject = Instantiate(foodPrefab, cursorPos, Quaternion.identity);
        FurnitureBox.instance.AddFurnitureToRoom(foodObject);

        ObjectInteraction.instance.DraggedObject = foodObject.gameObject;

        PetMenu.instance.Close();

        if (Random.Range(0, 10) <= 2)
        {
            MonologueWindow mw = MonologueWindow.instance;
            mw.RandomMonologueFromList(mw.FeedMonologues);
        }
    }
}