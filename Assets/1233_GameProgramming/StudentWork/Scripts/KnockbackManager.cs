using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KnockbackManager : MonoBehaviour
{
    //This rigidbody
    [SerializeField] public Rigidbody rb;

    //Distance the character flies when hit
    [SerializeField] public float knockbackPercentage = 0f;

    //knocks character back based on given strength
    public void Damaged(Transform attacker, float knockback)
    {
        knockbackPercentage += knockback;

        Vector3 knockbackDirection = (rb.position - attacker.position).normalized;
        float knockbackForce = 1 + knockbackPercentage;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }

}
