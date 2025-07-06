using System.Collections;
using UnityEngine;

public class Clock : RoomObject, IObjectFunction
{
    [SerializeField] private float timeScale = 3;
    private Coroutine activeCoroutine;

     public void RMB_Click()
    {
        if(activeCoroutine != null)
            StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(SpeedUp(10));
    }
    private IEnumerator SpeedUp(int time)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSeconds(time);
        Time.timeScale = 1;
        Debug.Log("PENIS DICK COCK");
    }
}