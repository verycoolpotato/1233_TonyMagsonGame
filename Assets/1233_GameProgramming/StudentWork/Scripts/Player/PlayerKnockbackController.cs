using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerKnockbackController : KnockbackManager
{
    [SerializeField] private GameObject enemy;
    private void Update()
    {
        //debug stuff, will be deleted
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("World");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Instantiate(enemy, new Vector3(0,0,0),Quaternion.identity);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerDamaged(collision, collision.relativeVelocity.magnitude);
           
        }

        if (collision.gameObject.CompareTag("OffMap"))
        {
            SceneManager.LoadScene("World");

        }
    }


    private void playerDamaged(Collision collision, float velocity)
    {
        var enemy = collision.gameObject.GetComponent<SnowmanMover>();
        if (enemy != null)
        {
            damaged(enemy.transform, enemy.knockback * velocity);
            SendMessage("UpdateKnockbackNumber",Mathf.Round(knockbackPercentage) * 3);
        }
    }

    
}

