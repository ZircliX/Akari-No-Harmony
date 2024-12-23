using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

        public void PlaySound(AudioClip audioClip)
        {
            musicSource.clip = audioClip;
            musicSource.Play();
        }

        public void StopSound()
        {
            musicSource.Stop();
        }
    
        public void PlayMusic(string musicName)
        {
            var s = Array.Find(musicSounds, x => x.name == musicName);

            musicSource.clip = s.clip;
            musicSource.Play();
        }
        
        public void PlaySFX(string sfxName)
        {
            var s = Array.Find(sfxSounds, x => x.name == sfxName);
            
            sfxSource.PlayOneShot(s.clip);
        }

        public void MusicPause()
        {
            musicSource.Pause();
        }

        public void MusicResume()
        {
            musicSource.UnPause();
        }
    }
}