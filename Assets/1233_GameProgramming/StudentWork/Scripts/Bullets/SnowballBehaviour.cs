using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SnowballBehaviour : BulletStat
{

    [SerializeField] Rigidbody Rb;
    [SerializeField] GameObject HitParticle;
    private void OnCollisionEnter(Collision collision)
    {
        GameObject Clone = Instantiate(HitParticle, transform.position, Quaternion.identity);
        Destroy(Clone, 1);
        Destroy(gameObject);
    }
}
