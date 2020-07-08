using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftUI : MonoBehaviour
{
    public Gift gift;

    [Header("UI elements")]
    [SerializeField] private Text giftIdText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGiftUI(Gift selectedGift)
    {
        gift = selectedGift;
        giftIdText.text = gift.id.ToString();
    }

    public void selectGift()
    {
        //Deselect gifthouse associations
        SantaGameManager.Instance.DeselectGiftsAndHousesAssociations();

        //when we click on the giftUI button the house associated will be highlighted
        SantaGameManager.Instance.selectGift(gift.houseId);
    }
}
