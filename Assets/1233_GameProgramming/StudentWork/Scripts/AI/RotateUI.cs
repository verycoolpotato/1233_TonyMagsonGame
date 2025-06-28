using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUI : MonoBehaviour
{
   
    void Update()
    {
       transform.LookAt(PlayerLocatorSingleton.Instance.transform.position);
    }
}
