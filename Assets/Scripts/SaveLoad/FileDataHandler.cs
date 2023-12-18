using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFilename = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "magas";
    private readonly string backupExtension = ".bak";

    public FileDataHandler(string dataDirPath, string dataFilename, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFilename = dataFilename;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string profileID, bool allowRestoreFromBackup = true)
    {
        string fullPath = Path.Combine(dataDirPath, profileID, dataFilename);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        //try to roll again recursively (false to stop possible infinite recursion loop)
                        loadedData = Load(profileID, false);
                    }
                }
                else
                {
                    Debug.LogError("Error occured when trying to load file at path: "
                        + fullPath + " and backup did not work.\n" + e);
                }

            }
        }
        return loadedData;
    }

    public void Save(GameData data, string profileID)
    {
        //base case -> if profiled Id return
        if (profileID == null)
        {
            Debug.LogError("profileId was null in data Handler");
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileID, dataFilename);
        string backupFilePath = fullPath + backupExtension;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            //verify the newly saved file can be loaded successfully
            GameData verifiedGameData = Load(profileID);
            //if data is verified, back it up
            if (verifiedGameData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }
            else
            {
                throw new Exception("Save filed could not be verified and backup could not be created");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public void Delete(string profileID) 
    { 
        if (profileID == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileID, dataFilename);
        try
        {
            if (File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete profile data for profileID: " + profileID + " at path: " + fullPath + "\n" + e);
        }
    }

    // XOR operation using the encryption word
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        //loop over the dir where the slots exist
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileID = dirInfo.Name;

            //skip the Unity folder since it will always be there
            if(profileID == "Unity")
            {
                continue;
            }

            //exclude the junk that doesn't include a data json object
            string fullPath = Path.Combine(dataDirPath, profileID, dataFilename);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading profiles because it doesn't contain game data: " + profileID);
                continue;
            }

            //load
            GameData profileData = Load(profileID);
            if (profileData != null)
            {
                profileDictionary.Add(profileID, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but data was null. ProfileID: " + profileID);
            }
        }

        return profileDictionary;
    }

    private bool AttemptRollback(string fullPath)
    {
        bool success = false;
        string backupFilePath = fullPath + backupExtension;
        try
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
            }
            else
            {
                throw new Exception("Tried to roll back, but no backup file exists to roll back to.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured wwhen trying to roll back to backup file at: "
                + backupFilePath + "\n" + e);
        }

        return success;
    }
}
