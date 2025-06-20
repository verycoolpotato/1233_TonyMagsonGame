using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnowmanMover : MonoBehaviour
{
    private Vector3 targetPos;
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
    [SerializeField] private AIStates state; 

    [SerializeField] public float knockback;
    
    [SerializeField] private NavMeshAgent _agent;

    private void Start()
    {
        _agent.updateRotation = false;
    }
    private void Update()
    {
        stateSwitcher();


        targetPos = PlayerLocatorSingleton.Instance.transform.position;

        FacePlayer();
    }
    private void FacePlayer()
    {
        Vector3 LookPos = targetPos - transform.position;

        LookPos.y = 0;

       Quaternion rotation = Quaternion.LookRotation(LookPos);

        transform.rotation = Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime * rotateSpeed);
    }
    private void stateSwitcher()
    {
        if (PlayerLocatorSingleton.Instance != null && _agent.enabled)
        {
            switch (state)
            {
                case AIStates.Charge:
                    chargeState();
                    break;

                case AIStates.StrafeLeft:
                    strafeState(1);
                    break;

                case AIStates.StrafeRight:
                    strafeState(-1);
                    break;
            }
        }
        
    }
    private void chargeState()
    {
        
            _agent.destination = targetPos;
            _agent.speed = chargeSpeed;
            
        
        
    }
    private void strafeState(float strafeDir)
    {
        
        var dir = Vector3.Cross(targetPos - transform.position, Vector3.up);

        _agent.SetDestination(transform.position + dir * strafeDir);
        _agent.speed = strafeSpeed;
        
    }
   
   


}
