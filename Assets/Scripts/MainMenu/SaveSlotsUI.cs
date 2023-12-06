using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsUI : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenuUI mainMenuUI;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    private SaveSlot[] saveSlots;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        DataPersistenceManager.Instance.ChangeSelectedProfileID(saveSlot.GetProfileID());

        if (!saveSlot.GetDataExistence())
        {
            //make new game
            DataPersistenceManager.Instance.NewGame();
        }
        //before loading scenes, save game needs to be called
        DataPersistenceManager.Instance.SaveGame();
        if (!saveSlot.GetDataExistence())
        {
            //DEFAULT SCENE TO BE LOADED ON NEW GAME
            SceneManager.LoadSceneAsync("MAGITIS_DevScene");
        }
        else
        {
            SceneManager.LoadSceneAsync(GameManager.Instance.currentScene);
        }
    }

    public void OnBackClicked()
    {
        mainMenuUI.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        foreach (SaveSlot saveSlot in saveSlots)
        {
            profilesGameData.TryGetValue(saveSlot.GetProfileID(), out GameData profileData);
            saveSlot.SetData(profileData);
        }
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    //I do this to prevent a double click on the buttons which could lead in multiple game Data being created
    private void DisableMenuButtons()
    {
        foreach(SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
