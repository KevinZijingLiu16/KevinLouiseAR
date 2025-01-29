using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private string sceneNameToBeLoaded;
    public void LoadScene(string sceneName)
    {
        sceneNameToBeLoaded = sceneName;

        StartCoroutine(InitializeSceneLoading());

    }

    IEnumerator InitializeSceneLoading()
    {
        // every time load the scene, we need to wait for the end of the frame, which is the loading scene.

        yield return SceneManager.LoadSceneAsync("Scene_Loading");

        // after the loading scene is loaded, we can start loading the scene we want to load.

        StartCoroutine(LoadActualScene());
    }

    IEnumerator LoadActualScene()
    {
        var asyncSceneLoading = SceneManager.LoadSceneAsync(sceneNameToBeLoaded);
        // this value stops the scene from displaying until it is fully loaded.
        asyncSceneLoading.allowSceneActivation = false;

        while (!asyncSceneLoading.isDone)
        {
            // the progress of the loading scene is between 0 and 0.9f
            // we can use this value to display a loading bar.
            // Debug.Log(asyncSceneLoading.progress);
            // if the loading scene is fully loaded, we can activate the scene.
            if (asyncSceneLoading.progress >= 0.9f)
            {
                asyncSceneLoading.allowSceneActivation = true;
            }
            yield return null;
        }

    }

}
