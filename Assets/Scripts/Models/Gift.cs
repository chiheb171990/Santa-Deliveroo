using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : Destination
{
    public int id;
    public int houseId;
    public int santaId;
    public bool isSelected = false;


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

    private void OnTriggerEnter(Collider other)
    {
        //check if the trigger is a santa and the gift is selected by the same santa
        if (other.gameObject.tag == "Santa" && isSelected == true && other.gameObject.GetComponent<Santa>().id == santaId)
        {
            Santa touchedSanta = other.gameObject.GetComponent<Santa>();
            if (touchedSanta.collectedGifts.Count < 5)
            {
                touchedSanta.CollectGift(this);

                //Check if the scroll view is enabled
                if (SantaMainController.Instance.SantaScrollViewObject.activeInHierarchy && InputManager.Instance.selectedSanta.GetComponent<Santa>().id == other.gameObject.GetComponent<Santa>().id)
                {
                    SantaMainController.Instance.ClearSantaScrollView();
                    SantaMainController.Instance.InitSantaScrollView(touchedSanta.collectedGifts);
                }
                gameObject.SetActive(false);
            }
        }
    }
}
