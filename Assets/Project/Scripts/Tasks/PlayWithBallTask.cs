using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWithBallTask : Task
{
    Rigidbody2D ball;
    int numberOfKicks = 0;
    int requiredNumberOfKicks;
    float lastKick = 0;
    private const float requiredDistanceForKick = 0.8f;
    private const float kickCoolDown = 0.3f;

    public override void TaskStart()
    {

    }

    public override void Actions()
    {
        float distance = Vector2.Distance(ball.transform.position, Pet.Instance.transform.position);
        if (distance > requiredDistanceForKick)
        {
            Task goToBall = new DynamicTargetTask("Go to " + ball.name, 2, ball.transform, requiredDistanceForKick);
            Pet.Instance.AddTask(goToBall, 0);
        }
        else
        {
            KickTheBall();
        }
    }
    public override void TaskInterrupt() {}
    public override void TaskEnd()
    {
        Pet pet = Pet.Instance;
    }
     public override void TaskFail()
    {
    }
    private void KickTheBall()
    {
        float distance = Vector2.Distance(ball.transform.position, Pet.Instance.transform.position);
        if (distance > requiredDistanceForKick || Time.time < lastKick + kickCoolDown) return;

        Vector2 dir = (ball.transform.position - Pet.Instance.transform.position).normalized;
        float force = Random.Range(1, 10);
        ball.velocity += dir * force;
        if (ball.gameObject.GetComponent<Ball>())
            ball.gameObject.GetComponent<Ball>().PlayHitSound();

        Pet pet = Pet.Instance;
        pet.Stats.Cheerfulness = pet.Stats.Cheerfulness - 0.25f;
        pet.Stats.Boredom = pet.Stats.Boredom + 0.5f;

        lastKick = Time.time;
        numberOfKicks++;
    }
    public override bool isDone()
    {
        return numberOfKicks >= requiredNumberOfKicks ? true : false;
    }
    public override bool isFailed()
    {
        if(ball == null || ball.gameObject.activeSelf == false) return true;
        return false;
    }
    public PlayWithBallTask(string taskName, float timeLimit, Rigidbody2D ball)
    : base(taskName, timeLimit)
    {
        requiredNumberOfKicks = Random.Range(1, 5);
        SetTimeLimit(requiredNumberOfKicks * 3);
        this.ball = ball;
    }
}
   
