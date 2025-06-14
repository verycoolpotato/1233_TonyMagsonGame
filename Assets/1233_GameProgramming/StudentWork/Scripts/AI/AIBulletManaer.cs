using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBulletManaer : MonoBehaviour
{
    [Tooltip("Primary thrown projectile")]
    [SerializeField] private GameObject SnowballPrefab;

    [Tooltip("Projectile throw force (speed)")]
    [SerializeField] private float throwForce;

    [SerializeField] private Transform ThrowPos;

    private void Update()
    {
        
    }

    private void ProjectileShot(GameObject projectile)
    {
        Vector3 projectileSpawn = ThrowPos.position;

        //Instantiate a projectile with name clone at camera position
        GameObject Clone = Instantiate(projectile, projectileSpawn, Quaternion.identity);

        //Set clone rotation and velocity, speed stored as variable throwForce
        Clone.transform.rotation = transform.rotation;
        Clone.GetComponent<Rigidbody>().AddForce(Clone.transform.forward * throwForce, ForceMode.Impulse);

        //Delete clone after 3 seconds
        Destroy(Clone, 3f);

    }
}
