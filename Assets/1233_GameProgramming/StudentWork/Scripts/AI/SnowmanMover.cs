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
        IceGrab,
        IceThrow,
        Idle
    }

   
    [SerializeField] private float RunSpeed;
    [SerializeField] private float walkSpeed;

    [Tooltip("Current state of the enemy AI")]
    [SerializeField] private AIStates state = AIStates.Idle;

    [Tooltip("How far the player is knocked back on contact with this character")]
    [SerializeField] public float knockback;
    
    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private Animator _animator;


    [SerializeField] private GameObject IceObj;

    private bool carryingIce;

    private Vector3 targetPos;
    private Vector3 MoveDirection;

    private float CountFrom = 2;
    private float time;

    private int _animAxisX;
    private int _animHit;
    private int _animAxisZ;




    private void Start()
    {
        //Randomise state every x seconds
        
        SetAnimID();
        _agent.updateRotation = true;
       
    }
    private void Update()
    {
        AnimateCharacter();
        if(_agent != null && _agent.enabled)
        {
            _agent.destination = targetPos;
        }
        
        StateSwitcher();
        targetPos = PlayerLocatorSingleton.Instance.transform.position;
        
        stateTimer(CountFrom);
    }

    private void stateTimer(float CountFrom)
    {
        time -= 1 * Time.deltaTime;
        if (time < 0)
        {
            ShuffleState();
            time = CountFrom;
        }
        
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

                case AIStates.IceGrab:
                    IceGrab();
                    break;

                case AIStates.IceThrow:
                    IceThrow();
                    break;
                
            }
        }
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
        _agent.speed = walkSpeed;
        MoveDirection = Vector3.zero;
        _animator.SetTrigger(_animHit);
        
    }
   
}
