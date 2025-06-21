using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnowmanMover : MonoBehaviour
{
   
    //all possible ai states
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

    [SerializeField] private Animator _animator;

    [Tooltip("How long each state lasts")]
    [SerializeField] private float stateDuration = 1;

    private Vector3 targetPos;
    private Vector3 MoveDirection;

    private int _animAxisX;
    private int _animHit;
    private int _animAxisZ;




    private void Start()
    {
        //Randomise state every x seconds
        InvokeRepeating(nameof(ShuffleState), 1,stateDuration);
        SetAnimID();
        _agent.updateRotation = false;
    }
    private void Update()
    {
        AnimateCharacter();
      

        StateSwitcher();
        targetPos = PlayerLocatorSingleton.Instance.transform.position;
        FacePlayer();
    }

 


    private void SetAnimID()
    {
        _animAxisX = Animator.StringToHash("X");
        _animAxisZ = Animator.StringToHash("Z");
        _animHit = Animator.StringToHash("Hit");
    }
   

    private void AnimateCharacter()
    {
        _animator.SetFloat(_animAxisX, MoveDirection.x, 0.1f, Time.deltaTime);
        _animator.SetFloat(_animAxisZ, MoveDirection.z, 0.1f, Time.deltaTime);

    }
    //Rotate towards player
    private void FacePlayer()
    {
        Vector3 LookPos = targetPos - transform.position;
        LookPos.y = 0;
       Quaternion rotation = Quaternion.LookRotation(LookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation,rotation,Time.deltaTime * rotateSpeed);
    }

    //Randomise state when called - CHANGE LATER TO ALLOW CUSTOM WEIGHT VALUES FOR STATES
    private void ShuffleState()
    {
        int newState = Random.Range(0, 2);
        state = (AIStates)newState;
    }

    

    //Prevent multiple states from being active
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
                case AIStates.Block:
                    BlockState();
                    break;
               
            }
        }
    }

    //Run towards player
    private void ChargeState()
    {
            _agent.destination = targetPos;
            _agent.speed = chargeSpeed;


        stateDuration = 0.5f;

        MoveDirection.z = 1; 
        MoveDirection.x = 0;
    }

    //strafe left or right, direction is determined by strafeDir which is set to either 1 or -1
    private void StrafeState(float strafeDir)
    {
        var dir = Vector3.Cross(targetPos - transform.position, Vector3.up);

        _agent.SetDestination(transform.position + dir * strafeDir);
        _agent.speed = strafeSpeed;

        stateDuration = 0.08f;
        MoveDirection.x = strafeDir;
        MoveDirection.z = 0;
    }
    

   
   private void BlockState()
    {
        MoveDirection = Vector3.zero;

        
    }
   

    //recieves from enemy knockback manager when hit by projectile
    public void KnockedBack()
    {
        state = AIStates.Block;
        _animator.SetTrigger(_animHit);
        
    }
   
}
