using UnityEngine;

public class AnimationTriggerTask : Task
{
    string triggerName;
    float length;
    private float start;
    public AnimationTriggerTask(string taskName, float timeLimit, string triggerName, float length = 4) : base(taskName, timeLimit)
    {
        this.triggerName = triggerName;
        this.length = length;
        start = Time.time;
    }
    public override void TaskStart()
    {
        Pet.Instance.Animator.SetTrigger(triggerName);
    }
    public override void Actions()
    {

    }
    public override void TaskInterrupt()
    {

    }
    public override void TaskEnd()
    {

    }
    public override void TaskFail()
    {

    }
    public override bool isDone()
    {
        if (Time.time > start + length)
            return true;
        else
            return false;
    }
    public override bool isFailed()
    {
        return false;
    }
}