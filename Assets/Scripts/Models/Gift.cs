using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : Destination
{
    // Start is called before the first frame update
    public int id;
    public int houseId;
    public int santaId;
    public bool isSelected = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGift(int Id,int HouseId)
    {
        id = Id;
        houseId = HouseId;
        destination = transform.position;
    }

    public void SelectGift(int santa_id)
    {
        isSelected = true;
        santaId = santa_id;
    }

    public void OnTriggerEnter(Collider other)
    {
        //check if the trigger is a santa and the gift is selected by the same santa
        if (other.gameObject.tag == "Santa" && isSelected == true && other.gameObject.GetComponent<Santa>().id == santaId)
        {
            other.gameObject.GetComponent<Santa>().CollectGift(this);
            gameObject.SetActive(false);
        }
    }
}
