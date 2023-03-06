using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;

    private SceneLoader()
    {}

    public static SceneLoader getInstance()
    {
        if (instance == null) instance = new SceneLoader();
        return instance;
    }

    [SerializeField] private GameObject _loadingCanvas;
    
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    
    public void LoadSceneAsync(string name)
    {
        StartCoroutine(LoadingSceneAsync(name));
        Instantiate(_loadingCanvas);
    }
    
    IEnumerator LoadingSceneAsync(string name)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(name);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            Debug.Log(op.progress);

            yield return null;
        }
    }
    
}
