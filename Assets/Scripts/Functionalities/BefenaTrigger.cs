using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BefenaTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Santa")
        {
            //santa and befana leave the game
            SantaGameManager.Instance.BefanaCatchSanta(other.gameObject.GetComponent<Santa>());

            //destroy the befana
            transform.parent.gameObject.GetComponent<Befana>().isSantaCatched = true;
            Destroy(this.transform.parent.parent.gameObject);
        }
    }
}
