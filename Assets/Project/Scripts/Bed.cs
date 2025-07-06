using UnityEngine;

public class Bed : RoomObject, IObjectFunction
{

    [SerializeField] private GameObject storyMark;
    public static Bed instance { get; private set; }

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
    public void ActivateStoryMark(bool isActive)
    {
        storyMark.SetActive(isActive);
    }
}