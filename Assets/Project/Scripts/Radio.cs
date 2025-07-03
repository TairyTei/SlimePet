using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : RoomObject, IObjectFunction
{
    [SerializeField] private MusicPlayer musicPlayer;
    public override void Put()
    {

    }
    public override void PutAway()
    {

    }
    public void RMB_Click()
    {
        musicPlayer.Open();
    }
    void OnDestroy()
    {
        CursorSprite.Instance.RemoveSprite(InteractCursorSprite);
    }
}
