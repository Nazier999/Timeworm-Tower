using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    [Header("UI Menus")]
    public Canvas pauseMenuCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject settingsMenuCanvas;

    [Space(10)]
    [Header("Check to see what menu the player is in")]
    [SerializeField] private GameObject mainMenuOpen;
    [SerializeField] private GameObject settingsMenuOpen;

    private bool isPaused = false;

    private void Start()
    {
        mainMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(false);
    }

    // Toggles the pause menu based on player input
    public void TogglePauseMenu(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (isPaused && settingsMenuCanvas.activeSelf)
            {
                // If paused and settings menu is active, do nothing
                return;
            }

            if (isPaused)
            {
                // Resume the game
                Time.timeScale = 1f;
                isPaused = false;

                // Close the pause menu canvas
                pauseMenuCanvas.gameObject.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                // Pause the game
                Time.timeScale = 0f;
                isPaused = true;

                // Open the pause menu canvas
                pauseMenuCanvas.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(mainMenuOpen);
            }
        }
    }

    // Opens the settings menu
    private void OpenSettingsMenu()
    {
        settingsMenuCanvas.SetActive(true);
        pauseMenuCanvas.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(settingsMenuOpen);
    }

    // Event handler for the Settings button press
    public void OnSettingsPress()
    {
        OpenSettingsMenu();
    }

    // Event handler for the Resume button press
    public void OnResumePress()
    {
        // Resume the game
        Time.timeScale = 1f;
        isPaused = false;

        // Close the pause menu canvas
        pauseMenuCanvas.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Event handler for the Settings Back button press
    public void OnSettingsBack()
    {
        pauseMenuCanvas.gameObject.SetActive(true);
        settingsMenuCanvas.SetActive(false);
    }
}