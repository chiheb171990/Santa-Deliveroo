using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Destination
{
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitHouse(int Id)
    {
        id = Id;
        // set the y to the Nav Mesh Amplitude
        destination =new Vector3(transform.position.x,9,transform.position.z);
    }
}
