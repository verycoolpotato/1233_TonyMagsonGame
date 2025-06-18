using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerKnockbackController : KnockbackManager
{
    [Tooltip("This Character Controller")]
    [SerializeField] private CharacterController _controller;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerDamaged(collision);
        }
    }

    private void playerDamaged(Collision collision)
    {
        float knockback = collision.gameObject.GetComponent<SnowmanMover>().knockback;
       
        _controller.enabled = false;
        rb.isKinematic = false;
        
        damaged(collision,knockback);
        Invoke("enableCharacterController", 1);



    }
    //wait for player to stop moving to re-enable character controller
    private void enableCharacterController()
    {
        _controller.enabled = true;
        rb.isKinematic = true;
    }
}
