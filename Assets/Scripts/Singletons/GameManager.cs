using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//DERIVE FROM GENERIC SINGLETON
public class GameManager : GenericSingleton<GameManager>, IDataPersistence
{
    private float elapsedTime = 0;
    private string currentScene;
    private Player player;

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

    private void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player = playerGO.GetComponent<Player>();
        }
        else
        {
            Debug.LogError("Player not found, teleporting him to appropriate levels will not work!");
        }
    }

    private void Update()
    {
        Timer();
    }

    //trigger everytime a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;
        Debug.Log("Scene loaded: " + currentScene);
    }

    public void LoadData(GameData data)
    {
        elapsedTime = data.timer;
    }

    public void SaveData(GameData data)
    {
        data.timer = elapsedTime;
        data.lastScene = currentScene;
    }

    public void LoadScene(string sceneName, Vector3 positionToLoad)
    {
        //we need to save game to file FIRST, before loading any scene
        DataPersistenceManager.Instance.SaveGame();
        //start the scene loading
        StartCoroutine(LoadSceneCoroutine(sceneName, positionToLoad));
    }

    public IEnumerator LoadSceneCoroutine(string sceneName, Vector3 positionToLoad)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");
            asyncOperation.allowSceneActivation = true;

            yield return null;
        }
        player.TeleportPlayer(positionToLoad);
        // Enable the next scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    private void Timer()
    {
        // the ticking time
        elapsedTime += Time.deltaTime;
    }
}
