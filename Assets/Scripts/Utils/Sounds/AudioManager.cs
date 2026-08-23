using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource musicSource;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Header("SFX Clips")]
    [SerializeField] private SoundData[] soundDataArray;

    [SerializeField] private int maxConcurrentSFX = 10;
    [SerializeField] private SoundEmitter soundEmitterPrefab;

    private readonly Dictionary<string, AudioClip> soundDataDictionary = new();
    private readonly Queue<SoundEmitter> sfxSourcesQueue = new();

    private void Start()
    {
        PlayMainMenuMusic();
        LoadSFXData();
    }

    private void LoadSFXData()
    {
        foreach (var soundData in soundDataArray)
        {
            if (!soundDataDictionary.ContainsKey(soundData.key))
            {
                soundDataDictionary.Add(soundData.key, soundData.clip);
            }
            else
            {
                LogService.LogWarning($"Duplicate sound key detected: {soundData.key}. Please ensure all keys are unique.");
            }
        }
    }

    void OnEnable()
    {
        SceneController.OnSceneLoadEnded += HandleSceneLoadEnded;
    }

    void OnDisable()
    {
        SceneController.OnSceneLoadEnded -= HandleSceneLoadEnded;
    }

    private void HandleSceneLoadEnded(string sceneAddress)
    {
        if (sceneAddress == SceneDatabase.GAMEPLAY)
        {
            PlayGameplayMusic();
        }
        else if (sceneAddress == SceneDatabase.MAIN_MENU)
        {
            PlayMainMenuMusic();
        }
    }

    private void PlayMainMenuMusic()
    {
        musicSource.clip = mainMenuMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void PlayGameplayMusic()
    {
        musicSource.clip = gameplayMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void ReturnSoundEmitter(SoundEmitter soundEmitter)
    {
        if (sfxSourcesQueue.Count < maxConcurrentSFX)
        {
            sfxSourcesQueue.Enqueue(soundEmitter);
        }
    }

    private void ReloadEmitter()
    {
        for (int i = 0; i < maxConcurrentSFX; i++)
        {
            SoundEmitter newEmitter = Instantiate(soundEmitterPrefab, transform);
            newEmitter.gameObject.SetActive(false);
            sfxSourcesQueue.Enqueue(newEmitter);
        }
    }

    public void PlaySFX(string key)
    {
        if (soundDataDictionary.TryGetValue(key, out AudioClip clip))
        {
            if (sfxSourcesQueue.Count == 0)
            {
                ReloadEmitter();
            }

            SoundEmitter emitter = sfxSourcesQueue.Dequeue();
            emitter.gameObject.SetActive(true);
            emitter.PlaySound(clip);
        }
        else
        {
            LogService.LogWarning($"Sound key '{key}' not found in the sound data dictionary.");
        }
    }
}

[Serializable]
public struct SoundData
{
    public string key;
    public AudioClip clip;
}