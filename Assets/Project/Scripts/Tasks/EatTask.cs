using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatTask : DynamicTargetTask
{
    private FoodObject food;
    public override void TaskEnd()
    {
        base.TaskEnd();
        Pet pet = Pet.Instance;
        PetStats stats = pet.Stats;

        stats.Satiety = stats.Satiety + food.satiety;
        stats.Love = stats.Love + food.love;
        stats.Happiness = stats.Happiness + 1f;
        stats.PoopBar += food.poopiness;
        if (stats.PoopBar >= 3)
        {
            pet.Poo();
        }
        FurnitureBox.instance.RemoveFurnitureFromRoom(food);
        MonoBehaviour.Destroy(food.gameObject);
        SoundManager.instance.PlaySound(pet.EatSound, 0.3f, SoundType.Main);
    }
     public EatTask(string taskName, float timeLimit, FoodObject food, float requiredDistance)
    : base(taskName, timeLimit, food.transform, requiredDistance)
    {
        this.food = food;
        this.target = food.transform;
        this.requiredDistance = requiredDistance;
    }
}
   
