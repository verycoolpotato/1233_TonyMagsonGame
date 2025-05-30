using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudentWork
{ 
    public class ShootingManager : MonoBehaviour
    {
        [SerializeField] private GameObject SnowballPrefab;
        [SerializeField] private PlayerInputs _input;

        [SerializeField] private float throwForce;

        [SerializeField] private Camera _camera;


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
