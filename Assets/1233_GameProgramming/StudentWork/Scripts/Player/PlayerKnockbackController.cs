using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerKnockbackController : KnockbackManager
{
    [SerializeField] private GameObject enemy;
   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyKnockbackSource"))
        {
            playerDamaged(collision, collision.relativeVelocity.magnitude);
           
        }

        if (collision.gameObject.CompareTag("OffMap"))
        {
            GameManager.instance.LoseLife();

        }
    }


    private void playerDamaged(Collision collision, float velocity)
    {
        var enemy = collision.gameObject.GetComponent<KnockbackStats>();
        if (enemy != null)
        {
            //calls the damaged function on the knockback manager
            Damaged(enemy.transform, enemy.knockback * velocity);

            //message will be recieved by UI script
            SendMessage("UpdateKnockbackNumber",Mathf.Round(knockbackPercentage) * 1);


        }
    }

    
}

