using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockbackManager : MonoBehaviour
{
    [SerializeField] float despawnTimer;

    [SerializeField] private NavMeshAgent navMeshAgent;
    
    [SerializeField] private Rigidbody rb;
    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.CompareTag("Projectile"))
        {
            damaged(collision);
            
        }
    }

    //decrease mass when hit, less mass means more knockback is recieved
    private void damaged(Collision collision)
    {
        BulletStat stats = collision.gameObject.GetComponent<BulletStat>();
       
        rb.mass -= stats.knockback;
       
        
        rb.mass = Mathf.Clamp(rb.mass, 0.1f, 10);


        //if mass is too low diable movement and enter ringout state
        if (rb.mass <1)
            ringout();

        Vector3 knockbackDirection = new Vector3(0, collision.GetContact(0).normal.y, 0);

        rb.AddForce(knockbackDirection, ForceMode.Impulse);

       
    }

   

    //allows the enemy to go flying when hit and despawns after a set time
    private void ringout()
    {
        
        navMeshAgent.enabled = false;

        Destroy(gameObject, despawnTimer);
    }
   
}
