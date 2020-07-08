using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class House : Destination
{
    public int id;
    public List<int> santasId;
    public List<Gift> associatedGifts;
    public bool isSelected;

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

    public void SelectHouse(int santa_id)
    {
        isSelected = true;
        //There is possibilities that there is more then one santa select a house
        santasId.Add(santa_id);
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if the trigger is a santa and the house is selected by the same santa
        if (other.gameObject.tag == "Santa" && isSelected == true && santasId.Contains(other.gameObject.GetComponent<Santa>().id))
        {
            //check if the santa have the associated gift
            Santa touchedSanta = other.gameObject.GetComponent<Santa>();
            int i = 0;
            while(i < associatedGifts.Count)
            {
                if (touchedSanta.collectedGifts.Contains(associatedGifts[i]))
                {
                    //deliver the gift
                    SantaGameManager.Instance.DeliveredGift(associatedGifts[i]);
                    
                    //Remove the gift from the santa
                    touchedSanta.DeliverGift(associatedGifts[i]);

                    //Remove the gift from the santa gift UI list if the scroll view is enabled and if the active scroll view content is corresponding to the selected santa
                    if (SantaMainController.Instance.SantaScrollViewObject.activeInHierarchy && InputManager.Instance.selectedSanta.GetComponent<Santa>().id == touchedSanta.id)
                    {
                        //Refresh the scroll view
                        SantaMainController.Instance.ClearSantaScrollView();
                        SantaMainController.Instance.InitSantaScrollView(touchedSanta.collectedGifts);
                    }

                    //Remove the gift from house
                    associatedGifts.Remove(associatedGifts[i]);

                    //remove the santa id
                    santasId.Remove(touchedSanta.id);
                }
                else
                {
                    i++;
                }
            }

            //Check if all gifts are delivred
            if (associatedGifts.Count == 0)
            {
                //Dishighlight the house
                GetComponent<Outline>().enabled = false;

                //Set the house non clickable
                gameObject.layer = 0;
            }
        }
    }
}
