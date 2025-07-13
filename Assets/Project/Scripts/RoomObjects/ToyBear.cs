using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBear : RoomObject
{
    [SerializeField] private AudioClip hugSound;
    public AudioClip HugSound => hugSound;
}
