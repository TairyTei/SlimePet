
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TaskGenerator{
    public static Task GetRandomTask(){
        
        if (Pet.Instance.Stats.Cheerfulness < 1)
        {
            Task goToSleep = new SleepTask("Sleep", -1, true);
            return goToSleep;
        }
        if (Pet.Instance.Stats.Satiety < 2f && Random.Range(0, 2) == 0)
        {
            Task eatTask = EatTask();
            if (eatTask != null)
            {
                return eatTask;
            }
            else
            {
                MonologueWindow.instance.RandomHungerMonologue();
            }
        }
        if (Pet.Instance.Stats.Cleanliness < 2f && Random.Range(0, 3) == 0)
        {
            if (FurnitureBox.instance.GetObjectsWithTag("SwimmingPool").Length > 0)
            {
                Task swim = new SwimTask( -1, true);
                return swim;
            }
        }
        if (Pet.Instance.Stats.Boredom < 2f && Random.Range(0, 4) == 0)
        {
            Task task = PlayTask();
            if (task != null)
                return task;
        }
        if (Pet.Instance.Stats.Love < 2f && Random.Range(0, 4) == 0)
        {
            RoomObject[] favoriteObjects = FurnitureBox.instance.GetObjectsWithTag("favorite");
            if (favoriteObjects.Length > 0)
            {
                int randomObject = Random.Range(0, favoriteObjects.Length);
                Task hugTheToy = new HugTheBearTask("HugTheBear", 10f, favoriteObjects[randomObject]);
                return hugTheToy;
            }
        }

        int random = Random.Range(0, 100);
        switch (random)
        {
            case int n when (n <= 80):
                return GoToRandomPointTask();
            case int n when (n > 80 && n <= 90):

                RoomObject[] objects = FurnitureBox.instance.GetObjectsInRoom();
                if (objects.Length == 0) return GetRandomTask();

                int randObj = Random.Range(0, objects.Length);
                if (objects[randObj].tags.Contains("scary")) return GetRandomTask();
                return objects[randObj].ObjectTask();

             case int n when (n > 90 && n <= 95):
                if(PlayerPrefs.GetInt("DoesPetCatchCursor") == 1)        
                    return CatchCursorTask();
                return GetRandomTask();
            case int n when (n > 95 && n <= 100):
                string triggerName = "IdleAnim" + Random.Range(1, 4);
                Task playAnimTask = new AnimationTriggerTask("Play animation", 10, triggerName, 4);
                return playAnimTask;
            default:
                return GoToRandomPointTask();
        }
    }

    public static Task GoToRandomPointTask(bool breakAfter = true){
        WindowBorderCollider wb = WindowBorderCollider.instance;
        Vector2 point;
        bool isThereAScaryObject;
        do
        {
            if (wb.isInBounds(Pet.Instance.transform.position))
            {
                Vector2 random = new Vector2(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 4.0f));
                point = (Vector2)Pet.Instance.transform.position + random;
            }
            else
            {
                point = wb.GetRandomPointInRoom();
            }

            isThereAScaryObject = false;
            Collider2D [] collidersAround = Physics2D.OverlapCircleAll(point, 4);
            foreach (Collider2D collider in collidersAround)
            {
                RoomObject roomObject = collider.GetComponent<RoomObject>();
                if (roomObject == null) continue;

                if (roomObject.tags.Contains("scary"))
                {
                    isThereAScaryObject = true;
                }
            }
        } while (!wb.isInBounds(point) || isThereAScaryObject);
        Task gototask = new GoToTargetTask("go to point " + Random.Range(0, 999), 15, point, 1, breakAfter);
        return gototask;
    }
    public static Task CatchCursorTask(){
        Task task = new DynamicTargetTask("Catch cursor", 12f, WorldCursor.instance.transform, 0.1f);
        return task;
    }
    public static Task PlayTask()
    {
        List<Task> availableTasks = new List<Task>();

        RoomObject[] balls = FurnitureBox.instance.GetObjectsWithTag("ball");
        for(int i = 0; i < balls.Length; i++)
        {
            if (!balls[i].GetComponent<Rigidbody2D>()) continue;

            Rigidbody2D ballRigidbody = balls[i].GetComponent<Rigidbody2D>();
            Task playWithBall = new PlayWithBallTask("Play with " + balls[i].id, 15, ballRigidbody);
            availableTasks.Add(playWithBall);
        }

        if (FurnitureBox.instance.GetObjectsWithTag("toyMouse").Length > 0)
        {
            RoomObject toyMouse = FurnitureBox.instance.GetObjectsWithTag("toyMouse")[0];
            availableTasks.Add(toyMouse.ObjectTask());
        }

        if (availableTasks.Count == 0)
            return null;

        int randomTask = Random.Range(0, availableTasks.Count);
        return availableTasks[randomTask]; 
    }
    public static Task EatTask(){
        RoomObject[] food = FurnitureBox.instance.GetObjectsWithTag("food");
        if(food.Length <= 0) return null;
        int randomFood = Random.Range(0, food.Length-1);
        Task eatTask = new EatTask("Eat " + food[randomFood].id, 15,food[randomFood], 0.7f);
        return eatTask;
    }
}