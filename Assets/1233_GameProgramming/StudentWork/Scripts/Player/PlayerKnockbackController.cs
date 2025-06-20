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
        var enemy = collision.gameObject.GetComponent<SnowmanMover>();
        if (enemy != null)
        {
            damaged(enemy.transform, enemy.knockback);
        }
    }
}

