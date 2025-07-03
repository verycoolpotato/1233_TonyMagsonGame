using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyKnockbackController : KnockbackManager
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float ringoutThreshold = 7f;

    [SerializeField] private TextMeshProUGUI knockbackPercentText;

    private Coroutine reenableCoroutine;

    //Much of the enemy knockback behaviour doesnt work quite as intended - needs revision

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            EnemyDamaged(collision);

            //Alert other scripts on this object that it has been hit - used by SnowmanMover script
            SendMessage("KnockedBack");
        }

        if (collision.gameObject.CompareTag("OffMap"))
        {
            Destroy(gameObject);

        }
    }


    private void EnemyDamaged(Collision collision)
    {
        // Get knockback amount
        BulletStat stats = collision.gameObject.GetComponent<BulletStat>();

    

        // If knockback is over threshold, disable navmesh permanently and unlock constraints
        if (knockbackPercentage >= ringoutThreshold)
        {
            rb.constraints = RigidbodyConstraints.None;
            navMeshAgent.enabled = false;
           
            knockbackPercentText.text = "";
            return;
        }

        //disable navmesh while being knocked back
        navMeshAgent.enabled = false;

        //perform knockback
        Damaged(collision.transform, stats.knockback);

        UpdateUI();

        // Only run one coroutine at a time (prevents breaking everything)
        if (reenableCoroutine != null)
        {
            StopCoroutine(reenableCoroutine);
        }

        reenableCoroutine = StartCoroutine(reenableWhenStopped());
    }

    // Re-enable the NavMeshAgent once the enemy has stopped moving
    private IEnumerator reenableWhenStopped()
    {
        yield return new WaitUntil(() => rb.velocity.magnitude < 0.3f);
        navMeshAgent.enabled = true;
    }

    //updates the enemy percentage counter ("Health bar")
    private void UpdateUI()
    {
        knockbackPercentText.text = knockbackPercentage.ToString() + "%";
        if(knockbackPercentage > 70)
        {
            knockbackPercentText.color = Color.red;
        }
    }
}
