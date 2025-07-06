using UnityEngine;

public class Bed : RoomObject, IObjectFunction {

    public static Bed instance;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        FurnitureBox.instance.AddFurnitureToRoom(this);
    }
    public void RMB_Click()
    {
        if (!BedMenu.instance.isOpened)
        {
            BedMenu.instance.Open();
        }
        else
        {
            BedMenu.instance.Close();
        }
    }
}