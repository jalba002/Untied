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

    public static AsyncOperation LoadScene(string sName)
    {
        return SceneManager.LoadSceneAsync(sName, LoadSceneMode.Additive);
    }

    public static void UnloadScene(string sName)
    {
        SceneManager.UnloadSceneAsync(sName);
    }

    public static bool IsSceneLoaded(string sName)
    {
        return SceneManager.GetSceneByName(sName).isLoaded;
    }

    public static Scene GetScene(string sName)
    {
        return SceneManager.GetSceneByName(sName);
    }
}
