using System.Collections.Generic;
using System.IO;
using System.Linq;
using Audio;
using AudioDelegates;
using MapsGenerators;
using Menu;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class LevelSelection : MonoBehaviour
    {
        public GameObject buttons;
        public Transform content;

        public static List<AudioClip> clipsList = new();
        private static List<Map> maps;
        public static List<Map> orderedMaps { get; private set; }

        private void OnEnable()
        {
            GetMaps();
        }

        public void Refresh()
        {
            GetMaps();
            foreach (var map in orderedMaps)
            {
                clipsList.Add(JsonSystem.LoadAudioClip(Application.dataPath  + "/StreamingAssets/MapData/" + map.songData.songName + ".mp3"));
            }
        }

        private void GetMaps()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
            
            string[] mapsPath = Directory.GetFiles(Application.dataPath + "/StreamingAssets/MapData/", "*.json");
            
            maps = mapsPath.Select(JsonSystem.LoadMapToJson).ToList();
            orderedMaps = maps.OrderBy(m => m.mapDiff).ToList();

            for (var index = 0; index < orderedMaps.Count; index++)
            {
                var map = orderedMaps[index];
                SetupMapInfo(map, index);
            }
        }

        private void SetupMapInfo(Map map, int index)
        {
            var newMapInfo = Instantiate(buttons, content);
            newMapInfo.name = map.mapName;
            
            newMapInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = map.mapName;
            
            var mapS = newMapInfo.GetComponent<MapSelect>();
            mapS.map = map;
            mapS.mapIndex = index;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ReloadAudios()
        {
            string path = Application.dataPath + "/StreamingAssets/MapData/";
            string[] mapsPath = Directory.GetFiles(path, "*.json");
            
            maps = mapsPath.Select(JsonSystem.LoadMapToJson).ToList();
            orderedMaps = maps.OrderBy(m => m.mapDiff).ToList();
            
            foreach (var map in orderedMaps)
            {
                clipsList.Add(JsonSystem.LoadAudioClip(path + map.songData.songName + ".mp3"));
            }
        }
    }
}