using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
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
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can Jump")]
        public float JumpHeight = 1.2f;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you Move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you Move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        [Header("Cameras")]

        [Tooltip("Player Standard Camera")]
        [SerializeField] private GameObject _mainCamera;

        [Tooltip("Player Camera when aiming")]
        [SerializeField] private GameObject _aimCamera;

        [Tooltip("Main Camera (Always Active)")]
        [SerializeField] private Camera _camera;

        [Tooltip("This Animator")]
        [SerializeField] private Animator _animator;

        [Tooltip("This Rigidbody")]
        [SerializeField] private Rigidbody _rb;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
       

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDCrouch;
        private int _animAxisX;
        private int _animAxisZ;
        private int _animIDRunning;
       

#if ENABLE_INPUT_SYSTEM
       [SerializeField] private PlayerInput _playerInput;
#endif
      
        [SerializeField] private PlayerInputs _input;

        //Disables standard player rotation while walking
        public bool Strafing;

      

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }
        private void FixedUpdate()
        {
            Move();
            
        }


        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);

            AssignAnimationIDs();

           
           
        }

        private void Update()
        {
            

            _hasAnimator = TryGetComponent(out _animator);

            AimState(_input.Aim);
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            
            _animIDRunning = Animator.StringToHash("Running");
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
           
            _animAxisX = Animator.StringToHash("X");
            _animAxisZ = Animator.StringToHash("Z");
        }

       
       
        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.Look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.Look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.Look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }


        //Swaps to aiming camera when recieving aim input
        private void AimState(bool AimButton)
        {
            _aimCamera.SetActive(AimButton);
            
        }

     
        private void Move()
        {
           
            // set target speed based on Move speed, Sprint speed and if Sprint is pressed
            float targetSpeed = _input.Sprint ? SprintSpeed : MoveSpeed;

            _animator.SetBool(_animIDRunning, _input.Sprint);
          
            // if there is no input, set the target speed to 0
            if (_input.Move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_rb.velocity.x, 0.0f, _rb.velocity.z).magnitude;

           

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.Move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                        Time.fixedDeltaTime * SpeedChangeRate);


                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

           


            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;



            //determine direction to Move
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;

            
            
            //switch player rotation target when performing certain actions
            float LookDirection;

            if (_input.Sprint)
                LookDirection = _targetRotation;
            else
                LookDirection = _camera.transform.rotation.eulerAngles.y;
            


            //rotation player should be facing
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, LookDirection, ref _rotationVelocity,
                    RotationSmoothTime);

               
                
            // rotate to face input direction relative to camera position
             transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                
        
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward  * Time.fixedDeltaTime * _speed;

            // Apply force if there is input
            if (_input.Move.x != 0 || _input.Move.y != 0)
            {
                _rb.AddForce(targetDirection,ForceMode.Acceleration);
            }













            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);

                _animator.SetFloat(_animAxisX, inputDirection.x, 0.1f, Time.deltaTime);
                _animator.SetFloat(_animAxisZ, inputDirection.z, 0.1f, Time.deltaTime);
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
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_rb.transform.position), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_rb.transform.position), FootstepAudioVolume);
            }
        }
    }
}