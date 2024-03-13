using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float timer;
    public Vector3 playerPosition;
    public int playerExperience;
    public int playerLevel;
    public int playerHealth;
    public int playerGold;
    public Vector3 cameraPosition;
    public SerializableDictionary<string, string> dialogueVars;
    public SerializableDictionary<string, QuestData> questData;
    public string lastScene;

    //when NewGame() is called, these are the default values that are used
    public GameData()
    {
        timer = 0;
        playerPosition = new Vector3((float)-0.26, (float)0.59, (float)-1.65);
        playerExperience = 0;
        playerLevel = 1;
        playerHealth = 100;
        playerGold = 0;
        cameraPosition = new Vector3(0, 2, (float)-3.5);
        dialogueVars = new SerializableDictionary<string, string>();
        questData = new SerializableDictionary<string, QuestData>(); 
        lastScene = "MAGITIS_DevScene";
    }
}
