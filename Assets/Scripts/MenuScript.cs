using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private float lastVolume;
    private bool isActive = true;
    
    public AudioMixer audioMixer;
    
    public void SetVolume(float volume)
    {
        if (!isActive) return;
        
        audioMixer.SetFloat("volume", volume);
        lastVolume = volume;
    }

    public void VolumeState()
    {
        isActive = !isActive;
        audioMixer.SetFloat("volume", isActive ? lastVolume : -80);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
