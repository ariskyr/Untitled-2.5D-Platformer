using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

//DERIVE FROM GENERIC SINGLETON
public class GameManager : GenericSingleton<GameManager>, IDataPersistence
{
    public string currentScene;
    private string test;

    public void LoadData(GameData data)
    {
        test = SceneManager.GetActiveScene().name;
        if (test != "MainMenu")
        {
            currentScene = test;
            Debug.Log("here1" + currentScene);
        }
        else
        {
            currentScene = data.currentScene;
            Debug.Log("here2" + currentScene);
        }
    }

    public void SaveData(GameData data)
    {
        if (test == null)
        {
            data.currentScene = "MAGITIS_DevScene";
            Debug.Log("here3" + data.currentScene);
        }
        else
        {
            data.currentScene = currentScene;
            Debug.Log("here4" + data.currentScene);
        }
    }
}
