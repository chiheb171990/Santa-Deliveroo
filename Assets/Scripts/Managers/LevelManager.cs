using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMB<LevelManager>
{
    public List<Level> allLevels;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class Level
{
    public int levelId;
    public string name;
    public int santaNumber;
    public int houseNumber;
    public int giftNumber;
    public int giftAmount;
    public float levelTime;
    
}
