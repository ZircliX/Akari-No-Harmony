using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Optionally, find the GameManager object in the scene, if it's not already set.
                _instance = FindAnyObjectByType<AudioManager>();
                if (_instance == null)
                {
                    // Create a new GameObject with a GameManager component if none exists.
                    GameObject gameManager = new GameObject("GameManager");
                    _instance = gameManager.AddComponent<AudioManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Ensure there's only one instance by destroying duplicates.
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Optionally, make the GameManager persist across scenes.
        }
    }
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        PlayMusic("Theme");
    }

    private void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        musicSource.clip = s.clip;
        musicSource.Play();
    }
    
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        sfxSource.clip = s.clip;
        sfxSource.Play();
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        
        musicSource.clip = s.clip;
        musicSource.Stop();
    }
}