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

    [SerializeField] private GameObject IceObj;

    private bool carryingIce;

    private Vector3 targetPos;
    private Vector3 MoveDirection;

    private int _animAxisX;
    private int _animHit;
    private int _animAxisZ;




    private void Start()
    {
        //Randomise state every x seconds
        
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

    //Run towards player
    private void ChargeState()
    {
            _agent.destination = targetPos;
            _agent.speed = chargeSpeed;


        

        MoveDirection.z = 1; 
        MoveDirection.x = 0;
    }

    
    private void IceGrab()
    {
        if (!carryingIce)
        {
            carryingIce = true;
          
        }
        IceObj.SetActive(carryingIce);
    }

    private void IceThrow()
    {
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
        MoveDirection = Vector3.zero;
        _animator.SetTrigger(_animHit);
        
    }
   
}
