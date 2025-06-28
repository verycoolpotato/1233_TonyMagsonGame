using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnowmanMover : MonoBehaviour
{
   
    //Possible AI states
    private enum AIStates
    {
        Charge,
        IceGrab,
        IceThrow,
    }

    [Tooltip("How fast does the enemy move when in the charge state")]
    [SerializeField] private float RunSpeed;

    [Tooltip("How fast does the enemy move when in the ice grab, ice throw states")]
    [SerializeField] private float walkSpeed;

    [Tooltip("Current state of the enemy AI")]
    [SerializeField] private AIStates state;

    [Tooltip("How far the player is knocked back on contact with this character")]
    [SerializeField] public float knockback;

    [Tooltip("This Navmesh Agent")]
    [SerializeField] private NavMeshAgent _agent;

    [Tooltip("This Animator")]
    [SerializeField] private Animator _animator;

    [Tooltip("Static ice chunk that appears above enemy")]
    [SerializeField] private GameObject IceObj;

 
    private bool carryingIce;

    //Position to move towards
    private Vector3 targetPos;

    //Direction of movement used by animator
    private Vector3 MoveDirection;

    private float CountFrom = 2;
    private float time;

    private int _animAxisX;
    private int _animHit;
    private int _animAxisZ;

    //Is in idle state?
    private bool Idle;


    private void Start()
    {
        Idle = true;
        //Randomise state every x seconds
        
        SetAnimID();
        _agent.updateRotation = true;
       
    }
    private void Update()
    {
        AnimateCharacter();

        //if possible move towards player
        if(_agent != null && _agent.enabled)
        {
             _agent.SetDestination(targetPos);
        }
        
        
        targetPos = PlayerLocatorSingleton.Instance.transform.position;

        //Check for player in range and idle state
        if (!Idle)
        {
            StateTimer(CountFrom);
            StateSwitcher();
        }
        else
        {
            IdleState();
        }
        
    }

    //Counts down from a timer and calls shufflestate when the time is up
    private void StateTimer(float CountFrom)
    {
        time -= 1 * Time.deltaTime;
        if (time < 0)
        {
            ShuffleState();
            time = CountFrom;
        }
    }
    //Assign anim ids on start
    private void SetAnimID()
    {
        _animAxisX = Animator.StringToHash("X");
        _animAxisZ = Animator.StringToHash("Z");
        _animHit = Animator.StringToHash("Hit");
    }
   
    //set walking anims based on x and z of movedirection
    private void AnimateCharacter()
    {
        _animator.SetFloat(_animAxisX, MoveDirection.x, 0.1f, Time.deltaTime);
        _animator.SetFloat(_animAxisZ, MoveDirection.z, 0.1f, Time.deltaTime);
    }
    

    //Randomise state when called
    private void ShuffleState()
    {
        int newState = Random.Range(0, 3);
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

                case AIStates.IceGrab:
                    IceGrab();
                    break;

                case AIStates.IceThrow:
                    IceThrow();
                    break;

                
            }
        }
    }

    //Check for player distance and end idle state when alerted
    private void IdleState()
    {
        Vector3 distance =
            PlayerLocatorSingleton.Instance.transform.position - _agent.transform.position;

        Idle = distance.magnitude > 15;

        _agent.speed = 0;
        MoveDirection.z = 0;
        MoveDirection.x = 0;

    }

    //Run towards player
    private void ChargeState()
    {

            _agent.speed = RunSpeed;

        CountFrom = 4;

        MoveDirection.z = 1; 
        MoveDirection.x = 0;
    }

    //if not carrying ice pick up ice
    private void IceGrab()
    {
        MoveDirection.z = 1;
        MoveDirection.x = 0;
        _agent.speed = walkSpeed;
        CountFrom = 1;

        if (!carryingIce)
        {
            carryingIce = true;
          
        }
        IceObj.SetActive(carryingIce);
    }

    //if carrying ice then throw ice at player
    private void IceThrow()
    {
        MoveDirection.z = 1;
        MoveDirection.x = 0;
        _agent.speed = walkSpeed;
        CountFrom = 2;


        if (carryingIce)
        {
            SendMessage("ThrowIce");
            carryingIce = false;
        }
        IceObj.SetActive(carryingIce);
    }
   
    //recieves from enemy knockback manager when hit by projectile
    public void KnockedBack()
    {
        //End idle state
        Idle = false;
        _agent.speed = walkSpeed;

        //Stop walking animation
        MoveDirection = Vector3.zero;

        //Play hit animation
        _animator.SetTrigger(_animHit);
        
    }
   
}
