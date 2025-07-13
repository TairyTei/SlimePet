using System.Collections.Generic;
using UnityEngine;

public class Bin : RoomObject, IObjectFunction
{
    [SerializeField] private Sprite aimCursorSprite;
    [SerializeField] private float flySpeed = 5f;
    [SerializeField] private float accelerationFactor = 1.5f;
    [SerializeField] private float removalDistance = 0.1f;
    [SerializeField] private AudioClip removeSound;
    
    private bool isRemoveModeEnabled;
    private List<RemovingObject> removingObjects = new List<RemovingObject>();

    public override bool CanBeRemoved => false;

    void Update()
    {
        if (!isRemoveModeEnabled) return;

        ProcessObjectsMovement();
        HandleMouseInput();
    }

    private void ProcessObjectsMovement()
    {
        for (int i = removingObjects.Count - 1; i >= 0; i--)
        {
            RemovingObject data = removingObjects[i];
            if (data.roomObject == null)
            {
                removingObjects.RemoveAt(i);
                continue;
            }

            if (!data.isInitialized)
            {
                data.Initialize(transform.position);
                removingObjects[i] = data;
            }

            data.UpdatePosition(Time.deltaTime, flySpeed, accelerationFactor);
            removingObjects[i] = data;

            if (data.IsReached(removalDistance))
            {
                RemoveObject(data.roomObject);
                data.Cleanup();
                removingObjects.RemoveAt(i);
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) TryAddObjectToRemove();
        if (Input.GetMouseButtonDown(1)) TryDisableRemoveMode();
    }

    private void TryAddObjectToRemove()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapCircle(mousePos, 0.1f);
        if (collider == null) return;

        RoomObject roomObject = collider.GetComponent<RoomObject>();
        if (roomObject == null || !roomObject.CanBeRemoved) return;

        foreach (RemovingObject obj in removingObjects)
        {
            if (obj.roomObject == roomObject) return;
        }

        removingObjects.Add(new RemovingObject(roomObject));
    }

    private void TryDisableRemoveMode()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapCircle(mousePos, 0.3f);
        if (collider == null || collider.gameObject != gameObject) DisableRemoveMode();
    }

    private void RemoveObject(RoomObject obj)
    {
        FurnitureBox.instance.RemoveFurnitureFromRoom(obj);
        if (FurnitureBox.instance.AllFurniture.Contains(obj))
        {
            obj.gameObject.SetActive(false);
        }
        else
        {
            Destroy(obj.gameObject);
        }
        SoundManager.instance.PlaySound(removeSound, 0.2f);
    }

    public void RMB_Click()
    {
        if (!isRemoveModeEnabled) EnableRemoveMode();
    }

    private void EnableRemoveMode()
    {
        isRemoveModeEnabled = true;
        CursorSprite.Instance.AddSprite(aimCursorSprite, 0);
    }

    private void DisableRemoveMode()
    {
        foreach (RemovingObject data in removingObjects)
        {
            if (data.roomObject != null) data.Cleanup();
        }
        removingObjects.Clear();
        isRemoveModeEnabled = false;
        CursorSprite.Instance.RemoveSprite(aimCursorSprite);
    }

    private class RemovingObject
    {
        public RoomObject roomObject;
        public bool isInitialized;
        private Vector2 startPosition;
        private Vector2 targetPosition;
        private float startTime;
        private bool originalKinematic;
        private Dictionary<Collider2D, bool> originalTriggers = new Dictionary<Collider2D, bool>();

        public RemovingObject(RoomObject roomObject)
        {
            this.roomObject = roomObject;
            Rigidbody2D rb = roomObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                originalKinematic = rb.isKinematic;
                rb.isKinematic = true;
            }

            Collider2D[] colliders = roomObject.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D col in colliders)
            {
                originalTriggers[col] = col.isTrigger;
                col.isTrigger = true;
            }
        }

        public void Initialize(Vector2 target)
        {
            startPosition = roomObject.transform.position;
            targetPosition = target;
            startTime = Time.time;
            isInitialized = true;
        }

        public void UpdatePosition(float deltaTime, float speed, float acceleration)
        {
            if (!isInitialized || roomObject == null) return;

            float elapsed = Time.time - startTime;
            float progress = Mathf.Clamp01(elapsed * speed);
            float curvedProgress = Mathf.Pow(progress, acceleration);

            roomObject.transform.position = Vector2.Lerp(
                startPosition, 
                targetPosition, 
                curvedProgress
            );
        }

        public bool IsReached(float threshold)
        {
            if (roomObject == null) return true;
            return Vector2.Distance(roomObject.transform.position, targetPosition) <= threshold;
        }

        public void Cleanup()
        {
            if (roomObject == null) return;

            Rigidbody2D rb = roomObject.GetComponent<Rigidbody2D>();
            if (rb != null) rb.isKinematic = originalKinematic;

            foreach (var pair in originalTriggers)
            {
                if (pair.Key != null) pair.Key.isTrigger = pair.Value;
            }
        }
    }
}