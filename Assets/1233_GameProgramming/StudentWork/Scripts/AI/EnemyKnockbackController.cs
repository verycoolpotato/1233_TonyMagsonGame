using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKnockbackController : KnockbackManager
{
   

    [SerializeField] private NavMeshAgent navMeshAgent;

    [SerializeField] private float ringoutThreshold = 10f;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Projectile"))
        {
           
            enemyDamaged(collision);
        }
    }

    private void enemyDamaged(Collision collision)
    {
        //if percentage is high, unlock rotation and increase knockback
        if (knockbackPercentage >= ringoutThreshold)
        {
            rb.constraints = RigidbodyConstraints.None;
            knockbackPercentage = -300;

        }
        // Temporarily disable navmesh
        navMeshAgent.enabled = false;

        //ensure only one instance of the coroutine is running
        StopCoroutine(reenableWhenStopped());



        //enact knockback
        BulletStat stats = collision.gameObject.GetComponent<BulletStat>();
        if (stats == null) 
        {
            Debug.Log("No knockback recieved on hit");
            return;
        }

        damaged(collision, stats.knockback);

        StartCoroutine(reenableWhenStopped());
    }

    
   

    //wait for enemy to stop moving to re-enable navmesh
    private IEnumerator reenableWhenStopped()
    {
        yield return new WaitUntil(() => rb.velocity.magnitude < 0.1f);
        navMeshAgent.enabled = true;
    }






}
