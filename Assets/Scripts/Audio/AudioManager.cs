using System;
using UnityEngine;

namespace Audio
{
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
                        GameObject audioManager = new GameObject("AudioManager");
                        _instance = audioManager.AddComponent<AudioManager>();
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
        }

        public void PlaySound(AudioClip audioClip)
        {
            musicSource.clip = audioClip;
            musicSource.Play();
        }

        public void StopSound()
        {
            musicSource.Stop();
        }
    
        public void PlaySFX(string sfxName)
        {
            var s = Array.Find(sfxSounds, x => x.name == sfxName);

            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }
}