using UnityEngine;

public class PlaySlotMachineTask : DynamicTargetTask
{
    SlotMachine slotMachine;
    public PlaySlotMachineTask(string taskName, float timeLimit, SlotMachine slotMachine) : base(taskName, timeLimit, slotMachine.transform, 0.5f, true)
    {
        this.slotMachine = slotMachine;
    }
    public override void TaskStart()
    {
        base.TaskStart();
        Pet.Instance.SpeedMultiplier = 1.5f;
    }
    public override void TaskInterrupt()
    {
        base.TaskInterrupt();
        Pet.Instance.SpeedMultiplier = 1;
    }
    public override void TaskEnd()
    {
        base.TaskEnd();
        slotMachine.Spin();
        Pet.Instance.Stats.Boredom++;
        Pet.Instance.SpeedMultiplier = 1;
    }
    public override void TaskFail()
    {
        base.TaskFail();
        Pet.Instance.SpeedMultiplier = 1;
    }
}