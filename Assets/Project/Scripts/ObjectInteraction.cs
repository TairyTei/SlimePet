using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject DraggedObject;
    private Vector2 draggedObjectOldPos = new Vector2(0, 0);
    private Vector2 mouseOffset;
    private Vector2 mousePos = new Vector2();
    Collider2D colliderUnderCursor;
    public Collider2D ColliderUnderCursor { get { return colliderUnderCursor; } }
    [SerializeField] private Sprite petHoldSprite;
    private bool onPet = false;
    public static ObjectInteraction instance;

    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (!Shower.Instance.IsOpened && !Pat.Instance.IsOpened)
        {
            Interaction();
        }
    
    }
    private void Interaction()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (DraggedObject)
        {
            Drag();
            return;
        }

        Collider2D newCollider = null;
        Collider2D[] results = Physics2D.OverlapCircleAll(mousePos, 0.2f);
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].gameObject != DraggedObject)
            {
                newCollider = results[i]; 
                break;
            }
        }
        if (newCollider == null && colliderUnderCursor != null
        || newCollider != null && colliderUnderCursor == null
        || newCollider != colliderUnderCursor)
        {
            ObjectUnderCursorChanged(colliderUnderCursor, newCollider);
            colliderUnderCursor = newCollider;
        }

        if (Input.GetMouseButtonDown(0) && colliderUnderCursor != null)
        {
            LMB_Click();
        }
        else if (Input.GetMouseButtonDown(1) && colliderUnderCursor != null)
        {
            RMB_Click();
        }
    }

    private void Drag()
    {
        draggedObjectOldPos = DraggedObject.transform.position;
        if (WindowBorderCollider.instance.isInBounds(mousePos))
            DraggedObject.transform.position = mousePos + mouseOffset;
        Vector2 deltaPos = (Vector2)DraggedObject.transform.position - draggedObjectOldPos;
        
        if (!onPet && CheckPlayer())
        {
            onPet = true;
            if (DraggedObject.GetComponent<FoodObject>() && Pet.Instance.Stats.Satiety < 10)
            {
                Pet.Instance.AddIdleRequest("Feed");
                Pet.Instance.Animator.SetBool("IsMouthOpened", true);
            }
        }
        if (onPet && !CheckPlayer())
        {
            onPet = false;
            if (Pet.Instance.IsThereIdleRequest("Feed"))
            {
                Pet.Instance.RemoveIdleRequest("Feed");
                Pet.Instance.Animator.SetBool("IsMouthOpened", false);
            }
        }
        

        if (Input.GetMouseButtonUp(0))
        {

            RoomObject draggedRoomObject = null;
            if (DraggedObject.GetComponent<RoomObject>())
            {
                draggedRoomObject = DraggedObject.GetComponent<RoomObject>();
            }
            if (onPet && draggedRoomObject != null)
            {
                FoodObject food = (FoodObject)draggedRoomObject;
                food.Put();
                if (Pet.Instance.IsThereIdleRequest("Feed"))
                {
                    Pet.Instance.RemoveIdleRequest("Feed");
                    Pet.Instance.Animator.SetBool("IsMouthOpened", false);
                }
                onPet = false;
            }
            if (DraggedObject.GetComponent<Rigidbody2D>())
            {
                DraggedObject.GetComponent<Rigidbody2D>().velocity = deltaPos / Time.deltaTime;
            }
            if (DraggedObject.GetComponent<Pet>())
            {
                CursorSprite.Instance.RemoveSprite(petHoldSprite);
            }

            DraggedObject = null;
        }
    }
    private void LMB_Click()
    {
        if (colliderUnderCursor == null) return;

        if (colliderUnderCursor.tag == "DraggableObject")
        {
            mouseOffset = (Vector2)colliderUnderCursor.transform.position - mousePos;
            draggedObjectOldPos = colliderUnderCursor.transform.position;
            DraggedObject = colliderUnderCursor.gameObject;

            if (DraggedObject.GetComponent<Rigidbody2D>())
                DraggedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (DraggedObject.GetComponent<Pet>())
            {
                Pet pet = DraggedObject.GetComponent<Pet>();
                if (pet.TaskCount > 0 && !pet.GetActiveTask().IsDragAllowed)
                {
                    DraggedObject = null;
                    return;
                }
                CursorSprite.Instance.AddSprite(petHoldSprite);
                Task petTask = pet.GetActiveTask();
                if (petTask != null)
                {
                    if (petTask.TaskName.ToLower() == "sleep"
                    || petTask.TaskName.ToLower() == "swim")
                        pet.CancelActiveTask();
                }
            }
        }
    }
    private void RMB_Click()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapCircle(mousePos, 0.2f);

        if (collider == null) return;

        if (collider.GetComponent<IObjectFunction>() != null)
        {
            collider.GetComponent<IObjectFunction>().RMB_Click();
        }
    }
    private void ObjectUnderCursorChanged(Collider2D oldColl, Collider2D newColl)
    {
        RoomObject oldObj = null;
        RoomObject newObj = null;
        if (oldColl != null && oldColl.GetComponent<RoomObject>())
        {
            oldObj = oldColl.GetComponent<RoomObject>();
        }
        if (newColl != null && newColl.GetComponent<RoomObject>())
        {
            newObj = newColl.GetComponent<RoomObject>();
        }

        if (newObj != null && newObj.InteractCursorSprite != null)
        {
            CursorSprite.Instance.AddSprite(newObj.InteractCursorSprite);
        }
        if (oldObj != null && oldObj.InteractCursorSprite != null)
        {
            CursorSprite.Instance.RemoveSprite(oldObj.InteractCursorSprite);
        }
    }
    private bool CheckPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, 0.2f);
        if (colliders.Length == 0) return false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == Pet.Instance.gameObject) return true;
        }
        return false;
    }
}
