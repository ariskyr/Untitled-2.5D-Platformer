using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : GenericSingleton<DataPersistenceManager>
{
    //TODO remove
    [Header("DEV Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool disableAutoSave = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    [Header("Auto Saving config")]
    [SerializeField] private float autoSaveTimeSeconds = 60f;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private string selectedProfileID = "";
    private Coroutine autoSaveCoroutine;

    protected override void Awake()
    {
        base.Awake();

        if (disableDataPersistence )
        {
            Debug.LogWarning("DEV: Data Persistence is disabled.");
        }

        //persistent data path is under user appdata
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        //return if on main menu
        if (scene.name == "MainMenu") return;

        LoadGame();

        //DEV
        if (disableAutoSave) return;

        //start up the auto saving coroutine
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }
        autoSaveCoroutine = StartCoroutine(AutoSave());
    }

    public void ChangeSelectedProfileID(string newProfileID)
    {
        //update selected slot
        this.selectedProfileID = newProfileID;
        //Load game using the newly selected profile
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        //DEBUG
        if (disableDataPersistence) return;

        gameData = dataHandler.Load(selectedProfileID);

        //dev debug
        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        if (gameData == null)
        {
            Debug.Log("No data was found. A new game needs to be started before data can be loaded.");
            return;
        }

        //push the loaded data to all scripts that need it
        foreach (IDataPersistence dataObj in dataPersistenceObjects) 
        {
            dataObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        //DEBUG
        if (disableDataPersistence) return;

        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A new game needs to be started before data can be saved.");
            return;
        }
        
        //pass data to scripts to update the gamedata
        foreach (IDataPersistence dataObj in dataPersistenceObjects)
        {
            dataObj.SaveData(gameData);
        }

        dataHandler.Save(gameData, selectedProfileID);
    }

    public void DeleteProfileData(string profileID)
    {
        dataHandler.Delete(profileID);
    }

    //remove in the future
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds);
            SaveGame();
            Debug.Log("Auto Saved Game!");
        }
    }
}
