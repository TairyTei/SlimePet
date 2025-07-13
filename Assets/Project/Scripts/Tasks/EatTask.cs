using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatTask : DynamicTargetTask
{
    private RoomObject foodObj;
    public override void TaskEnd()
    {
        base.TaskEnd();
        Pet pet = Pet.Instance;
        PetStats stats = pet.Stats;

        if (foodObj is FoodObject)
        {
            FoodObject food = (FoodObject)foodObj;
            stats.Satiety = stats.Satiety + food.satiety;
            stats.Love = stats.Love + food.love;
            stats.Happiness = stats.Happiness + 1f;
            stats.PoopBar += food.poopiness;
        }

        if (stats.PoopBar >= 3)
            {
                pet.Poo();
            }
        
        FurnitureBox.instance.RemoveFurnitureFromRoom(foodObj);
        if (FurnitureBox.instance.AllFurniture.Contains(foodObj))
        {
            foodObj.PutAway();
            foodObj.gameObject.SetActive(false);
        }
        else
        {
            RoomObject.DestroyRoomObject(foodObj);
        }
        SoundManager.instance.PlaySound(pet.EatSound, 0.3f, SoundType.Main);
    }
     public EatTask(string taskName, float timeLimit, RoomObject food, float requiredDistance)
    : base(taskName, timeLimit, food.transform, requiredDistance)
    {
        this.foodObj = food;
        this.target = food.transform;
        this.requiredDistance = requiredDistance;
    }
}
   
