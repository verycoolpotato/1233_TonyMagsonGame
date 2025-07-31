using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyKnockbackController : KnockbackManager
{
    [SerializeField] private NavMeshAgent NavMeshAgent;
    [SerializeField] private float RingoutThreshold = 7f;

    [SerializeField] private TextMeshProUGUI KnockbackPercentText;

    [SerializeField] private AudioSource DamagedAudio;

    [SerializeField] private AudioSource DeathAudio;

    private Coroutine _reenableCoroutine;

    //Much of the enemy Knockback behaviour doesnt work quite as intended - needs revision

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
        // Get Knockback amount
        BulletStat stats = collision.gameObject.GetComponent<BulletStat>();

        DamagedAudio.Play();

        //disable navmesh while being knocked back
        NavMeshAgent.enabled = false;

        //perform Knockback
        Damaged(collision.transform, stats.Knockback);

        UpdateUI();

        // If Knockback is over threshold, disable navmesh permanently and unlock constraints
        if (KnockbackPercentage >= RingoutThreshold)
        {
            Rb.constraints = RigidbodyConstraints.None;
           

           
            DeathAudio.Play();

            KnockbackPercentage = 1000;
            
            Damaged(collision.transform, stats.Knockback);

            KnockbackPercentText.text = "";
            return;
        }

        

        

       

        // Only run one coroutine at a time (prevents breaking everything)
        if (_reenableCoroutine != null)
        {
            StopCoroutine(_reenableCoroutine);
        }

        _reenableCoroutine = StartCoroutine(reenableWhenStopped());
    }

    // Re-enable the NavMeshAgent once the enemy has stopped moving
    private IEnumerator reenableWhenStopped()
    {
        yield return new WaitUntil(() => Rb.velocity.magnitude < 0.2f);
        NavMeshAgent.enabled = true;
    }

    //updates the enemy percentage counter ("Health bar")
    private void UpdateUI()
    {
        KnockbackPercentText.text = KnockbackPercentage.ToString() + "%";
        if(KnockbackPercentage > 70)
        {
            KnockbackPercentText.color = Color.red;
        }
    }
}
