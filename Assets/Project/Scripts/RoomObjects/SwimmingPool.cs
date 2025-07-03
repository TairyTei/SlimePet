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
        Task swim = new SwimTask("Swim", -1, false);
        Pet.Instance.AddTask(swim, 0);    
    }
    void OnDestroy()
    {
        CursorSprite.Instance.RemoveSprite(InteractCursorSprite);
    }
}
