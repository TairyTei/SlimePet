using System.Collections;
using UnityEngine;

public class VacuumCleaner : RoomObject
{
    public float speed = 4;
    public float checkDelay = 5f;
    public AudioClip suckSound;
    private Transform target;
    void Update()
    {
        if (target != null)
        {
            Clean();
        }
    }

    public override void Put()
    {
        StartCoroutine(RoomCheck());
    }
    public override void PutAway()
    {
        target = null;
        StopCoroutine("RoomCheck");
    }

    IEnumerator RoomCheck()
    {        
        while (true)
        {
            //Debug.Log("StartFind");
            yield return new WaitForSeconds(checkDelay);
            if (target != null || !gameObject.activeSelf) continue;

            RoomObject[] poops = FurnitureBox.instance.GetObjectsWithTag("Poop");
            //Debug.Log(poops.Length);
            if (poops.Length > 0)
            {
                int rand = Random.Range(0, poops.Length);
                target = poops[rand].transform;
            }
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
        }
    }
}