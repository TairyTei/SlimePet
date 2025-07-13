using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugTheBearTask : DynamicTargetTask
{
    private RoomObject toyObject;
    private const int loveCount = 1;
    public HugTheBearTask(string taskName, float timeLimit, RoomObject toy) : base(taskName, timeLimit, toy.transform, 0.5f, true)
    {
        toyObject = toy;
    }
    public override void TaskEnd()
    {
        base.TaskEnd();

        Pet.Instance.Stats.Love += loveCount;
        ParticleManager pm = ParticleManager.Instance;
        pm.PlayParticlesFor(pm.Love, 2f);
        if (toyObject is ToyBear)
        {
            ToyBear toyBear = (ToyBear)toyObject;
            SoundManager.instance.PlaySound(toyBear.HugSound, 0.2f, SoundType.Main);
        }
    }
}
