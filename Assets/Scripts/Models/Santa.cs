using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class Santa : MonoBehaviour
{
    public int id;

    public List<Destination> waypoints;
    public List<Gift> collectedGifts;

    //Private variables
    private LineRenderer lineRenderer;
    private NavMeshAgent santaNavAgent;

    // Start is called before the first frame update
    void Start()
    {
        //Caching the components
        lineRenderer = GetComponent<LineRenderer>();
        santaNavAgent = transform.parent.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initSanta(int Id)
    {
        id = Id;
    }

    public void clickDestinationWithShift(Destination dest)
    {
        if (waypoints.Count > 0)
        {
            if (waypoints[waypoints.Count - 1] != dest)
            {
                waypoints.Add(dest);
            }
        }
        else
        {
            waypoints.Add(dest);
        }
        
    }

    public void ClickShiftFinished()
    {
        if (waypoints.Count > 0)
        {
            santaNavAgent.SetDestination(waypoints[0].destination);
        }
    }

    public void clickDestination(Destination dest)
    {
        if(waypoints.Count > 0)
        {
            if (waypoints[waypoints.Count - 1] != dest)
            {
                waypoints.Add(dest);
            }
        }
        else
        {
            waypoints.Add(dest);
        }
        if (!santaNavAgent.hasPath)
        {
            santaNavAgent.SetDestination(waypoints[0].destination);
        }
    }

    public void CollectGift(Gift gift)
    {
        //Add the gift in the selected gifts list
        collectedGifts.Add(gift);

        //Delete the gift from the first destination
        waypoints.RemoveAt(0);

        //Set the next destination if there is waypoints
        if (waypoints.Count > 0)
        {
            santaNavAgent.SetDestination(waypoints[0].destination);
        }

        //slow down a bit the speed
        santaNavAgent.speed -= 0.5f;
    }

    public void DeliverGift(Gift gift)
    {
        //remove the gift from list
        collectedGifts.Remove(gift);

        //Delete the gift from the first destination
        waypoints.RemoveAt(0);

        //Set the next destination if there is waypoints
        if (waypoints.Count > 0)
        {
            santaNavAgent.SetDestination(waypoints[0].destination);
        }

        //speed up a bit the santa speed
        santaNavAgent.speed += 0.5f;
    }

    public void touchWaypoint()
    {
        //Remove the touched destination
        waypoints.RemoveAt(0);

        //Set the next destination if there is waypoints
        if (waypoints.Count > 0)
        {
            santaNavAgent.SetDestination(waypoints[0].destination);
        }
    }
}
