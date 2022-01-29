using UnityEngine;

public class PlayerPreferences : MonoBehaviour
{
    private PlayerPreferences _playerPreferences;

    public PlayerPreferences Instance
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
        AudioManager.Instance.GetMixerVariable(variableName, out float number);
        Debug.Log(variableName + " has " + number);
        PlayerPrefs.SetFloat(variableName, number);
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