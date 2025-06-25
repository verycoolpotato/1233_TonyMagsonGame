using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceThrowLogic : MonoBehaviour
{
    [SerializeField] private GameObject iceChunkObj;

    public void ThrowIce()
    {
        Vector3 throwFromPosition = transform.position + new Vector3(0,2,0);

       GameObject projectile = Instantiate(iceChunkObj, throwFromPosition, Quaternion.identity);

       
      
    }
}
