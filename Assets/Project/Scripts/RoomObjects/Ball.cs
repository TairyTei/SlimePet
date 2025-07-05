using Unity.Mathematics;
using UnityEngine;

public class Ball : RoomObject, IObjectFunction {
    public AudioClip hit;
    public override bool IsFunctional {get{ return true; }}
    public override void Put()
    {
        base.Put();

        Pet.Instance.AddTask(ObjectTask());
    }
    public void RMB_Click(){
        if(Pet.Instance.TaskCount >= 3) return;

        Pet.Instance.AddTask(ObjectTask(), 0);
        Pet.Instance.CancelBreak();
    }
    public override Task ObjectTask()
    {
        if (UnityEngine.Random.Range(0, 10) == 0)
        {
            return base.ObjectTask();
        }
        Rigidbody2D ball = this.GetComponent<Rigidbody2D>();
        Task playWithBall = new PlayWithBallTask("Play with " + id, 15, ball);
        return playWithBall;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 v = GetComponent<Rigidbody2D>().velocity;
        float speed = math.sqrt(math.dot(v, v));
        if(speed > 1){
           PlayHitSound();
        }
    }
    public void PlayHitSound(){
         SoundManager.instance.PlaySound(hit, 0.5f);
    }
}