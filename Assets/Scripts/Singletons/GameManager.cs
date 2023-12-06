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
        }
        else
        {
            currentScene = data.currentScene;
        }
    }

    public void SaveData(GameData data)
    {
        if (test == null)
        {
            data.currentScene = "MAGITIS_DevScene";
        }
        else
        {
            data.currentScene = currentScene;
        }
    }
}
