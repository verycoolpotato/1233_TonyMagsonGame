using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocatorSingleton : MonoBehaviour
{
    public static PlayerLocatorSingleton Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is more than one PlayerLocatorSingleton");
            Destroy(Instance);
        }

        
    }
}
