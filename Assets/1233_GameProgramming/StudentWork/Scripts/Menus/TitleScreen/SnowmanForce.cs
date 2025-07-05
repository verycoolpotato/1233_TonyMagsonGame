using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SnowmanForce : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private bool gravity = false;

    public void FlingSnowman()
    {
        Vector3 direction = new Vector3(0,10,10);

        

        rb.useGravity = true;
        
        rb.AddForce(direction * 500,ForceMode.Impulse);
        rb.AddTorque(direction * 5000,ForceMode.Impulse);
        gravity = true;
    }

    private void Update()
    {
        if (gravity)
        {
            rb.AddForce(Vector3.down * 1000,ForceMode.Acceleration);
        }
    }
}
