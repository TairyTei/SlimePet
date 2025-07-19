using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticFeeder : RoomObject
{
    public float maxPushForce;
    public float timeDelay = 5;
    public RoomObject [] foodArray;
    [SerializeField] private Animator animator;
    public override void Put()
    {
        base.Put();
        StartFeed(timeDelay);
    }
    public override void PutAway()
    {
        base.PutAway();
        StopFeed();
    }
    private IEnumerator FeedCycle(float timeDelay){
        while(true){
            if (gameObject.activeSelf && FurnitureBox.instance.GetObjectsWithTag("food").Length < 1)
            {
                int rand = Random.Range(0, foodArray.Length - 1);
                RoomObject createdFood = Instantiate(foodArray[rand], transform.position, Quaternion.identity);
                FurnitureBox.instance.AddFurnitureToRoom(createdFood);
                Vector2 force = new Vector2(Random.Range(-maxPushForce, maxPushForce), Random.Range(-maxPushForce, maxPushForce));
                createdFood.GetComponent<Rigidbody2D>().velocity = force;
                animator.SetTrigger("Throw");
            }
            yield return new WaitForSeconds(timeDelay);
        }
    }
    public void StartFeed(float timeDelay){
        StopCoroutine("FeedCycle");
        StartCoroutine(FeedCycle(timeDelay));
    }
    public void StopFeed(){
        StopCoroutine("FeedCycle");
    }
}
