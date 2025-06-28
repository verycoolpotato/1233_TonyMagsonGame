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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            enemyDamaged(collision);

            //Alert other scripts on this object that it has been hit
            SendMessage("KnockedBack");
        }

        if (collision.gameObject.CompareTag("OffMap"))
        {
            Destroy(gameObject);

        }
    }

    private void enemyDamaged(Collision collision)
    {
        // Get knockback amount
        BulletStat stats = collision.gameObject.GetComponent<BulletStat>();
        if (stats == null)
        {
            Debug.LogWarning("Projectile hit but has no bullet stat");
            return;
        }

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
        damaged(collision.transform, stats.knockback);

        UpdateUI();

        // Only run one coroutine at a time
        if (reenableCoroutine != null)
        {
            StopCoroutine(reenableCoroutine);
        }

        reenableCoroutine = StartCoroutine(reenableWhenStopped());
    }

    // Re-enable the NavMeshAgent once the enemy has slowed down
    private IEnumerator reenableWhenStopped()
    {
        yield return new WaitUntil(() => rb.velocity.magnitude < 0.1f);
        navMeshAgent.enabled = true;
    }

    private void UpdateUI()
    {
        knockbackPercentText.text = knockbackPercentage.ToString() + "%";
        if(knockbackPercentage > 70)
        {
            knockbackPercentText.color = Color.red;
        }
    }
}
