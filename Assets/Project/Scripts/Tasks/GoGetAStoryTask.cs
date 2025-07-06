
using UnityEngine;

public class GoGetAStoryTask : Task
{
    private float startTime;
    private Vector2 targetPos;
    private bool isPetOnThePoint = false;
    private const float standardJourneyTime = 10;
    private float journeyTime;
    public GoGetAStoryTask(string taskName, float timeLimit) : base(taskName, timeLimit)
    {
        startTime = Time.time;
        journeyTime = standardJourneyTime * Pet.Instance.Stats.Level;
    }
    public override void TaskStart()
    {

        do
        {
            targetPos = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
        } while (WindowBorderCollider.instance.isInBounds(targetPos));
    }
    public override void Actions()
    {
        if (isPetOnThePoint) return;

        if (Vector2.Distance(Pet.Instance.transform.position, targetPos) > 0.5f
        && subtasks.Count == 0)
        {
            Task task = new GoToTargetTask("Go to point " + targetPos.ToString(), -1, targetPos, 0.4f, false);
            subtasks.Add(task);
        }
        else if (Vector2.Distance(Pet.Instance.transform.position, targetPos) <= 0.5f)
        {
            isPetOnThePoint = true;
        }

        SubtasksExecution();
    }
    public override void TaskInterrupt()
    {

    }
    public override void TaskEnd()
    {
        if (Pet.Instance.Stats.StoriesQueue > 0)
        {
            Pet.Instance.Stats.StoriesQueue--;
            PlayerPrefs.SetInt("StoriesInQueue", Pet.Instance.Stats.StoriesQueue);
            StoriesWindow.Instance.UnlockRandomStory();
        }
    }
    public override void TaskFail()
    {

    }
    public override bool isDone()
    {
        if (Time.time > startTime + journeyTime)
            return true;
        else
            return false;
    }
    public override bool isFailed()
    {
        Vector2 pos = Pet.Instance.transform.position;
        if (isPetOnThePoint && WindowBorderCollider.instance.isInBounds(pos))
            return true;
        else
            return false;
    }
}