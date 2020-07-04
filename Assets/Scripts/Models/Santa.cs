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
        santaNavAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickDestination(Destination dest)
    {
        waypoints.Add(dest);
        santaNavAgent.SetDestination(waypoints[0].destination);
    }
    
    public void CollectGift(Gift gift)
    {
        //Add the gift in the selected gifts list
        collectedGifts.Add(gift);
    }
}
