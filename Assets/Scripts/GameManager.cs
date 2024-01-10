using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void StartGame()
    {
        // Load your game scene
        SceneManager.LoadScene(1);
    }

    public void OpenOptionsMenu()
    {
        // Load your options menu scene or show/hide options UI
        // SceneManager.LoadScene("YourOptionsMenuScene");
        // Alternatively, you can enable/disable a canvas with options UI
    }

    public void Credits()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        // Quit the game (works in a standalone build)
        Application.Quit();
    }
}
