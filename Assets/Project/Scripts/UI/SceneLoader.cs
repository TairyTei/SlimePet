using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("UI References")]
    public Image progressBar;
    public TextMeshProUGUI progressText;  
    public GameObject loadingScreen; 

    [Header("Settings")]
    public string sceneNameToLoad;

    private AsyncOperation loadingOperation;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        if (loadingScreen != null) loadingScreen.SetActive(true);
        if (progressBar != null) progressBar.gameObject.SetActive(true);

        if (progressBar != null) progressBar.fillAmount = 0f;
        if (progressText != null) progressText.text = "0%";

        loadingOperation = SceneManager.LoadSceneAsync(sceneNameToLoad);
        loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);

            if (progressBar != null) progressBar.fillAmount = progress;
            if (progressText != null) progressText.text = Mathf.Round(progress * 100f) + "%";

            if (loadingOperation.progress >= 0.9f)
            {
                loadingOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public static void LoadScene(string sceneName)
    {
        PlayerPrefs.SetString("SceneToLoad", sceneName);
        SceneManager.LoadScene("LoadingScene");
    }
}