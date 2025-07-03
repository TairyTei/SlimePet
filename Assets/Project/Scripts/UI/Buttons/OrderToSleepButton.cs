using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderToSleepButton : MonoBehaviour
{
    public Pet pet;
    public PetMenu petMenu;

    public void OrderToSleep()
    {
        pet.CancelBreak();
        pet.AddTask(new SleepTask("Sleep", -1, false), 0);
	    petMenu.Close();
    }
}
