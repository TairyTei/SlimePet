using Unity.Mathematics;
using UnityEngine;

public class FoodObject : RoomObject, IObjectFunction {

    public float satiety;
    public float love;
    public float poopiness = 0;
    public override bool IsFunctional {get{ return true; }}
    public override void Put()
    {
        base.Put();

        if (Pet.Instance.Stats.Satiety == 10) return;
        Task eatTask = new EatTask("Eat " + nameOfObject, 15, this, 0.8f);
        Pet.Instance.AddTask(eatTask, 0);
    }
    public void RMB_Click(){
        if(Pet.Instance.TaskCount >= 3) return;
        if(Pet.Instance.Stats.Satiety == 10) return;

        Task eatTask = new EatTask("Eat " + nameOfObject, 15, this, 0.8f);
        Pet.Instance.AddTask(eatTask, 0);
        Pet.Instance.CancelBreak();
    }
}