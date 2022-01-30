using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string sceneToLoad = "01";

    public RectTransform MainParent;
    public RectTransform ControlsParent;
    public RectTransform OptionsParent;
    public RectTransform CreditsParent;

    private InputSystemUIInputModule InputSystemUI;

    private void Start()
    {
        ReturnToMain();
        InputSystemUI = FindObjectOfType<InputSystemUIInputModule>();
        InputSystemUI.cancel.action.performed += HandleReturn;
    }

    public void ToggleOptions()
    {
        MainParent.gameObject.SetActive(OptionsParent.gameObject.activeInHierarchy);
        OptionsParent.gameObject.SetActive(!OptionsParent.gameObject.activeInHierarchy);
    }

    public void ToggleControls()
    {
        MainParent.gameObject.SetActive(ControlsParent.gameObject.activeInHierarchy);
        ControlsParent.gameObject.SetActive(!ControlsParent.gameObject.activeInHierarchy);
    }

    public void ToggleCredits()
    {
        MainParent.gameObject.SetActive(CreditsParent.gameObject.activeInHierarchy);
        CreditsParent.gameObject.SetActive(!CreditsParent.gameObject.activeInHierarchy);
    }

    private void HandleReturn(InputAction.CallbackContext context)
    {
        ReturnToMain();
    }

    public void ReturnToMain()
    {
        if(MainParent != null)
            MainParent.gameObject.SetActive(true);
        if(OptionsParent != null)
            OptionsParent.gameObject.SetActive(false);
        if(ControlsParent != null)
            ControlsParent.gameObject.SetActive(false);
        if(CreditsParent != null)
            CreditsParent.gameObject.SetActive(false);
    }

    public void Resume()
    {
        gameObject.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ExitApplication()
    {
        if (Application.isPlaying)
        {
            Application.Quit();
        }
    }

    public string twitterNameParameter = "Check this amazing game made by @andrew_raya @JordiAlbaDev @ssegarra3D @Sergisggs GerardTorras for the #GlobalGameJam @globalgamejam! Here the link: ";
    public string twitterDescriptionParam = "";
    private const string twitterAdress = "https://twitter.com/intent/tweet";
    public string globalGameJamLink = "https://globalgamejam.org/2022/games/untied-8";

    public void PressToShareTwitter()
    {
        Application.OpenURL(twitterAdress + "?text=" + WWW.EscapeURL(twitterNameParameter + "\n" +
            twitterDescriptionParam + "\n" + globalGameJamLink));
    }
}
