using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad = "UI_MM";

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
