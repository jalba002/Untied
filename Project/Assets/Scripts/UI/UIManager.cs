using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public string sceneName = "UI";
    public bool loadOnStart = false;

    private IEnumerator loadSceneCoroutine;

    private void Start()
    {
        if (loadOnStart) Load();
    }

    [Button("Load UI Scene")]
    public void Load()
    {
        AsyncOperation sceneLoadOperation;
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        else
            SceneManager.UnloadSceneAsync(sceneName);
    }
}
