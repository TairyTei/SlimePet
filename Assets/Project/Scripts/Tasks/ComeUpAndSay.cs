using UnityEngine;

public class ComeUpAndSay : DynamicTargetTask
{
    private Monologue monologue;
    public ComeUpAndSay(string taskName, Monologue monologue, float timeLimit, Transform target, float requiredDistance, bool breakAfter = false)
    : base(taskName, timeLimit, target, requiredDistance, breakAfter)
    {
        this.monologue = monologue;
    }

    public override void TaskEnd()
    {
        MonologueWindow.instance.CreateMonologue(monologue);
        Pet.Instance.Break(monologue.time);
    }
   
}