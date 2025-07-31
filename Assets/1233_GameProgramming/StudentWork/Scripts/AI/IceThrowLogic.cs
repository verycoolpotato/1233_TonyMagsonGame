using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceThrowLogic : MonoBehaviour
{
    [SerializeField] private GameObject IceChunkObj;

    public void ThrowIce()
    {
       Vector3 throwFromPosition = transform.position + new Vector3(0,2,0);

       GameObject projectile = Instantiate(IceChunkObj, throwFromPosition, Quaternion.identity);

       projectile.GetComponent<Rigidbody>().AddForce(transform.rotation * Vector3.forward * 25,ForceMode.Impulse);

        Destroy(projectile,4);
      
    }
}
