using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingPool : RoomObject, IObjectFunction
{
    public override void Put()
    {

    }
    public override void PutAway()
    {

    }
    public void RMB_Click()
    {
        if (Pet.Instance.TaskCount >= 3
        || Pet.Instance.TaskCount > 0 && Pet.Instance.GetActiveTask().TaskName == ObjectTask().TaskName) return;

        Task swim = new SwimTask( -1, false);
        Pet.Instance.AddTask(swim, 0);    
    }
    void OnDestroy()
    {
        CursorSprite.Instance.RemoveSprite(InteractCursorSprite);
    }
}
