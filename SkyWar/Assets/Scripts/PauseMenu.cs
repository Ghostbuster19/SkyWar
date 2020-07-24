using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// In game pause menu.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject PauseMenuUi;
    public AudioSource Audio;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GamePaused)
                PauseGame();
            else ResumeGame();
        }
    }

    // Method to pause the game.
    void PauseGame()
    {

        Time.timeScale = 0f;
        PauseMenuUi.SetActive(true);
        GamePaused = true;
    }

    // Method to resume the game.
    public void ResumeGame()
    {
        PauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    // Method for leaving the game to the main menu.
    public void LoadMainMenu(int index)
    {
        SceneManager.LoadScene(index);
    }

    // Method for muting the Audio.
    public void MuteAudio()
    {
        Audio.mute = !Audio.mute;
    }

    // Exit the game. Does not work in the editor.
    public void ExitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
