using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject LoadingScene;
    public TMP_Text LoadingText;
    public Slider ProgressSlider;
    public RawImage ProgressImage;
    public float RotateSpeed;

    // Method for the play button in the main menu.
    // Loads the game asynchronously.
    public void PlayGame()
    {
        StartCoroutine(LoadGameAsync()); 
    }

    // Method for loading the game asynchronously.
    IEnumerator LoadGameAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        LoadingScene.SetActive(true); 

        while (!operation.isDone)
        {
            float realProgress = Mathf.Clamp01(operation.progress);
            ProgressSlider.value = realProgress;
            LoadingText.text = realProgress * 100f + "%";
            ProgressImage.rectTransform.Rotate(0f, 0f, RotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Mehtod for quitting the game. Does not work in the editor.
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
