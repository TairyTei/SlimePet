using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> initializables = new List<MonoBehaviour>();
    void Awake()
    {
        LocalizationSystem.Initialize();
        foreach (IInitializable initializable in initializables)
        {
            initializable.Initialize();
        }
        
    }
}
