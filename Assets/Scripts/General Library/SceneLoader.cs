using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string LoadingSceneName = "LoadingScene";
    [SerializeField] private float delayBetweenLoading = 3;
    private Coroutine loadingLoader = null;

    public UnityEvent OnSceneChanging;


    public void Load(int index)
    {
        OnSceneChanging.Invoke();
        SceneManager.LoadScene(index);
    }
    public void Load(string sceneName)
    {
        OnSceneChanging.Invoke();
        SceneManager.LoadScene(sceneName);
    }
    public void ReloadScene()
    {
        OnSceneChanging.Invoke();
        int sceneNo = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneNo);
    }

    public void LoadNextScene()
    {
        OnSceneChanging.Invoke();
        int sceneNo = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(++sceneNo);
    }
    public void LoadNextWithLoop()
    {
        OnSceneChanging.Invoke();
        int sceneNo = SceneManager.GetActiveScene().buildIndex;
        ++sceneNo;

        if (sceneNo >= SceneManager.sceneCountInBuildSettings)
            sceneNo = 0;
        SceneManager.LoadScene(sceneNo);
    }

    #region AdditiveLoader
#pragma warning disable 0618
    public void LoadWithLoading(string SceneName)
    {
        StopLoader();
        loadingLoader = StartCoroutine(LoadingTo(LoadingSceneName, SceneName));


        IEnumerator LoadingTo(string loadingSceneName, string nextSceneName)
        {
            float loadingTime = delayBetweenLoading;
            //load loading scene additivly
            SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
            int index = SceneManager.GetAllScenes().Length - 1;
            Scene loadingScene = SceneManager.GetSceneAt(index);

            while (!loadingScene.isLoaded)//wait for ready to load our "Loading" Scene
            {
                yield return null;
            }

            DontDestroyOnLoad(gameObject);

            //set Loading scene as a root scene in the hierarchy
            Scene previousScene = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(loadingScene);
            SceneManager.UnloadScene(previousScene);


            //Make Loading Delay to load next scene
            loadingTime += Time.time;
            while (Time.time < loadingTime)
            {
                yield return null;
            }

            //load Target Scene Additivly
            SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
            index = SceneManager.GetAllScenes().Length - 1;
            Scene nextScene = SceneManager.GetSceneAt(index);


            while (!nextScene.isLoaded)//wait for ready to load our Target Scene
            {
                yield return null;
            }
            OnSceneChanging.Invoke();
            SceneManager.SetActiveScene(nextScene);
            SceneManager.UnloadScene(loadingScene);

            Destroy(gameObject);

        }
    }

    private void StopLoader()
    {
        if (loadingLoader != null)
            StopCoroutine(loadingLoader);
    }
#pragma warning restore
    #endregion AdditiveLoader
}