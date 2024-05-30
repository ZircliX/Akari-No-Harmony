using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AudioDelegates
{
    [ExecuteInEditMode]
    public class MapGenerator : MonoBehaviour
    {
        public bool create;

        [Header("Song Data")]
        public new AudioClip audio;
        public int bpm;
        public float offset;
        private Song songData;
    
        [Space]
        [Header("Map Data")]
        public string mapName;
        public int mapDiff;
        public int mapSpeed;
        private int currentTypeIndex;
        private Map mapData;

        [Header("Create Duplicates")]
        private int duplicateNum = 1;
        private List<int> usedValues = new();
        
        [Header("Random Color")]
        public int typeChangeProba;
        private int numOfSame;
    
        private void Update()
        {
            if (create)
            {
                create = false;

                CreateSongData();
                AnalyseSong();
            
                CreateMap();
                SaveMap();
            }
        }

        private void SaveMap()
        {
            JsonSystem.SaveMapToJson(mapData.mapName, mapData);
            JsonSystem.SaveAudio(audio);
        }

        private void CreateMap()
        {
            mapData = new Map
            {
                mapName = mapName,
                mapDiff = mapDiff,
                songData = songData
            };

            foreach (var time in songData.songPositionInSeconds)
            {
                usedValues.Clear();
                numOfSame++;
                
                if (ShouldChangeCircleType())
                {
                    numOfSame = 0;
                    currentTypeIndex = Random.Range(0, 3);
                }

                duplicateNum = GetDuplicateNum();

                for (int i = 0; i < duplicateNum; i++)
                {
                    int columnIndex = GetUniqueRandomValue(0, 3);
                    var newCircle = CreateCircle(time, currentTypeIndex, columnIndex);
                    mapData.circles.Add(newCircle);
                }
            }
        }

        private bool ShouldChangeCircleType()
        {
            return numOfSame >= 4;
        }

        private int GetDuplicateNum()
        {
            return Random.Range(0, 3) == 0 ? Random.Range(2, 4) : 1;
        }

        private int GetUniqueRandomValue(int min, int max)
        {
            int randomValue;
            do {
                randomValue = Random.Range(min, max);
            } 
            while (usedValues.Contains(randomValue));

            usedValues.Add(randomValue);
            return randomValue;
        }

        private Circle CreateCircle(double time, int typeIndex, int columnIndex)
        {
            return new Circle
            {
                typeIndex = typeIndex,
                downSpeed = 4f,
                timeToSpawn = time - 2f,
                timeToBeat = time,
                columnIndex = columnIndex
            };
        }

        private void CreateSongData()
        {
            songData = new Song
            {
                songBPM = bpm,
                songOffset = offset,
                songName = audio.name,
                songLength = audio.length
            };
        }

        private void AnalyseSong()
        {
            float secPerBeat = 60f / songData.songBPM;
            float currentBeat = 0f;
            float currentSecond = songData.songOffset;

            while (currentSecond < songData.songLength)
            {
                if (currentBeat % mapSpeed == 0 && currentSecond >= 3)
                {
                    songData.songPositionInSeconds.Add(currentSecond);
                }
            
                currentBeat += 1f;
                currentSecond += secPerBeat;
            }
        }
    }
    
    public class Song
    {
        public int songBPM;
        public float songOffset;
        public string songName;
        public float songLength;
    
        public List<double> songPositionInSeconds = new();
    }
    
    public class Map
    {
        public string mapName;
        public int mapDiff;
        public Song songData;
        public List<Circle> circles = new();
    }
    
    public class Circle
    {
        public int typeIndex;
        
        public float downSpeed;
    
        public double timeToSpawn;
        public double timeToBeat;
    
        [Range(0, 2)]
        public int columnIndex;
    }
}