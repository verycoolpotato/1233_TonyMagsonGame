using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudentWork
{ 
    public class ShootingManager : MonoBehaviour
    {

        [SerializeField] GameObject HitParticle;
        [Header("Projectile Settings")]

        [Tooltip("Primary thrown projectile")]
        [SerializeField] private GameObject SnowballPrefab;

        [Tooltip("Projectile throw force (speed)")]
        [SerializeField] private float ThrowForce;

        [Tooltip("Player Input reference")]
        [SerializeField] private PlayerInputs Input;

        [Tooltip("This Animator")]
        [SerializeField] private Animator Animator;

        [SerializeField] private Transform ThrowPos;

        [SerializeField] private AudioSource AudioSource;

        [SerializeField] private Camera MainCamera;

        private int _animIDThrow;

 

        private void Awake()
        {
            _animIDThrow = Animator.StringToHash("Throw");
           
        }
        private void Update()
        {
            CanShoot();
        }

        //check if player is pressing aim and shoot, if yes then play animation
        private void CanShoot()
        {
           if(Input.Aim && Input.Shoot)
            {
                Animator.SetTrigger(_animIDThrow);
                Input.Shoot = false;
            }
        }

        //Called by animation event, allows player to queue next shot
        public void InputQueue()
        {
            Animator.ResetTrigger(_animIDThrow);
        }

        public void ProjectileShot()
        { 
            Vector3 projectileSpawn = ThrowPos.position;

            //Instantiate a projectile with name clone at thirdPersonCamera position
            GameObject Clone = Instantiate(SnowballPrefab,projectileSpawn, Quaternion.identity);

            //Set clone rotation and velocity, speed stored as variable ThrowForce
            Clone.transform.rotation = MainCamera.transform.rotation;
            Clone.GetComponent<Rigidbody>().AddForce(Clone.transform.forward * ThrowForce, ForceMode.Impulse);

            //Delete clone after 3 seconds
            Destroy(Clone,3f);

        }
        public void PlayThrowAudio()
        {
            AudioSource.Play(); 
        }
    }
}
