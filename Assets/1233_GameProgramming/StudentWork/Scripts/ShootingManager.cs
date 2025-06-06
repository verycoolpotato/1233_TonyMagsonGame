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

        private Camera _camera;

        private int _animIDThrow;

        private enum Firetype {Projectile, Hitscan}

        [SerializeField] private Firetype fireType;

        [SerializeField] LayerMask raycastMask;

        private void Awake()
        {
            _animIDThrow = Animator.StringToHash("Throw");
            _camera = Camera.main;
        }
        private void Update()
        {
            CanShoot();
        }

        //check if player is pressing aim and shoot
        private void CanShoot()
        {
           if(_input.Aim && _input.Shoot)
            {
                switch (fireType)
                {
                    case Firetype.Hitscan:
                        RaycastShot();
                    break;

                    case Firetype.Projectile:
                        ProjectileShot(SnowballPrefab);
                    break;
                }  
                _animator.SetTrigger(_animIDThrow);
                _input.Shoot = false;
                

            }
        }

        private void ProjectileShot(GameObject projectile)
        {
            
            //Instantiate a projectile with name clone at camera position
            GameObject Clone = Instantiate(projectile,_camera.transform.position, Quaternion.identity);

            //Set clone rotation and velocity, speed stored as variable throwForce
            Clone.transform.rotation = _camera.transform.rotation;
            Clone.GetComponent<Rigidbody>().AddForce(Clone.transform.forward * throwForce, ForceMode.Impulse);

            //Delete clone after 3 seconds
            Destroy(Clone,3f);

        }
        private void RaycastShot()
        {
            if( Physics.Raycast(_camera.transform.position,_camera.transform.forward,out RaycastHit hit,Mathf.Infinity,raycastMask))
            {
                HitscanImpact(hit.point, hit.normal);
            }
        }

        private void HitscanImpact(Vector3 position,  Vector3 rotation)
        {
            GameObject Clone = Instantiate(hitParticle,position,Quaternion.Euler(rotation));
            Destroy(Clone, 2);
        }

       

       
    }

    

}
