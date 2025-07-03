using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem poopSmell;
    public ParticleSystem PoopSmell { get { return poopSmell; } set { poopSmell = value; } }
    [SerializeField] private ParticleSystem sleep;
    public ParticleSystem Sleep { get { return sleep; } set { sleep = value; } }


    private static ParticleManager instance;
    public static ParticleManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("ParticleManager instance is null");
                return null;
            }
            else
            {
                return instance;
            }
        }
        set
        {
            instance = value;
        }
    }
    void Awake()
    {
        instance = this;
    }
}
