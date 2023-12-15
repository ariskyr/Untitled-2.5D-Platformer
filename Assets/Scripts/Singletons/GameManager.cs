using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//DERIVE FROM GENERIC SINGLETON
public class GameManager : GenericSingleton<GameManager>, IDataPersistence
{
    private float elapsedTime = 0;
    private string currentScene;
    private PlayerMovement playerMovement;

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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        // Wait until the next scene is loaded
        while (!asyncLoad.isDone)
        {
            Debug.Log($"Loading progress: {asyncLoad.progress}, isDone: {asyncLoad.isDone}");
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        playerMovement.TeleportPlayer(positionToLoad);
        asyncLoad.allowSceneActivation = true;
        // Enable the next scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    private void Timer()
    {
        // the ticking time
        elapsedTime += Time.deltaTime;

        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(elapsedTime);

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
}
