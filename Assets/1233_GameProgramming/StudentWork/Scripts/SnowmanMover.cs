using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnowmanMover : MonoBehaviour
{
    
    [SerializeField] private NavMeshAgent navMeshAgent;

    private void Update()
    {
        if(PlayerLocatorSingleton.Instance != null && navMeshAgent.enabled)
        {
            navMeshAgent.destination = PlayerLocatorSingleton.Instance.transform.position;
        }
       
        

        

    }
   
    
    

 

}
