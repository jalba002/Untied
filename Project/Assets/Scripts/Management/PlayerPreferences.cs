using UnityEngine;

public class PlayerPrefsLoader : MonoBehaviour
{
    private PlayerPrefsLoader _playerPreferences;

    public PlayerPrefsLoader Instance
    {
        get => _playerPreferences;
        set
        {
            if (_playerPreferences != null)
            {
                Destroy(this);
                return;
            }

            _playerPreferences = value;
        }
    }

    public string[] audioVariables =
    {
        "MasterVolume",
        "SoundsVolume",
        "MusicVolume",
        "AmbienceVolume",
        "CinematicsVolume"
    };

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadAudioPreferences();
    }

    public void SaveAudioPreferences()
    {
        Debug.Log("Saving prefs");
        foreach (var frase in audioVariables)
        {
            StoreFloat(frase);
        }
        PlayerPrefs.Save();
    }

    private void StoreFloat(string variableName)
    {
        AudioManager.Instance.GetMixerVariable(variableName, out float variable);
        PlayerPrefs.SetFloat(variableName, variable);
    }

    private float RecoverFloat(string variableName)
    {
        return PlayerPrefs.GetFloat(variableName);
    }

    public void LoadAudioPreferences()
    {
        Debug.Log("Loading prefs");
        foreach (var frse in audioVariables)
        {
            AudioManager.Instance.SetMixerVariable(frse, RecoverFloat(frse));
        }
    }
}