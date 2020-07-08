using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaMainController : SingletonMB<SantaMainController>
{
    [Header("Objects Variables")]
    public GameObject SantaScrollViewObject;
    [SerializeField] private Transform GiftsContent;
    [SerializeField] private GameObject giftUIPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitSantaScrollView(List<Gift> gifts)
    {
        //Activate the scroll view panel
        SantaScrollViewObject.SetActive(true);

        //Instantiate the giftsUI
        for(int i = 0; i < gifts.Count; i++)
        {
            GameObject giftUI = Instantiate(giftUIPrefab, GiftsContent);

            //Init the gift UI
            giftUI.GetComponent<GiftUI>().InitGiftUI(gifts[i]);
        }
    }

    public void ClearSantaScrollView()
    {
        for(int i = 0; i < GiftsContent.transform.childCount; i++)
        {
            Destroy(GiftsContent.transform.GetChild(i).gameObject);
        }

        //Desactivate the scroll view panel
        SantaScrollViewObject.SetActive(false);
    }
}
