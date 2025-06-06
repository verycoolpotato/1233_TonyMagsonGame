using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SnowballBehaviour : BulletStat
{

    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject hitParticle;
    private void OnCollisionEnter(Collision collision)
    {
        GameObject Clone = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(Clone, 1);
        Destroy(gameObject);
    }
}
