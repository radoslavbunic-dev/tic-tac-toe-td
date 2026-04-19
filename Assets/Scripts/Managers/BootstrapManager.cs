using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    public static Action OnInitialized;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(LoadPlayScene());
        
    }

    IEnumerator LoadPlayScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("PlayScene", LoadSceneMode.Single);
        yield return loadOperation;
        OnInitialized?.Invoke();
        Destroy(gameObject);
    }
}
