using System.Collections;
using UnityEngine;

public class Clock : RoomObject, IObjectFunction
{
    [SerializeField] private float timeScale = 3;
    [SerializeField] private float coolDownTime = 10f;
    [SerializeField] private AudioClip sound;
    private Coroutine activeCoroutine;

    private bool coolDown = false;

    public void RMB_Click()
    {
        if (coolDown) return;

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(SpeedUp(10));


    }
    public override void PutAway()
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        Time.timeScale = 1;
        coolDown = false;
    }

    private IEnumerator SpeedUp(int time)
    {
        coolDown = true;
        Time.timeScale = timeScale;
        SoundManager.instance.PlaySound(sound, 0);
        yield return new WaitForSeconds(time);
        Time.timeScale = 1;

        yield return new WaitForSeconds(coolDownTime);
        coolDown = false;
    }
}