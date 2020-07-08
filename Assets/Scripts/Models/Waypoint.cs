using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : Destination
{
    public int santaId;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitWaypoint(int santa_id)
    {
        santaId = santa_id;
        destination = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if the trigger is a santa and the gift is selected by the same santa
        if (other.gameObject.tag == "Santa" && other.gameObject.GetComponent<Santa>().id == santaId)
        {
            print("shiit2");
            other.gameObject.GetComponent<Santa>().touchWaypoint();
            Destroy(this.gameObject);
        }
    }
}
