using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class KnockbackManager : MonoBehaviour
{
    //This rigidbody
    [SerializeField] public Rigidbody Rb;

    //Distance the character flies when hit
    [SerializeField] public float KnockbackPercentage = 0f;

    //knocks character back based on given strength
    public void Damaged(Transform attacker, float knockback)
    {
        KnockbackPercentage += knockback;

        Vector3 knockbackDirection = (Rb.position - attacker.position).normalized;
        float knockbackForce = 1 + KnockbackPercentage;
        Rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }

}
