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
        [SerializeField] private GameObject SnowballPrefab;

        [Tooltip("Projectile throw force (speed)")]
        [SerializeField] private float throwForce;

        [Tooltip("Player input reference")]
        [SerializeField] private PlayerInputs _input;

        [Tooltip("This Animator")]
        [SerializeField] private Animator _animator;

        [SerializeField] private Transform ThrowPos;

        [SerializeField] private AudioSource _audioSource;

        private Camera _camera;

        private int _animIDThrow;

 

        private void Awake()
        {
            _animIDThrow = Animator.StringToHash("Throw");
            _camera = Camera.main;
        }
        private void Update()
        {
            CanShoot();
        }

        //check if player is pressing aim and shoot, if yes then play animation
        private void CanShoot()
        {
           if(_input.Aim && _input.Shoot)
            {
                _animator.SetTrigger(_animIDThrow);
                _input.Shoot = false;
            }
        }

        //Called by animation event, allows player to queue next shot
        public void InputQueue()
        {
            _animator.ResetTrigger(_animIDThrow);
        }

        public void ProjectileShot()
        { 
            Vector3 projectileSpawn = ThrowPos.position;

            //Instantiate a projectile with name clone at camera position
            GameObject Clone = Instantiate(SnowballPrefab,projectileSpawn, Quaternion.identity);

            //Set clone rotation and velocity, speed stored as variable throwForce
            Clone.transform.rotation = _camera.transform.rotation;
            Clone.GetComponent<Rigidbody>().AddForce(Clone.transform.forward * throwForce, ForceMode.Impulse);

            //Delete clone after 3 seconds
            Destroy(Clone,3f);

        }
        public void PlayThrowAudio()
        {
            _audioSource.Play(); 
        }
    }
}
