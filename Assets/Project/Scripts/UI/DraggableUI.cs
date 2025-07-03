using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableUI : MonoBehaviour
{
    [SerializeField] private Transform main;
    private Vector2 offset;
    private bool isActive = false;
    void Update()
    {
        if (!isActive) return;

        Drag();
    }
    private void Drag()
    {
        main.position = (Vector2)Input.mousePosition + offset;
    }

    public void Down()
    {
        if (!isActive)
        {
            isActive = true;
            offset = main.position - Input.mousePosition;
        }
    }
    public void Up()
    {
        if (isActive)
        {
            isActive = false;
        }
    }
}
