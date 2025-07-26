using System;
using System.Collections;
using UnityEngine;


public class TickController : MonoBehaviour, IInitializable
{
    public event Action OnSmallTick;
    public event Action OnLargeTick;

    public const float smallTick = 15;
    public const float largeTick = 600;

    public static TickController Instance { get; private set; }
    public void Initialize()
    {
        Instance = this;
        StartCoroutine(IESmallTick());
        StartCoroutine(IELargeTick());
    }
    private IEnumerator IESmallTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(smallTick / Time.timeScale);
            CallSmallTick();
        }
    }
    public void CallSmallTick()
    {
        OnSmallTick.Invoke();
    }
    private IEnumerator IELargeTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(largeTick / Time.timeScale);
            CallLargeTick();
        }

    }
    public void CallLargeTick()
    {
        OnLargeTick.Invoke();
    }

}