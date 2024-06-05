using System.Collections.Generic;
using System.IO;
using System.Linq;
using AudioDelegates;
using GamePlay;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    public class LevelSelection : MonoBehaviour
    {
        public GameObject buttons;
        public Transform content;

        private List<Map> maps;
        private List<Map> orderedMaps;

        public static LevelSelection Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            GetMaps();
        }

        public void GetMaps()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
            
            string[] mapsPath = Directory.GetFiles(Application.dataPath + "/StreamingAssets/MapData/", "*.json");
            
            maps = mapsPath.Select(JsonSystem.LoadMapToJson).ToList();
            orderedMaps = maps.OrderBy(m => m.mapDiff).ToList();

            foreach (var map in orderedMaps)
            {
                SetupMapInfo(map);
            }
        }

        private void SetupMapInfo(Map map)
        {
            var newMapInfo = Instantiate(buttons, content);
            newMapInfo.name = map.mapName;
            
            newMapInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = map.mapName;
            newMapInfo.GetComponent<MapSelect>().map = map;
        }

        public void LoadLevel(int index)
        {
            GameManager.Instance.SwitchState(1);
            SceneManager.LoadScene(index);
        }
    }
}