using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KnockbackManager : MonoBehaviour
{
    [SerializeField] public Rigidbody rb;

    //Distance the character flies when hit
    [SerializeField] public float knockbackPercentage = 0f;

    public void damaged(Collision collision, float knockback)
    {
      
        knockbackPercentage += knockback;


        Vector3 knockbackDirection = -collision.GetContact(0).normal.normalized;
        float knockbackForce = 1 + knockbackPercentage;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

        

        Destroy(collision.gameObject);
    }
}
