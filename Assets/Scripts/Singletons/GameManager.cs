using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//DERIVE FROM GENERIC SINGLETON
public class GameManager : GenericSingleton<GameManager>, IDataPersistence
{
    [Header("Scene Configuration")]
    [SerializeField] private ScenePlayerConfiguration playerSceneConfig;

    private float elapsedTime = 0;
    private string currentScene;

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

    private void Update()
    {
        Timer();
    }

    //trigger everytime a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;
        Debug.Log("Scene loaded: " + currentScene);

        if (playerSceneConfig == null)
        {
            Debug.LogError("ScenePlayerConfiguration is not assigned in GameManager Inspector!");
            return; // Cannot proceed without configuration
        }

        //get the player instances
        Player player3D = Player.Instance;
        Player2D player2D = Player2D.Instance;

        bool shouldUsePlayer2D = playerSceneConfig.scenesUsingPlayer2D.Contains(scene.name);

        if (shouldUsePlayer2D)
        {
            if (player3D != null && player3D.gameObject.activeSelf)
            {
                Debug.Log("GameManager: Deactivating Player (3D) first.");
                player3D.gameObject.SetActive(false); // Triggers OnDisable on Player
                player3D.playerInput.enabled = false;
            }
        }
        else
        {
            if (player2D != null && player2D.gameObject.activeSelf)
            {
                Debug.Log("GameManager: Deactivating Player2D first.");
                player2D.gameObject.SetActive(false); // Triggers OnDisable on Player2D
                player2D.playerInput.enabled = false;
            }
        }

        // Activate the player that SHOULD be active
        if (shouldUsePlayer2D)
        {
            if (player2D != null && !player2D.gameObject.activeSelf)
            {
                Debug.Log("GameManager: Activating Player2D.");
                player2D.gameObject.SetActive(true); // Triggers OnEnable on Player2D
                player2D.playerInput.enabled = true;
            }
        }
        else
        {
            if (player3D != null && !player3D.gameObject.activeSelf)
            {
                Debug.Log("GameManager: Activating Player (3D).");
                player3D.gameObject.SetActive(true); // Triggers OnEnable on Player
                player3D.playerInput.enabled = true;
            }
        }
    }

    public void LoadData(GameData data)
    {
        elapsedTime = data.timer;
    }

    public void SaveData(GameData data)
    {
        data.timer = elapsedTime;

        //dont save if we are on the main menu
        if (currentScene == "MainMenu") return;
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
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f)
        {
            Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");
            yield return null;
        }
        Debug.Log("Loading nearly complete. Activating Scene: " + sceneName);
        asyncOperation.allowSceneActivation = true;
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        Debug.Log("Scene officially loaded and active: " + sceneName);

        // (Updated teleport logic based on config might be needed here if teleport happens immediately)
        bool teleport2D = playerSceneConfig != null && playerSceneConfig.scenesUsingPlayer2D.Contains(sceneName);
        if (teleport2D)
        {
            Player2D player2DToTeleport = Player2D.Instance;
            if (player2DToTeleport != null && player2DToTeleport.gameObject.activeSelf) { player2DToTeleport.TeleportPlayer(positionToLoad); }
        }
        else
        {
            Player player3DToTeleport = Player.Instance;
            if (player3DToTeleport != null && player3DToTeleport.gameObject.activeSelf) { player3DToTeleport.TeleportPlayer(positionToLoad); }
        }
    }

    private void Timer()
    {
        // the ticking time
        elapsedTime += Time.deltaTime;
    }
}
