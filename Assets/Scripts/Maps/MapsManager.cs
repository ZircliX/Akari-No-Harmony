using System.IO;
using Audio;
using UnityEngine;

namespace Maps
{
    public class MapsManager : MonoBehaviour
    {
        private AudioClip sound;
        
        private void Start()
        {
            ChooseRandomJsonFile();
        }

        private void ChooseRandomJsonFile()
        {
            string[] mp3Files = Directory.GetFiles(Application.dataPath + "/StreamingAssets/MapData/", "*.mp3");

            int randomIndex = Random.Range(0, mp3Files.Length);
                
            string randomMusic = mp3Files[randomIndex];

            AudioManager.Instance.PlaySound(JsonSystem.LoadAudioClip(randomMusic));
        }
    }
}