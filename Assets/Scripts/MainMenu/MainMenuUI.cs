using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsUI saveSlotsMenu;
    [SerializeField] private ConfirmationPopupMenuUI confirmationPopupMenuUI;

    [Header("Menu Buttons")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button optionsGameButton;
    [SerializeField] private Button exitGameButton;


    public void OnStartGameClicked()
    {
        saveSlotsMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void OnQuitGameClicked()
    {
        DeactivateButtons();
        confirmationPopupMenuUI.ActivateMenu(
            "Are you sure you want to close the game?",
            //function when yes is clicked
            () =>
            {
                //quit the game
                Debug.Log("Game has quitted! (not working in editor)");
                Application.Quit();
            },
            //function when no is clicked
            () =>
            {
                saveSlotsMenu.DeactivateMenu();
                ActivateButtons();
                ActivateMenu();
            }
            );
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DeactivateButtons()
    {
        startGameButton.interactable = false;
        optionsGameButton.interactable = false;
        exitGameButton.interactable = false;
    }

    private void ActivateButtons()
    {
        startGameButton.interactable = true;
        optionsGameButton.interactable = true;
        exitGameButton.interactable = true;
    }
}
