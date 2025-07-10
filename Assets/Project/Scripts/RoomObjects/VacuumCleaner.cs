using System.Collections;
using System.Linq;
using UnityEngine;

public class VacuumCleaner : RoomObject
{
    public float speed = 4;
    public float checkDelay = 5f;
    public AudioClip suckSound;
    private Transform target;
    [SerializeField] private RoomObject cleanerStation;
    void Update()
    {
        if (target != null)
        {
            Clean();
        }
        else
        {
            ReturnToStation();
        }
    }

    public override void Put()
    {
        StartCoroutine(IERoomCheck());
    }
    public override void PutAway()
    {
        target = null;
        StopCoroutine("RoomCheck");
    }

    IEnumerator IERoomCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkDelay);
            if (target != null || !gameObject.activeSelf) continue;
            RoomCheck();
        }
    }
    private void RoomCheck()
    {
        RoomObject[] poops = FurnitureBox.instance.GetObjectsWithTag("Poop");
        if (poops.Length > 0)
        {
            int rand = Random.Range(0, poops.Length);
            target = poops[rand].transform;
        }
    }
    private void Clean()
    {
        Vector2 dir = (target.position - transform.position).normalized;
        transform.position = (Vector2)transform.position + (dir * speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.5f)
        {
            FurnitureBox.instance.RemoveFurnitureFromRoom(target.gameObject.GetComponent<RoomObject>());
            Destroy(target.gameObject);
            target = null;
            SoundManager.instance.PlaySound(suckSound, 0.3f, SoundType.Main);
            RoomCheck();
        }
    }
    private void ReturnToStation()
{
    bool isThereAStation = FurnitureBox.instance.GetObjectsInRoom().Contains(cleanerStation);
    if (cleanerStation == null || !isThereAStation) return;

    float distance = Vector2.Distance(transform.position, cleanerStation.transform.position);
    if (distance > 0.05f)
    {
        Vector2 dir = (cleanerStation.transform.position - transform.position).normalized;
        transform.position = (Vector2)transform.position + (dir * speed * Time.deltaTime);
    }
    else
    {
        transform.position = cleanerStation.transform.position;
    }
}
}