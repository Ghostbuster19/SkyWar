using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject PauseMenuUi;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GamePaused)
                PauseGame();
            else ResumeGame();
        }
    }

    void PauseGame()
    {

        Time.timeScale = 0f;
        PauseMenuUi.SetActive(true);
        GamePaused = true;
    }

    public void ResumeGame()
    {
        PauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void LoadMainMenu(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ExitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
