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
           
            gameObject.transform.position = Vector3.zero;
            knockbackPercentage = 0;
            if (GameManager.instance != null)
            {
                GameManager.instance.RestartLevel();
            }
            else
            {
                Debug.Log("GameManager is null, ensure the game was started from the GameRunScene");
            }
            

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

