using UnityEngine;

public class SwimTask : Task
{
    private bool willEnd = false;
    private const float cleanSpeed = 0.6f;
    RoomObject swimmingPool;
    public SwimTask(float timeLimit, bool willEnd) : base("swim", timeLimit)
    {
        this.willEnd = willEnd;
        swimmingPool = FurnitureBox.instance.SwimmingPool;
    }
    public override void TaskStart()
    {
        
    }

    public override void Actions()
    {
        Pet pet = Pet.Instance;
        if (Vector2.Distance(pet.transform.position, swimmingPool.transform.position) > 0.3f)
        {
            if (subtasks.Count == 0)
            {

                Task GoToSwimmingPool = new DynamicTargetTask("Go to swimming pool", -1, swimmingPool.transform, 0.3f);
                subtasks.Add(GoToSwimmingPool);

                Animator animator = Pet.Instance.Animator;
                animator.SetBool("Swim", false);
            }
        }
        else
        {
            if (pet.transform.position != swimmingPool.transform.position)
            {
                pet.transform.position = swimmingPool.transform.position;
            }
            pet.Stats.Cleanliness += cleanSpeed * Time.deltaTime;

            Animator animator = Pet.Instance.Animator;
            if (animator.GetBool("Swim") != true)
                animator.SetBool("Swim", true);

            /*
            if (!ParticleManager.Instance.Sleep.isPlaying)
            {
                ParticleManager.Instance.Sleep.Play();
            }
            */
        }
        SubtasksExecution();
    }
    public override void TaskInterrupt()
    {
        Pet.Instance.Animator.SetBool("Swim", false);
        subtasks.Clear();
        //ParticleManager.Instance.Sleep.Stop();
    }
    public override void TaskEnd()
    {
        Pet.Instance.Animator.SetBool("Swim", false);
        //ParticleManager.Instance.Sleep.Stop();
    }
    public override bool isDone()
    {
        if(willEnd){
            return Pet.Instance.Stats.Cleanliness >= 10;
        }
        return false;
    }
    public override void TaskFail()
    {
        Pet.Instance.Animator.SetBool("Swim", false);
        //ParticleManager.Instance.Sleep.Stop();
    }
    public override bool isFailed()
    {

        return !swimmingPool.gameObject.activeSelf;
    }
}
