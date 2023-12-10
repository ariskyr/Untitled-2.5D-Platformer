using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public int buttonPresses;
    public float timer;
    public Vector3 playerPosition;
    public Vector3 cameraPosition;
    public SerializableDictionary<string, string> dialogueVars;
    public SerializableSceneDictionary sceneVars;
    public string lastScene;

    //when NewGame() is called, these are the default values that are used
    public GameData()
    {


        buttonPresses = 0;
        timer = 0;
        playerPosition = new Vector3((float)-0.26, (float)0.59, (float)-1.65);
        cameraPosition = new Vector3(0, 2, (float)-3.5);
        dialogueVars = new SerializableDictionary<string, string>();
        sceneVars = new SerializableSceneDictionary
        {
            {
                "MAGITIS_DevScene",
                new SceneData
                {
                    playerPos = new Vector3((float)-0.26, (float)0.59, (float)-1.65),
                    camPos = new Vector3(0, 2, (float)-3.5)
                }
            }
        };
        lastScene = "MAGITIS_DevScene";
    }
}
