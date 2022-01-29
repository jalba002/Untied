using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioManager;

    public static AudioManager Instance
    {
        get => _audioManager;

        set
        {
            if (_audioManager != null)
            {
                Destroy(value);
                return;
            }

            _audioManager = value;
        }
    }

    public AudioMixer Mixer;

    [Header("Audio playing settings")]
    public AudioMixerGroup audioOutputGroup;
    private IEnumerator soundRemover;

    [System.Serializable]
    public struct TemporalAudio
    {
        public GameObject go;
        public AudioSource Source;
        public float destructionTime;

        public TemporalAudio(GameObject position, AudioSource source, float duration)
        {
            this.go = position;
            this.Source = source;
            this.destructionTime = Time.timeSinceLevelLoad + duration;
        }
    }

    public List<TemporalAudio> audios = new List<TemporalAudio>();

    [Header("AudioSources")]

    public AudioSource musicAudioSource;
    private IEnumerator changeMusicCoroutine;

    private void Awake()
    {
        Instance = this;
        //this.transform.parent = null;
        //DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        //if (soundRemover == null) return;
        soundRemover = checkAudios();
        StartCoroutine(soundRemover);
    }

    public void SetMixerVariable(string varName, float value)
    {
        Mixer.SetFloat(varName, value);
    }

    public void GetMixerVariable(string varName, out float value)
    {
        Mixer.GetFloat(varName, out value);
    }

    public void PlaySoundAt(Vector3 pos, AudioClip clip)
    {
        if (clip == null) return;

        var item = new GameObject("[TempAudio]");
        item.transform.position = pos;
        var casio = item.AddComponent<AudioSource>();
        casio.clip = clip;
        casio.outputAudioMixerGroup = audioOutputGroup;
        casio.spatialBlend = 1f;
        casio.Play();
        TemporalAudio temp = new TemporalAudio(item, casio, clip.length);
        audios.Add(temp);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        var item = new GameObject("[TempAudio]");
        var casio = item.AddComponent<AudioSource>();
        casio.clip = clip;
        casio.outputAudioMixerGroup = audioOutputGroup;
        casio.spatialBlend = 0f;
        casio.Play();
        TemporalAudio temp = new TemporalAudio(item, casio, clip.length);
        audios.Add(temp);
    }

    public void PlayMusic(AudioClip music)
    {
        if (changeMusicCoroutine != null) return;
        // Queue System?
        changeMusicCoroutine = ChangeMusicFade(musicAudioSource, music, 2f);
        StartCoroutine(changeMusicCoroutine);
    }

    [Button("Play Music Clip")]
    public void PlayMusic(AudioClip music, float timeToFade)
    {
        if (changeMusicCoroutine != null) return;

        changeMusicCoroutine = ChangeMusicFade(musicAudioSource, music, timeToFade);
        StartCoroutine(changeMusicCoroutine);
    }

    IEnumerator checkAudios()
    {
        while (enabled)
        {
            List<TemporalAudio> audiosToRemove = new List<TemporalAudio>();
            foreach (var item in audios)
            {
                if (item.destructionTime <= Time.timeSinceLevelLoad)
                {
                    Destroy(item.go);
                    audiosToRemove.Add(item);
                    //audios.Remove(item);
                }
            }

            foreach (var VARIABLE in audiosToRemove)
            {
                audios.Remove(VARIABLE);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FadeOut(AudioSource audioSource, float timeToFade)
    {
        // Music volume fades until zero.
        // When it does, change the music
        // After changing music, fade back to audio before.
        // End.

        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / timeToFade;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    IEnumerator FadeIn(AudioSource audioSource, float volume, float timeToFade)
    {
        // Music volume fades until zero.
        // When it does, change the music
        // After changing music, fade back to audio before.
        // End.
        audioSource.Play();

        while (audioSource.volume < volume)
        {
            audioSource.volume += volume * Time.deltaTime / timeToFade;

            yield return null;
        }
    }

    IEnumerator ChangeMusicFade(AudioSource audioSource, AudioClip clip, float timeToFade)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / timeToFade;

            yield return new WaitForEndOfFrame();
        }

        audioSource.Stop();
        audioSource.clip = clip;
        yield return new WaitForEndOfFrame();
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / timeToFade;

            yield return new WaitForEndOfFrame();
        }
        changeMusicCoroutine = null;
    }
}