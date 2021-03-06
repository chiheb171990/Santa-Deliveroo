﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public Level level;
    public Text textButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initLevelButton(Level lvl)
    {
        level = lvl;
        textButton.text = lvl.name;
    }

    public void ClickLevel()
    {
        //Load the level scene
        SantaMainController.Instance.LoadLevelScene(level);
    }
}
