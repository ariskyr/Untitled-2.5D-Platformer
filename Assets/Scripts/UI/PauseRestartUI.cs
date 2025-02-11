using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseRestartUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button reloadButton;
    [SerializeField] private Button mainMenuButton;

    private bool isPaused = false;

    protected void Awake()
    {
        pauseMenu.SetActive(false);
        reloadButton.onClick.AddListener(OnReloadClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.playerEvents.onPlayerDeath += ShowGameOverUI;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.playerEvents.onPlayerDeath -= ShowGameOverUI;
    }

    private void Update()
    {
        //check if the scene is not the main menu
        Scene scene = SceneManager.GetActiveScene();
        bool mainMenuCheck = scene.name.Equals("MainMenu");
        if (InputManager.Instance.GetPausePressed() && !mainMenuCheck)
        {
            TogglePause();
        }
    }

    public void ShowGameOverUI()
    {
        statusText.text = "GAME OVER";
        ToggleUI(true);
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        statusText.text = isPaused ? "PAUSED" : "";
        ToggleUI(isPaused);
    }

    private void ToggleUI(bool show)
    {
        pauseMenu.SetActive(show);
        Time.timeScale = show ? 0f : 1f;
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }

    public void OnReloadClicked()
    {
        Time.timeScale = 1f;
        var (lastScene, lastPlayerPosition) = DataPersistenceManager.Instance.LoadGame();
        GameManager.Instance.LoadScene(lastScene, lastPlayerPosition);
        ToggleUI(false);
    }

    public void OnMainMenuClicked()
    {
        Debug.Log("Clicked main menu");
        DataPersistenceManager.Instance.SaveGame();
        Time.timeScale = 1f;
        GameManager.Instance.LoadScene("MainMenu", new Vector3(0, (float)0.49, 0));
        ToggleUI(false);
    }

}
