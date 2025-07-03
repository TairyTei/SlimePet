using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameAim : Task
{
    private int points = 0;
    private bool miss = false;
    private MinigameManager minigameManager;
    public override bool IsDragAllowed { get { return false; } }
    public MinigameAim(string taskName, float timeLimit, MinigameManager minigameManager) : base(taskName, timeLimit)
    {
        this.minigameManager = minigameManager;
    }
    public override void TaskStart()
    {
        CursorSprite.Instance.AddSprite(minigameManager.AimCursorSprite);

        minigameManager.SetActive_MinigameUI(true);
        int record = PlayerPrefs.GetInt("Aim_Record");
        minigameManager.SetValue_Record(record);
        minigameManager.SetValue_Score(0);
    }
    public override void Actions()
    {
        if (subtasks.Count == 0)
        {
            Task task = TaskGenerator.GoToRandomPointTask(false);
            subtasks.Add(task);
        }

        if (Input.GetMouseButtonDown(0))
        {
            ;
            Collider2D collider = ObjectInteraction.instance.ColliderUnderCursor;
            if (collider != null && collider.GetComponent<Pet>())
            {
                points++;
                minigameManager.SetValue_Score(points);
                Pet.Instance.SpeedMultiplier = 1.0f + points / 3.0f;
                SoundManager.instance.PlaySound(minigameManager.HitSound, 0.3f, SoundType.Main);
            }
            else
            {
                miss = true;
                SoundManager.instance.PlaySound(minigameManager.MissSound, 0.3f, SoundType.Main);
            }
        }


        SubtasksExecution();
    }
    public override void TaskInterrupt() {}
    public override void TaskEnd()
    {
        Pet.Instance.SpeedMultiplier = 1;
        Pet.Instance.Stats.Coins += points;
        Pet.Instance.Stats.Expirience += points;

        CursorSprite.Instance.RemoveSprite(minigameManager.AimCursorSprite);

        if (points > PlayerPrefs.GetInt("Aim_Record"))
        {
            PlayerPrefs.SetInt("Aim_Record", points);
            SoundManager.instance.PlaySound(minigameManager.RecordSound, 0, SoundType.System);
        }
        minigameManager.SetActive_MinigameUI(false);
    }
    public override void TaskFail()
    {

    }

    public override bool isDone()
    {
        return miss;
    }
    public override bool isFailed()
    {
        return false;
    }
}
