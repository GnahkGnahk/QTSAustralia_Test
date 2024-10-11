using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : Singleton<LoadSceneManager> 
{
    
    public void LoadScene(SceneGame sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
    }

    public void LoadSceneAsync(SceneGame sceneToLoad)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneToLoad));
    }

    private IEnumerator LoadSceneAsyncCoroutine(SceneGame sceneToLoad)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad.ToString());
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
