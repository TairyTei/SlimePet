using UnityEngine;

public class CatchToyMouseTask : DynamicTargetTask
{
    private readonly ToyMouse toyMouse;
    private readonly Pet pet;
    private float catchDistance = 0.4f;
    private float maxChaseTime = 8f;
    private float startTime;

    public CatchToyMouseTask(ToyMouse mouse) : base("CatchTheMouse",10f,mouse.transform, 0.5f, false)
    {
        this.toyMouse = mouse;
        this.pet = Pet.Instance;
    }

    public override void TaskStart()
    {
        base.TaskStart();
        toyMouse.StartRunning();
        startTime = Time.time;
    }

    public override void TaskEnd()
    {
        base.TaskEnd();
        toyMouse.StopRunning();
        pet.Stats.Cheerfulness -= 1f;
        pet.Stats.Boredom += 1;
    }

    public override void TaskFail()
    {
        base.TaskFail();
        toyMouse.StopRunning();
    }

    public override void TaskInterrupt() { TaskFail(); }

}