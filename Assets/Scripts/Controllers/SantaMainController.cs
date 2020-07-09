using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SantaMainController : SingletonMB<SantaMainController>
{
    [Header("Santa Objects Variables")]
    public GameObject SantaScrollViewObject;
    [SerializeField] private Transform GiftsContent;
    [SerializeField] private GameObject giftUIPrefab;

    [Header("Game UI elements")]
    [SerializeField] private Text timerText;
    [SerializeField] private Text restGiftsText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Main Menu UI elements")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform levelButtonContent;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = ((int)SantaGameManager.Instance.gameTimer).ToString();
    }
    #region Santa UI region
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
    #endregion
    #region Game UI region
    public void InitUIScene(int giftAmount)
    {
        restGiftsText.text = giftAmount.ToString();
    }

    public void DecrementGiftAmountText(int gift_amount)
    {
        restGiftsText.text = gift_amount.ToString();
    }

    public void WinGame()
    {
        winPanel.SetActive(true);
    }

    public void LoseGame()
    {
        losePanel.SetActive(true);
    }

    public void ClickWinLoseButton()
    {
        //Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
    #region Main Menu UI region
    public void InitLevelPanel()
    {
        //Disable the main menu panel and enable the level panel
        mainMenuPanel.SetActive(false);
        levelsPanel.SetActive(true);

        //Check if there is level saved in player prefs
        if(PlayerPrefs.HasKey("level"))
        {
            int currentLevel = PlayerPrefs.GetInt("level");

            //Init the level buttons
            for(int i = 0; i < currentLevel; i++)
            {
                GameObject levelButton = Instantiate(levelButtonPrefab, levelButtonContent);

                //Init the level button
                levelButton.GetComponent<LevelUI>().initLevelButton(LevelManager.Instance.allLevels[i]);
            }
        }
        
    }
    public void returnToMainMenu()
    {
        levelsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void startGame()
    {
        //check if there is level saved
        if (PlayerPrefs.HasKey("level"))
        {
            //get the last level
            int level = PlayerPrefs.GetInt("level");

            //load the level
            LoadLevelScene(LevelManager.Instance.allLevels[level-1]);
        }
        else
        {
            //set the level to level 1
            PlayerPrefs.SetInt("level", 1);

            //load the level
            LoadLevelScene(LevelManager.Instance.allLevels[0]);
        }
    }

    public void LoadLevelScene(Level level)
    {
        //Init the scene
        print(level.santaNumber);
        SantaGameManager.Instance.initScene(level.santaNumber, level.houseNumber, level.giftNumber, level.giftAmount, level.levelTime);

        //Init UI scene
        InitUIScene(level.giftAmount);

        //Disable the UI panels
        mainMenuPanel.SetActive(false);
        levelsPanel.SetActive(false);
    }
    #endregion
}
