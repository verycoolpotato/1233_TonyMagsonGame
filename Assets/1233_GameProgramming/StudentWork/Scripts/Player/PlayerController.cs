using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StudentWork
{
    [RequireComponent(typeof(Rigidbody))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class PlayerController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float moveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float sprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float rotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float speedChangeRate = 10.0f;

        [SerializeField] private KnockbackManager knockbackManager;
        
        public AudioClip landingAudioClip;
        public AudioClip[] footstepAudioClips;
        [Range(0, 1)] public float footstepAudioVolume = 0.5f;

        [SerializeField] private AudioMixer audioMixer;

        [Space(10)]
        [Tooltip("The height the player can Jump")]
        public float jumpHeight = 1.2f;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the thirdPersonCamera will follow")]
        public GameObject cinemachineCameraTarget;

        [Tooltip("How far in degrees can you Move the thirdPersonCamera up")]
        public float topClamp = 70.0f;

        [Tooltip("How far in degrees can you Move the thirdPersonCamera down")]
        public float bottomClamp = -30.0f;

        [Tooltip("Additional degress to override the thirdPersonCamera. Useful for fine tuning thirdPersonCamera position when locked")]
        public float cameraAngleOverride = 0.0f;

        [Tooltip("For locking the thirdPersonCamera position on all axis")]
        public bool lockCameraPosition = false;

        [Header("Cameras")]

        [Tooltip("Player Standard Camera")]
        [SerializeField] private GameObject thirdPersonCamera;

        [Tooltip("Player Camera when aiming")]
        [SerializeField] private GameObject aimCamera;

        [Tooltip("Main Camera (Always Active)")]
        [SerializeField] private Camera mainCamera;

        [Tooltip("This Animator")]
        [SerializeField] private Animator animator;

        [Tooltip("This Rigidbody")]
        [SerializeField] private Rigidbody rb;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask groundLayers;

        // cinemachine
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;

        // player
        private float speed;
        private float animationBlend;
        private float targetRotation = 0.0f;
        private float rotationVelocity;
        private Vector3 lastGroundedPosition;


        public bool Grounded = true;

        // animation IDs
        private int animIDSpeed;
   
        private int animIDMotionSpeed;
        private int _animIDGrounded;
        private int animAxisX;
        private int animAxisZ;
        private int animIDRunning;
        private int animIDLives;

       

#if ENABLE_INPUT_SYSTEM
        [SerializeField] public PlayerInput playerInput;
#endif
      
        [SerializeField] private PlayerInputs input;

        //Disables standard player rotation while walking
        public bool strafing;

      

        private const float threshold = 0.01f;

        private bool hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }
        private void FixedUpdate()
        {
            Move();
        }

        private void OnEnable()
        {
            knockbackManager.knockbackPercentage = 0;
            transform.position = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
        private void Start()
        {
            

            cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            hasAnimator = TryGetComponent(out animator);

            AssignAnimationIDs();

        }
        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - 0.28f,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, 0.1f, groundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (animator != null)
            {
                animator.SetBool(_animIDGrounded, Grounded);
            }
        }
        public void SnapBackToGround()
        {
            transform.position = lastGroundedPosition;
            rb.velocity = Vector3.zero;
        }

        private void Update()
        {
            GroundedCheck();

            

            hasAnimator = TryGetComponent(out animator);

            AimState(input.Aim);

            if(Grounded)
                lastGroundedPosition = transform.position;
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
           

            animIDRunning = Animator.StringToHash("Running");
            animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");

            animIDLives = Animator.StringToHash("Lives");

            animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
           
            animAxisX = Animator.StringToHash("X");
            animAxisZ = Animator.StringToHash("Z");
        }

       
       
        private void CameraRotation()
        {
            // if there is an input and thirdPersonCamera position is not fixed
            if (input.Look.sqrMagnitude >= threshold && !lockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                cinemachineTargetYaw += input.Look.x * deltaTimeMultiplier;
                cinemachineTargetPitch += input.Look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

            // Cinemachine will follow this target
            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride,
                cinemachineTargetYaw, 0.0f);
        }


        //Swaps to aiming thirdPersonCamera when recieving aim input
        private void AimState(bool AimButton)
        {
            aimCamera.SetActive(AimButton);
            
        }

     
        private void Move()
        {
           
            // set target speed based on Move speed, Sprint speed and if Sprint is pressed
            float targetSpeed = input.Sprint ? sprintSpeed : moveSpeed;

            animator.SetBool(animIDRunning, input.Sprint);
          
            // if there is no input, set the target speed to 0
            if (input.Move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z).magnitude;

           

            float speedOffset = 0.1f;
            float inputMagnitude = input.analogMovement ? input.Move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                        Time.fixedDeltaTime * speedChangeRate);


                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(input.Move.x, 0.0f, input.Move.y).normalized;



            //determine direction to Move
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  mainCamera.transform.eulerAngles.y;

            
            
            //switch player rotation target when performing certain actions
            float LookDirection;

            if (input.Sprint)
                LookDirection = targetRotation;
            else
                LookDirection = mainCamera.transform.rotation.eulerAngles.y;
            


            //rotation player should be facing
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, LookDirection, ref rotationVelocity,
                    rotationSmoothTime);

               
                
            // rotate to face input direction relative to thirdPersonCamera position
             transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                
        
            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward  * Time.fixedDeltaTime * speed;
            

            // Apply force if there is input - MAIN MOVEMENT CONTROL
            if (input.Move.x != 0 || input.Move.y != 0)
            {
                rb.AddForce(targetDirection,ForceMode.Acceleration);
            }
            

            // update animator if using character
            if (hasAnimator)
            {
                animator.SetFloat(animIDSpeed, animationBlend);
                animator.SetFloat(animIDMotionSpeed, inputMagnitude);

                animator.SetFloat(animAxisX, inputDirection.x, 0.1f, Time.deltaTime);
                animator.SetFloat(animAxisZ, inputDirection.z, 0.1f, Time.deltaTime);

                animator.SetInteger(animIDLives, GameManager.instance.lives);
            }
        }
       
        

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (footstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, footstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(footstepAudioClips[index], transform.TransformPoint(rb.transform.position), footstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(landingAudioClip, transform.TransformPoint(rb.transform.position), footstepAudioVolume);
            }
        }
    }
}