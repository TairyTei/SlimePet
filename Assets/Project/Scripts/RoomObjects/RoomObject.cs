using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomObject : LocalizableMB
{
    public Sprite icon;
    public string id;
    public string NameOfObject { get; private set; }
    [SerializeField] private string namePath;
    //public string descriptionOfObject;
    public int price;
    public List<string> tags = new List<string>();
    public List<string> temporaryTags = new List<string>();
    [SerializeField] private Sprite interactCursorSprite;
    [SerializeField] private MonologueRule[] overlookMonologues;
    [SerializeField] private MonologueRule[] spawnMonologues;
    public MonologueRule[] SpawnMonologues { get { return spawnMonologues; } }
    public Sprite InteractCursorSprite { get { return interactCursorSprite; } }
    public virtual bool IsFunctional { get { return false; } }
    public virtual bool RandomTags => true;
    public virtual bool CanBeRemoved => true;
    public virtual void Put()
    {

    }
    public virtual void PutAway()
    {

    }
    public virtual Task ObjectTask()
    {
        Task task;
        if (overlookMonologues.Length > 0)
        {
            Monologue monologue = overlookMonologues[Random.Range(0, overlookMonologues.Length)].monologue;
            task = new ComeUpAndSay("Go to " + id, monologue, 10f, transform, 1.5f, true);
        }
        else
        {
            task = new DynamicTargetTask("Go to " + id, 10f, transform, 1.5f, true);
        }
        return task;
    }
    public static void DestroyRoomObject(RoomObject roomObject)
    {
        if (FurnitureBox.instance.GetObjectsInRoom().Contains(roomObject))
        {
            FurnitureBox.instance.RemoveFurnitureFromRoom(roomObject);
        }
        CursorSprite.Instance.RemoveSprite(roomObject.InteractCursorSprite);
        Destroy(roomObject.gameObject);
    }
    public override void OnLanguageChanged()
    {
        NameOfObject = ExternalTextLoader.LoadText(namePath);
    }
}
