using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Befana : MonoBehaviour
{
    public List<Transform> waypoints;

    private NavMeshAgent befanaNavMesh;
    private int index = 0;
    public bool isSantaCatched = false;
    // Start is called before the first frame update
    void Start()
    {
        befanaNavMesh = GetComponent<NavMeshAgent>();
        SetNextDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (befanaNavMesh.remainingDistance < 1f)
        {
            SetNextDestination();
        }
    }

    public void SetNextDestination()
    {
        befanaNavMesh.SetDestination(waypoints[index].position);
        index = index == waypoints.Count - 1 ? 0 : index + 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("enter");
        //check if the trigger is a santa and the gift is selected by the same santa
        if (other.gameObject.tag == "Santa")
        {
            befanaNavMesh.SetDestination(other.gameObject.transform.position);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        //check if the trigger is a santa and the gift is selected by the same santa
        if (other.gameObject.tag == "Santa")
        {
            if(!isSantaCatched)
            {
                befanaNavMesh.SetDestination(other.gameObject.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //check if the trigger is a santa and the gift is selected by the same santa
        if (other.gameObject.tag == "Santa")
        {
            //return to its destinations
            SetNextDestination();
        }
    }
}
