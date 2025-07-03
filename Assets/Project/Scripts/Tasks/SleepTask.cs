using UnityEngine;

public class SleepTask : Task
{
    private bool willPetWakeUp = false;
    public override void TaskStart()
    {
    }

    public override void Actions()
    {
        Pet pet = Pet.Instance;
        if (Vector2.Distance(pet.transform.position, pet.bed.position) > 0.3f)
        {
            Task GoToBed = new DynamicTargetTask("Go to bed", -1, pet.bed, 0.2f);
            pet.AddTask(GoToBed, 0);

            Animator animator = Pet.Instance.Animator;
            animator.SetBool("IsSleep", false);
        }
        else
        {
            if (pet.transform.position != pet.bed.position)
            {
                pet.transform.position = pet.bed.position;
            }
            pet.Stats.Cheerfulness = pet.Stats.Cheerfulness + 0.2f * Time.deltaTime;

            Animator animator = Pet.Instance.Animator;
            if (animator.GetBool("IsSleep") != true)
                animator.SetBool("IsSleep", true);
            if (!ParticleManager.Instance.Sleep.isPlaying) {
                ParticleManager.Instance.Sleep.Play();
            }
        }
    }
    public override void TaskInterrupt()
    {
        Pet.Instance.Animator.SetBool("IsSleep", false);
        ParticleManager.Instance.Sleep.Stop();
    }
    public override void TaskEnd()
    {
        Debug.Log("Task: \"" + TaskName + "\" is completed");
        Pet.Instance.Animator.SetBool("IsSleep", false);
        ParticleManager.Instance.Sleep.Stop();
    }
    public override bool isDone()
    {
        if(willPetWakeUp){
            return Pet.Instance.Stats.Cheerfulness >= 10 ? true : false;
        }
        return false;
    }
    public override void TaskFail()
    {
        Pet.Instance.Animator.SetBool("IsSleep", false);
        ParticleManager.Instance.Sleep.Stop();
    }
    public override bool isFailed()
    {
        return false;
    }

    public SleepTask(string taskName, float timeLimit, bool willPetWakeUp) : base(taskName, timeLimit)
    {
        this.willPetWakeUp = willPetWakeUp;
    }
}
