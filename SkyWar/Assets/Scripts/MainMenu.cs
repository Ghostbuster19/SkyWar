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

    public void PlayGame()
    {
        StartCoroutine(LoadGameAsync()); 
    }

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

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
