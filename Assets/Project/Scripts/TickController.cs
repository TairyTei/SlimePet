using System;
using System.Collections;
using UnityEngine;


public class TickController : MonoBehaviour, IInitializable
{
    public event Action OnSmallTick;
    public event Action OnLargeTick;

    public const float middleTick = 15;
    public const float largeTick = 900;

    public static TickController Instance { get; private set; }
    public void Initialize()
    {
        Instance = this;
        StartCoroutine(SmallTick());
        StartCoroutine(LargeTick());
    }
    private IEnumerator SmallTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(middleTick / Time.timeScale);
            OnSmallTick.Invoke();
        }
    }
    private IEnumerator LargeTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(largeTick / Time.timeScale);
            OnLargeTick.Invoke();
        }
        
    }
}