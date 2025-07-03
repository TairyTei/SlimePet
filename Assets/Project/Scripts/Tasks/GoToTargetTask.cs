using UnityEngine;

public class GoToTargetTask : Task
{
    protected Vector2 targetPosition;
    private float requiredDistance;
    private bool breakAfter = false;

    public override void TaskStart()
    {
        Pet.Instance.Animator.SetBool("IsWalk", true);
        //Debug.Log("Task: \"" + TaskName + "\" is started");
    }

    public override void Actions()
    {
        Pet pet = Pet.Instance;
        Vector2 moveDir = (targetPosition - (Vector2)pet.transform.position).normalized;
        pet.transform.position += (Vector3)(moveDir * pet.MoveSpeed * pet.SpeedMultiplier) * Time.deltaTime;
    }
    public override void TaskInterrupt()
    {
        Pet.Instance.Animator.SetBool("IsWalk", false);
    }
    public override void TaskEnd()
    {
        Pet pet = Pet.Instance;
        if (breakAfter) pet.Break(Random.Range(1, 5));
        Pet.Instance.Animator.SetBool("IsWalk", false);
        //Debug.Log("Task: \"" + TaskName + "\" is completed");
    }
     public override void TaskFail()
    {
        Pet.Instance.Animator.SetBool("IsWalk", false);
    }
    public override bool isDone()
    {
        Pet pet = Pet.Instance;
        float distance = Vector2.Distance(pet.transform.position, targetPosition);
        return distance <= requiredDistance ? true : false;
    }
    public override bool isFailed()
    {
        return false;
    }
    public GoToTargetTask(string taskName, float timeLimit, Vector2 targetPosition, float requiredDistance, bool breakAfter = false) : base(taskName, timeLimit)
    {
        this.targetPosition = targetPosition;
        this.requiredDistance = requiredDistance;
        this.breakAfter = breakAfter;
    }
}
