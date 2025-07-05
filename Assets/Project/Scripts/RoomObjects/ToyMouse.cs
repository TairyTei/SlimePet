using UnityEngine;

public class ToyMouse : RoomObject, IObjectFunction
{
    [SerializeField] private float runSpeed = 2.5f;
    [SerializeField] private float waitAtPointTime = 0.15f;
    [SerializeField] private float maxTargetDistance = 2f;
    [SerializeField] private float avoidPetRadius = 2.5f;
    [SerializeField] private Transform spriteTransform;
    private Animator _animator;
    private Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }
    private Vector2 targetPoint;
    private float waitTimer = 0f;
    private bool isRunningFromPet = false;

    private void Update()
    {
        if (!isRunningFromPet) return;

        if (Vector2.Distance(transform.position, targetPoint) < 0.06f)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                PickNewTarget();
                waitTimer = waitAtPointTime;
            }
            return;
        }

        Vector2 newPos = Vector2.MoveTowards(transform.position, targetPoint, runSpeed * Time.deltaTime);
        transform.position = newPos;
    }

    public void StartRunning()
    {
        isRunningFromPet = true;
        PickNewTarget();
        waitTimer = waitAtPointTime;
        animator.SetBool("IsRunning", true);
    }

    public void StopRunning()
    {
        isRunningFromPet = false;
        animator.SetBool("IsRunning", false);
    }

    private void PickNewTarget()
    {
        WindowBorderCollider wb = WindowBorderCollider.instance;
        Vector2 point;
        int attempts = 0;
        do
        {
            if (wb.isInBounds(Pet.Instance.transform.position))
            {
                Vector2 random = new Vector2(Random.Range(-maxTargetDistance, maxTargetDistance), Random.Range(-maxTargetDistance, maxTargetDistance));
                point = (Vector2)transform.position + random;
            }
            else
            {
                point = wb.GetRandomPointInRoom();
            }
            attempts++;
        } while (!wb.isInBounds(point)
        || Vector2.Distance(point, Pet.Instance.transform.position) < avoidPetRadius && attempts < 20);
        targetPoint = point;
        if (targetPoint.x > transform.position.x) spriteTransform.localScale = new Vector2(1, 1);
        else spriteTransform.localScale = new Vector2(-1, 1);
    }

    public void RMB_Click()
    {
        if (Pet.Instance.TaskCount >= 3) return;

        Pet.Instance.AddTask(ObjectTask(), 0);
        Pet.Instance.CancelBreak();
    }

    public override Task ObjectTask()
    {
        return new CatchToyMouseTask(this);
    }
    private void OnDrawGizmosSelected()
    {
         Gizmos.DrawWireSphere(targetPoint, 0.5f);
    }
}