using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudentWork
{ 
    public class ShootingManager : MonoBehaviour
    {
        [Header("Projectile Settings")]

        [Tooltip("Primary thrown projectile")]
        [SerializeField] private GameObject SnowballPrefab;

        [Tooltip("Projectile throw force (speed)")]
        [SerializeField] private float throwForce;

        [Tooltip("Player input reference")]
        [SerializeField] private PlayerInputs _input;

        private Camera _camera;

        private void Awake()
        {
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
               
                Shoot(SnowballPrefab);
                _input.Shoot = false;

            }
        }

        private void Shoot(GameObject projectile)
        {
            
            //Instantiate a projectile with name clone at camera position
            GameObject Clone = Instantiate(projectile,_camera.transform.position, Quaternion.identity);

          //Set clone rotation and velocity, speed stored as variable throwForce
            Clone.transform.rotation = _camera.transform.rotation;
            Clone.GetComponent<Rigidbody>().AddForce(Clone.transform.forward * throwForce, ForceMode.Impulse);

          //Delete clone after 3 seconds
            Destroy(Clone,3f);
        }
    }

}
