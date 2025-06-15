using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackController : KnockbackManager
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerDamaged(collision);
        }
    }

    private void playerDamaged(Collision collision)
    {
        float knockback = collision.gameObject.GetComponent<SnowmanMover>().knockback;

        damaged(collision,knockback);


    }
}
