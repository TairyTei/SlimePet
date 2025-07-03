using UnityEngine;

public class MinigameVictim : Task
{
    private float time = 0;
    MinigameManager minigameManager;
    public override bool IsDragAllowed { get { return false; } }
    public MinigameVictim(string taskName, float timeLimit, MinigameManager minigameManager) : base(taskName, timeLimit)
    {
        this.minigameManager = minigameManager;
    }

    public override void TaskStart()
    {
        Pet.Instance.gameObject.transform.position = new Vector2(0, 0);

        minigameManager.SetActive_MinigameUI(true);
        int record = PlayerPrefs.GetInt("Victim_Record");
        minigameManager.SetValue_Record(record);
        minigameManager.SetValue_Score(0);
    }
    public override void Actions()
    {
        time = Time.time - createTime;

        if (time < 3) return;

        float distance = Vector2.Distance(WorldCursor.instance.transform.position, Pet.Instance.transform.position);
        if (distance >= 0.1f && subtasks.Count == 0)
        {
            Task task = new DynamicTargetTask("Catch the cursor", 10f, WorldCursor.instance.transform, 0.1f);
            subtasks.Insert(0, task);
        }

        Pet.Instance.SpeedMultiplier = 1 + Mathf.Pow(time / 25, 2f);
        minigameManager.SetValue_Score( Mathf.FloorToInt(time) );
        SubtasksExecution();
    }
    public override void TaskInterrupt() {}
    public override void TaskEnd()
    {
        Pet.Instance.SpeedMultiplier = 1;
        Pet.Instance.Stats.Coins += Mathf.FloorToInt(time / 10);
        Pet.Instance.Stats.Expirience += (int)(time / 10);

        if (Mathf.FloorToInt(time) > PlayerPrefs.GetInt("Victim_Record"))
        {
            PlayerPrefs.SetInt("Victim_Record", Mathf.FloorToInt(time));
            SoundManager.instance.PlaySound(minigameManager.RecordSound, 0, SoundType.System);
        }
        minigameManager.SetActive_MinigameUI(false);
    }
    public override void TaskFail()
    {

    }

    public override bool isDone()
    {
        float distance = Vector2.Distance(WorldCursor.instance.transform.position, Pet.Instance.transform.position);
        return distance < 0.1f;
    }
    public override bool isFailed()
    {
        return false;
    }
}
