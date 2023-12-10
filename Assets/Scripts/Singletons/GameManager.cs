using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//DERIVE FROM GENERIC SINGLETON
public class GameManager : GenericSingleton<GameManager>, IDataPersistence
{
    public string CurrentScene { get; private set; }
    public string LastScene { get; private set; }

    private void OnEnable()
    {
        //subscribe to sceneloaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        //unsubscribe
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CurrentScene = scene.name;
        Debug.Log("Scene loaded: " + CurrentScene);
    }

    public void LoadScene(string sceneName)
    {
        //before loading scenes, save game needs to be called
        DataPersistenceManager.Instance.SaveGame();
        //DEFAULT SCENE TO BE LOADED ON NEW GAME
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadData(GameData data)
    {
        Debug.Log("loaded");
    }

    public void SaveData(GameData data)
    {
        Debug.Log("saved");
    }

    //public void LoadData(GameData data)
    //{
    //    if (test != "MainMenu")
    //    {
    //        currentScene = test;
    //        Debug.Log("here1" + currentScene);
    //    }
    //    else
    //    {
    //        currentScene = data.currentScene;
    //        Debug.Log("here2" + currentScene);
    //    }
    //}

    //public void SaveData(GameData data)
    //{
    //    if (test == null)
    //    {
    //        data.currentScene = "MAGITIS_DevScene";
    //        Debug.Log("here3" + data.currentScene);
    //    }
    //    else
    //    {
    //        data.currentScene = currentScene;
    //        Debug.Log("here4" + data.currentScene);
    //    }
    //}
}
