using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerKnockbackController : KnockbackManager
{
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerDamaged(collision);
        }

        if (collision.gameObject.CompareTag("OffMap"))
        {
            SceneManager.LoadScene("World");

        }
    }


    private void playerDamaged(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<SnowmanMover>();
        if (enemy != null)
        {
            damaged(enemy.transform, enemy.knockback);
            SendMessage("UpdateKnockbackNumber",knockbackPercentage);
        }
    }

    
}

