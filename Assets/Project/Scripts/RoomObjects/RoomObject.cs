using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Sprite interactCursorSprite;
    [SerializeField] private MonologueRule[] overlookMonologues;
    public Sprite InteractCursorSprite { get { return interactCursorSprite; } }
    public virtual bool IsFunctional { get { return false; } }
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
    void OnDestroy()
    {
        CursorSprite.Instance.RemoveSprite(InteractCursorSprite);
    }
    public override void OnLanguageChanged()
    {
        NameOfObject = ExternalTextLoader.LoadText(namePath);
    }
}
