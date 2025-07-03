using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudentWork
{ 
    public class ShootingManager : MonoBehaviour
    {

        [SerializeField] GameObject hitParticle;
        [Header("Projectile Settings")]

        [Tooltip("Primary thrown projectile")]
        [SerializeField] private GameObject snowballPrefab;

        [Tooltip("Projectile throw force (speed)")]
        [SerializeField] private float throwForce;

        [Tooltip("Player input reference")]
        [SerializeField] private PlayerInputs input;

        [Tooltip("This Animator")]
        [SerializeField] private Animator animator;

        [SerializeField] private Transform throwPos;

        [SerializeField] private AudioSource audioSource;

        private Camera mainCamera;

        private int animIDThrow;

 

        private void Awake()
        {
            animIDThrow = Animator.StringToHash("Throw");
            mainCamera = Camera.main;
        }
        private void Update()
        {
            CanShoot();
        }

        //check if player is pressing aim and shoot, if yes then play animation
        private void CanShoot()
        {
           if(input.Aim && input.Shoot)
            {
                animator.SetTrigger(animIDThrow);
                input.Shoot = false;
            }
        }

        //Called by animation event, allows player to queue next shot
        public void InputQueue()
        {
            animator.ResetTrigger(animIDThrow);
        }

        public void ProjectileShot()
        { 
            Vector3 projectileSpawn = throwPos.position;

            //Instantiate a projectile with name clone at thirdPersonCamera position
            GameObject Clone = Instantiate(snowballPrefab,projectileSpawn, Quaternion.identity);

            //Set clone rotation and velocity, speed stored as variable throwForce
            Clone.transform.rotation = mainCamera.transform.rotation;
            Clone.GetComponent<Rigidbody>().AddForce(Clone.transform.forward * throwForce, ForceMode.Impulse);

            //Delete clone after 3 seconds
            Destroy(Clone,3f);

        }
        public void PlayThrowAudio()
        {
            audioSource.Play(); 
        }
    }
}
