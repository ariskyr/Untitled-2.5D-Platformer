using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : GenericSingleton<DataPersistenceManager>
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("No data was found. Initializing to defaults.");
            NewGame();
        }

        //push the loaded data to all scripts that need it
        foreach (IDataPersistence dataObj in dataPersistenceObjects) 
        {
            dataObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        //pass data to scripts to update the gamedata
        foreach (IDataPersistence dataObj in dataPersistenceObjects)
        {
            dataObj.SaveData(gameData);
        }

        dataHandler.Save(gameData);
    }

    //remove in the future
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
