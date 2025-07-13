using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem poopSmell;
    public ParticleSystem PoopSmell => poopSmell;
    [SerializeField] private ParticleSystem sleep;
    public ParticleSystem Sleep => sleep;
    [SerializeField] private ParticleSystem love;
    public ParticleSystem Love => love;



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
    public void PlayParticlesFor(ParticleSystem particleSystem, float time)
    {
        StartCoroutine(IEPlayParticlesFor(particleSystem, time));
    }
    IEnumerator IEPlayParticlesFor(ParticleSystem particleSystem, float time)
    {
        particleSystem.Play();
        yield return new WaitForSeconds(time);
        particleSystem.Stop();
    }
    void Awake()
    {
        instance = this;
    }
}
