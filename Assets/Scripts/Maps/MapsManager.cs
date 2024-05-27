using System.IO;
using UnityEngine;

namespace Maps
{
    public class MapsManager : MonoBehaviour
    {
        private void Start()
        {
            ChooseRandomJsonFile();
        }

        private void ChooseRandomJsonFile()
        {
            string[] jsonFiles = Directory.GetFiles(Application.dataPath + "/StreamingAssets/MapData/", "*.json");

            if (jsonFiles.Length > 0)
            {
                int randomIndex = Random.Range(0, jsonFiles.Length);
                
                string randomJsonFilePath = jsonFiles[randomIndex];

                var map = JsonSystem.LoadMapToJson(randomJsonFilePath);

                AudioManager.Instance.PlaySound(JsonSystem.ConvertToAudioClip(map.songData.songAudio));
            }
        }
    }
}
