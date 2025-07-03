using UnityEngine;

public class DynamicTargetTask : Task
{
    protected Transform target;
    protected float requiredDistance;
    protected bool breakAfter = false;

    public override void TaskStart()
    {
        //Debug.Log("Task: \"" + TaskName + "\" is started");
        Pet.Instance.Animator.SetBool("IsWalk", true);
    }

    public override void Actions()
    {
        Pet pet = Pet.Instance;
        Vector2 moveDir = ((Vector2)target.position - (Vector2)pet.transform.position).normalized;
        pet.transform.position += (Vector3)(moveDir * pet.MoveSpeed * pet.SpeedMultiplier) * Time.deltaTime;
    }
    public override void TaskInterrupt()
    {
        Pet.Instance.Animator.SetBool("IsWalk", false);
    }
    public override void TaskEnd()
    {
        //Debug.Log("Task: \"" + TaskName + "\" is completed");
        if (breakAfter) Pet.Instance.Break(Random.Range(1, 5));
        Pet.Instance.Animator.SetBool("IsWalk", false);
    }
    public override void TaskFail()
    {
        Pet.Instance.Animator.SetBool("IsWalk", false);
    }

    public override bool isDone()
    {
        Pet pet = Pet.Instance;
        float distance = Vector2.Distance(pet.transform.position, target.position);
        return distance <= requiredDistance ? true : false;
    }
    public override bool isFailed()
    {
        if (target == null || target.gameObject.activeSelf == false) return true;
        return false;
    }


    public DynamicTargetTask(string taskName, float timeLimit, Transform target, float requiredDistance, bool breakAfter = false) : base(taskName, timeLimit)
    {
        this.target = target;
        this.requiredDistance = requiredDistance;
        this.breakAfter = breakAfter;
    }
}


