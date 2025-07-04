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
    public float minLoadTime = 0.5f;

    private AsyncOperation loadingOperation;
    private float loadStartTime;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        loadStartTime = Time.time;
        
        if (loadingScreen != null) 
            loadingScreen.SetActive(true);
        
        if (progressBar != null) 
        {
            progressBar.gameObject.SetActive(true);
            progressBar.fillAmount = 0f;
        }
        
        if (progressText != null) 
            progressText.text = "0%";

        loadingOperation = SceneManager.LoadSceneAsync(sceneNameToLoad);
        loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone)
        {
            float elapsedTime = Time.time - loadStartTime;
            float artificialProgress = Mathf.Clamp01(elapsedTime / minLoadTime);
            
            float realProgress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            
            float displayProgress = Mathf.Min(artificialProgress, realProgress);

            if (progressBar != null) 
                progressBar.fillAmount = displayProgress;
            
            if (progressText != null) 
                progressText.text = Mathf.Round(displayProgress * 100f) + "%";

            if (loadingOperation.progress >= 0.9f && elapsedTime >= minLoadTime)
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