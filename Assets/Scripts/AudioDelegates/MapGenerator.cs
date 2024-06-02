using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AudioDelegates
{
    public static class MapGenerator
    {
        private static int currentTypeIndex;
        
        private static int duplicateNum = 1;
        private static readonly List<int> usedValues = new();
        
        private static int numOfSameColor;

        public static void SaveMap(Map mapData, AudioClip audioClip, string filePath)
        {
            JsonSystem.SaveMapToJson(mapData.mapName, mapData);
            JsonSystem.SaveAudio(filePath, audioClip);
        }

        public static Map CreateMap(string mapName, int mapDiff, Song songData)
        {
            var mapData = new Map
            {
                mapName = mapName,
                mapDiff = mapDiff,
                songData = songData
            };

            foreach (var time in songData.songPositionInSeconds)
            {
                usedValues.Clear();
                numOfSameColor++;
                
                if (ShouldChangeCircleType())
                {
                    numOfSameColor = 0;
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

            return mapData;
        }

        private static bool ShouldChangeCircleType()
        {
            return numOfSameColor >= 4;
        }

        private static int GetDuplicateNum()
        {
            return Random.Range(0, 3) == 0 ? Random.Range(2, 4) : 1;
        }

        private static int GetUniqueRandomValue(int min, int max)
        {
            int randomValue;
            do {
                randomValue = Random.Range(min, max);
            } 
            while (usedValues.Contains(randomValue));

            usedValues.Add(randomValue);
            return randomValue;
        }

        private static Circle CreateCircle(double time, int typeIndex, int columnIndex)
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

        public static Song CreateSongData(AudioClip audio, string songName, int bpm, float offset)
        {
            var songData = new Song
            {
                songBPM = bpm,
                songOffset = offset,
                songName = songName,
                songLength = (int)audio.length
            };

            return songData;
        }

        public static void AnalyseSong(Song songData, int beatSpeed)
        {
            float secPerBeat = 60f / songData.songBPM;
            float currentBeat = 0f;
            float currentSecond = songData.songOffset;
            
            

            while (currentSecond < songData.songLength)
            {
                if (currentBeat % beatSpeed == 0 && currentSecond >= 3)
                {
                    songData.songPositionInSeconds.Add(currentSecond);
                }
            
                currentBeat += 1f;
                currentSecond += secPerBeat;
            }
        }
    }
    
    [System.Serializable]
    public class Song
    {
        public int songBPM;
        public float songOffset;
        public string songName;
        public int songLength;
    
        public List<double> songPositionInSeconds = new();
    }
    
    [System.Serializable]
    public class Map
    {
        public string mapName;
        public int mapDiff;
        public Song songData;
        public List<Circle> circles = new();
    }
    
    [System.Serializable]
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