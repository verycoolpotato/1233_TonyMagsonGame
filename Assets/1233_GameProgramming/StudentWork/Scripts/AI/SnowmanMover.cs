using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnowmanMover : MonoBehaviour
{
   
    private enum AIStates
    {
        Charge,
        StrafeLeft,
        StrafeRight,
        Block
    }
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float strafeSpeed;

    [Tooltip("Current state of the enemy AI")]
    [SerializeField] private AIStates state;

    [Tooltip("How far the player is knocked back on contact with this character")]
    [SerializeField] public float knockback;
    
    [SerializeField] private NavMeshAgent _agent;

    [Tooltip("Multiplies the duration of each state (Default is 10)")]
    [SerializeField] private float stateDurationMultiplier = 10;

    //How long the state lasts
    private float stateDuration = 0.4f;

    private Vector3 targetPos;

    private void Start()
    {
        InvokeRepeating(nameof(ShuffleState), 1,stateDuration * stateDurationMultiplier);
        _agent.updateRotation = false;
    }
    private void Update()
    {
        StateSwitcher();
        targetPos = PlayerLocatorSingleton.Instance.transform.position;
        FacePlayer();
    }

    private void ShuffleState()
    {
      int newState = Random.Range(0, 3);
        state = (AIStates)newState;
    }

    private void FacePlayer()
    {
        Vector3 LookPos = targetPos - transform.position;
        LookPos.y = 0;
       Quaternion rotation = Quaternion.LookRotation(LookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime * rotateSpeed);
    }

    private void StateSwitcher()
    {
        if (PlayerLocatorSingleton.Instance != null && _agent.enabled)
        {
            switch (state)
            {
                case AIStates.Charge:
                    ChargeState();
                    break;

                case AIStates.StrafeLeft:
                    StrafeState(1);
                    break;

                case AIStates.StrafeRight:
                    StrafeState(-1);
                    break;
            }
        }
    }

    private void ChargeState()
    {
            _agent.destination = targetPos;
            _agent.speed = chargeSpeed;


        stateDuration = 0.5f;
    }

    private void StrafeState(float strafeDir)
    {
        var dir = Vector3.Cross(targetPos - transform.position, Vector3.up);

        _agent.SetDestination(transform.position + dir * strafeDir);
        _agent.speed = strafeSpeed;

        stateDuration = 0.08f;

    }
   
   


}
