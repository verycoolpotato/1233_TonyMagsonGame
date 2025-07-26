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
    [SerializeField] private float runSpeed;

    [Tooltip("How fast does the enemy move when in the ice grab, ice throw states")]
    [SerializeField] private float walkSpeed;

    [Tooltip("Current state of the enemy AI")]
    [SerializeField] private AIStates state;

    [Tooltip("How far the player is knocked back on contact with this character")]
    [SerializeField] public float knockback;

    [Tooltip("This Navmesh Agent")]
    [SerializeField] private NavMeshAgent agent;

    [Tooltip("This Animator")]
    [SerializeField] private Animator animator;

    [Tooltip("Static ice chunk that appears above enemy")]
    [SerializeField] private GameObject iceObj;

   


    [SerializeField] private GameObject hitParticle;

    private bool carryingIce;

    //Position to move towards
    private Vector3 targetPos;

    //Direction of movement used by Animator
    private Vector3 moveDirection;

    private float countFrom = 2;
    private float time;

    private int animAxisX;
    private int animHit;
    

    //Is in idle state?
    private bool idle;


    private void Start()
    {
        idle = true;
        //Randomise state every x seconds
        
        SetAnimID();
        agent.updateRotation = true;
       
    }
    private void Update()
    {
        AnimateCharacter();

        //if possible move towards player
        if(agent != null && agent.enabled)
        {
             agent.SetDestination(targetPos);
        }
        
        
        targetPos = PlayerLocatorSingleton.Instance.transform.position;

        //Check for player in range and idle state
        if (!idle)
        {
            StateTimer(countFrom);
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
        animAxisX = Animator.StringToHash("X");
        
        animHit = Animator.StringToHash("Hit");
    }
   
    //set walking anims based on x and z of movedirection
    private void AnimateCharacter()
    {
        animator.SetFloat(animAxisX, moveDirection.x, 0.1f, Time.deltaTime);
       
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
        if (PlayerLocatorSingleton.Instance != null && agent.enabled)
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
            PlayerLocatorSingleton.Instance.transform.position - agent.transform.position;

        idle = distance.magnitude > 15;

        agent.speed = 0;
       
        moveDirection.x = 0;

    }

    //Run towards player
    private void ChargeState()
    {

            agent.speed = runSpeed;

        countFrom = 4;

        
        moveDirection.x = 1;
    }

    //if not carrying ice pick up ice
    private void IceGrab()
    {
       
        moveDirection.x = 1;
        agent.speed = walkSpeed;
        countFrom = 1;

        if (!carryingIce)
        {
            carryingIce = true;
          
        }
        iceObj.SetActive(carryingIce);
    }

    //if carrying ice then throw ice at player
    private void IceThrow()
    {
       
        moveDirection.x = 1;
        agent.speed = walkSpeed;
        countFrom = 2;


        if (carryingIce)
        {
            SendMessage("ThrowIce");
            carryingIce = false;
        }
        iceObj.SetActive(carryingIce);
    }
   
    //recieves from enemy knockback manager when hit by projectile
    public void KnockedBack()
    {
        //End idle state
        idle = false;
        agent.speed = walkSpeed;

        //Stop walking animation
        moveDirection = Vector3.zero;

        Instantiate(hitParticle,transform.position,Quaternion.identity);

        //Play hit animation
        animator.SetTrigger(animHit);
        
    }
   
    

}
