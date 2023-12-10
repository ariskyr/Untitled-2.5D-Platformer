using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsUI : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private ConfirmationPopupMenuUI confirmationPopupMenuUI;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    private SaveSlot[] saveSlots;

    public string currentlyHoveredProfileId;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        DataPersistenceManager.Instance.ChangeSelectedProfileID(saveSlot.GetProfileID());

        if (!saveSlot.hasData)
        {
            //make new game
            DataPersistenceManager.Instance.NewGame();
            //before loading scenes, save game needs to be called
            DataPersistenceManager.Instance.SaveGame();
            //DEFAULT SCENE TO BE LOADED ON NEW GAME
            SceneManager.LoadSceneAsync("MAGITIS_DevScene");
        }
        else
        {
            //before loading scenes, save game needs to be called
            DataPersistenceManager.Instance.SaveGame();
            SceneManager.LoadSceneAsync(GameManager.Instance.CurrentScene);
        }
    }

    public void OnDeleteClicked()
    {
        DisableMenuButtons();
        if (currentlyHoveredProfileId != null)
        {
            confirmationPopupMenuUI.ActivateMenu(
            "Are you sure you want to delete save slot " + currentlyHoveredProfileId + "?",
            //function when yes is clicked
            () =>
            {
                DataPersistenceManager.Instance.DeleteProfileData(currentlyHoveredProfileId);
                ActivateMenu();
            },
            //function when no is clicked
            () =>
            {
                ActivateMenu();
            }
        );
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
        backButton.interactable = true;

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        foreach (SaveSlot saveSlot in saveSlots)
        {
            profilesGameData.TryGetValue(saveSlot.GetProfileID(), out GameData profileData);
            saveSlot.SetData(profileData);
            saveSlot.SetInteractable(true);
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
