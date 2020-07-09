using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMB<LevelManager>
{
    public List<Level> allLevels;
    
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
