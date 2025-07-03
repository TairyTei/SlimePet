using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallPet : MonoBehaviour
{
    public Pet pet;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Task gototask = new GoToTargetTask("go to point ", 10,mousePos , 1);
            pet.CancelBreak();
            pet.AddTask(gototask, 0);
        }
    }
}
