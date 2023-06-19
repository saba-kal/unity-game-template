using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads a main scene with additive scenes.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public List<SceneCollection> AvailableScenes;

    private bool _created = false;

    void Awake()
    {
        // Ensure the script is not deleted while loading.
        if (!_created)
        {
            DontDestroyOnLoad(this.gameObject);
            _created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Loads a single scene based on scene name.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Loads multiple scenes based on the sceneCollectionIndex.
    /// </summary>
    /// <param name="sceneCollectionIndex">The index of the scenes to cycle through.</param>
    public void LoadScene(int sceneCollectionIndex)
    {
        if (sceneCollectionIndex < 0 || sceneCollectionIndex >= AvailableScenes.Count)
        {
            Debug.LogError("Scene index is out of range.");
            return;
        }

        StartCoroutine(LoadMultipleScenesAsync(AvailableScenes[sceneCollectionIndex]));
    }

    IEnumerator LoadMultipleScenesAsync(SceneCollection sceneCollection)
    {
        yield return SceneManager.LoadSceneAsync(sceneCollection.MainSceneName);

        foreach (var additiveScene in sceneCollection.AdditiveScenes)
        {
            var scene = SceneManager.GetSceneByName(additiveScene);
            if (scene == null)
            {
                print("Scene is null");
                continue;
            }

            if (scene.isLoaded)
            {
                print("Scene is loaded");
                yield return SceneManager.UnloadSceneAsync(scene);
                continue;
            }

            yield return SceneManager.LoadSceneAsync(additiveScene, LoadSceneMode.Additive);
        }

        Destroy(this.gameObject);
    }
}

[Serializable]
public class SceneCollection
{
    public string MainSceneName;
    public List<string> AdditiveScenes;
}
