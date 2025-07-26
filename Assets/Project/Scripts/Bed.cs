using UnityEngine;

public class Bed : RoomObject, IObjectFunction, IInitializable
{

    [SerializeField] private GameObject storyMark;
    public static Bed instance { get; private set; }
    public override bool RandomTags => false;
    public override bool CanBeRemoved => false;


    public void Initialize()
    {
        instance = this;
        SettingsManager.instance.OnSettingsUpdated += OnSettingsUpdated;
    }
    private void OnSettingsUpdated()
    {
        Settings settings = SettingsManager.instance.Settings;
        Task activeTask = Pet.Instance.GetActiveTask();
        bool isInStory = activeTask != null && activeTask.TaskName == "Story";
        storyMark.SetActive(isInStory && settings.displayStoryMark);
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
    void OnDestroy()
    {
        SettingsManager.instance.OnSettingsUpdated -= OnSettingsUpdated;
    }
}