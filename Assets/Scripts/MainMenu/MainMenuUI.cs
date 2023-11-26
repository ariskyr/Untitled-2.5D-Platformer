using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsUI saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button optionsGameButton;
    [SerializeField] private Button exitGameButton;


    public void OnStartGameClicked()
    {
        saveSlotsMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
