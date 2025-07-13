using UnityEngine;

public class Coin : RoomObject, IObjectFunction
{
    public int count = 1;
    public AudioClip coinSound;
    public override bool IsFunctional { get { return true; } }
    public override void Put()
    {
        base.Put();
    }
    public void RMB_Click()
    {
        Pet.Instance.Stats.Coins = Pet.Instance.Stats.Coins + count;
        SoundManager.instance.PlaySound(coinSound, 0.2f, SoundType.Main);
        DestroyRoomObject(this);
    }

}