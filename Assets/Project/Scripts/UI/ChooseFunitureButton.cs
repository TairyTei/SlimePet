using TMPro;
using UnityEngine;
public class ChooseFunitureButton : MonoBehaviour {
    public TextMeshProUGUI text;
    private FurnitureBox furnitureBox;
    private RoomObject roomObject;
    public void Set(FurnitureBox furnitureBox, RoomObject roomObject){
        this.furnitureBox = furnitureBox;
        this.roomObject = roomObject;
        text.text = roomObject.nameOfObject;
    }
    public void Press(){
        furnitureBox.OpenFurniture(roomObject);
    }
}