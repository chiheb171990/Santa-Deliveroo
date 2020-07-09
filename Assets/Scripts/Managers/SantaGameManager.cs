using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class SantaGameManager : SingletonMB<SantaGameManager>
{
    [Header("Game Initiation Parameters")]
    public List<House> houses;
    public List<Transform> instantiationTransforms;
    [SerializeField] private GameObject santaObject;
    [SerializeField] private Transform santaParent;
    [SerializeField] private GameObject giftObject;
    [SerializeField] private Transform giftParent;
    [SerializeField] private GameObject waypointObject;
    [SerializeField] private Transform waypointParent;

    [Header("Game variables")]
    public List<House> choosedHouses;
    public List<Gift> gifts;
    public int amountGift;
    public float gameTimer;

    //private variables
    private House associationHouse;
    private List<Gift> associationGifts;
    private bool isGameBegin = false;
    private bool isGameWin = false;

    // Start is called before the first frame update
    void Start()
    {
        //initScene(5, 10, 20,1,100);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameBegin)
        {
            if (gameTimer >=0 && !isGameWin)
            {
                gameTimer -= Time.deltaTime;
            }
            else if(gameTimer<=0)
            {
                GameOver();
                print("game over");
            }
        }
    }

    public void initScene(int santaNumber,int houseNumber,int giftNumber,int amountGiftNumber, float gameTime)
    {
        //Init the game parameters
        amountGift = amountGiftNumber;
        gameTimer = gameTime;

        //Init the santas
        for(int i = 0; i < santaNumber; i++)
        {
            int transformInd = Random.Range(0, instantiationTransforms.Count);
            GameObject santaObj = Instantiate(santaObject, instantiationTransforms[transformInd].position, instantiationTransforms[transformInd].rotation, santaParent);
            
            //Init the santa component with its id
            santaObject.transform.GetChild(0).GetComponent<Santa>().initSanta(i);

            //Delete the transform used from the list to not have other objects in the same transform
            instantiationTransforms.RemoveAt(transformInd);
        }

        //Init Houses
        for (int i = 0; i < houseNumber; i++)
        {
            //Choose a random house and add it to the choosed houses list
            int houseInd = Random.Range(0, houses.Count);
            House choosedHouse = houses[houseInd];
            choosedHouses.Add(choosedHouse);

            //Set the layer of the choosed house to clickable layer
            choosedHouse.gameObject.layer = 9;

            //Init the house with its id
            choosedHouse.InitHouse(i);

            //Delete the selected house from allHouses
            houses.RemoveAt(houseInd);

            //Highlight the choosed house
            choosedHouse.GetComponent<Outline>().enabled = true;
        }

        //Init the gifts
        int giftHouseId = 0;
        for (int i = 0; i < giftNumber; i++)
        {
            int transformInd = Random.Range(0, instantiationTransforms.Count);
            GameObject giftObj = Instantiate(giftObject, instantiationTransforms[transformInd].position, instantiationTransforms[transformInd].rotation, giftParent);

            //Adjust the gift object rotation
            giftObj.transform.eulerAngles += new Vector3(-90, 0, 0);

            //Delete the transform used from the list to not have other objects in the same transform
            instantiationTransforms.RemoveAt(transformInd);

            //Init the gift component
            giftObj.GetComponent<Gift>().InitGift(i,giftHouseId);

            //Add the gift to the gifts list
            gifts.Add(giftObj.GetComponent<Gift>());

            //Add the gift to the list of the associatedGifts of the house
            choosedHouses[giftHouseId].associatedGifts.Add(giftObj.GetComponent<Gift>());

            //Increment the giftHouseId : if the id exceed the the selected houses count it return to 0 to check that all houses have minimum gift
            giftHouseId = (giftHouseId < houseNumber-1) ? giftHouseId+1 : 0;

        }

        //Beging the timer 
        isGameBegin = true;
    }

    public void selectGift(int houseId)
    {
        //Highlight the house associating to its gift
        associationHouse = choosedHouses.Find(p => p.id == houseId);
        //Highlight the house if it's not null
        if (associationHouse != null)
        {
            associationHouse.GetComponent<Outline>().color = 1;
        }
    }

    public void selectHouse(int houseId)
    {
        
        associationGifts = gifts.FindAll(p => p.houseId == houseId);
        //Highlight the gifts if they exists
        if (associationGifts.Count > 0)
        {
            for (int i = 0; i < associationGifts.Count; i++)
            {
                associationGifts[i].GetComponent<Outline>().enabled = true;
            }
        }
    }

    public void DeselectGiftsAndHousesAssociations()
    {
        //Dishighlight the associated house if it's not null
        if (associationHouse != null)
        {
            associationHouse.GetComponent<Outline>().color = 2;
        }
        //set the associationHouse to null
        associationHouse = null;

        if (associationGifts!=null)
        {
            for (int i = 0; i < associationGifts.Count; i++)
            {
                associationGifts[i].GetComponent<Outline>().enabled = false;
            }
            //Clear the associationGifts list
            associationGifts.Clear();
        }
    }

    public void DeliveredGift(Gift gift)
    {
        amountGift--;
        gifts.Remove(gift);

        //Update amount gift UI
        SantaMainController.Instance.DecrementGiftAmountText(amountGift);

        //Check if amount gift is done
        if(amountGift == 0)
        {
            //Win the game
            isGameWin = true;
            WinGame();
        }
    }

    public GameObject InstantiateWaypoint(Vector3 pos,int santa_id)
    {
        GameObject waypointObj = Instantiate(waypointObject, pos, Quaternion.identity, waypointParent);
        waypointObj.GetComponent<Waypoint>().InitWaypoint(santa_id);

        return waypointObj;
    }

    public void GameOver()
    {
        //Enable gameOver Panel
        SantaMainController.Instance.LoseGame();
    }

    public void WinGame()
    {
        //enable win panel
        SantaMainController.Instance.WinGame();

        //Save the level and pass to next level
        int lev = PlayerPrefs.GetInt("level");
        lev++;
        PlayerPrefs.SetInt("level",lev);
    }
}
