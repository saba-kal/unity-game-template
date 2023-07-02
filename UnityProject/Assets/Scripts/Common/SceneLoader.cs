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
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private List<SceneCollection> availableScenes;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
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
        if (sceneCollectionIndex < 0 || sceneCollectionIndex >= availableScenes.Count)
        {
            Debug.LogError("Scene index is out of range.");
            return;
        }

        StartCoroutine(LoadMultipleScenesAsync(availableScenes[sceneCollectionIndex]));
    }

    private IEnumerator LoadMultipleScenesAsync(SceneCollection sceneCollection)
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
    }
}

[Serializable]
public class SceneCollection
{
    public string MainSceneName;
    public List<string> AdditiveScenes;
}
