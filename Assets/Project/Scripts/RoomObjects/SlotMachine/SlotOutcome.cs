using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class SlotOutcome
{
    public Sprite[] Combination;
    public UnityEvent OnTrigger;
    public bool OrderSensitive = true;
}