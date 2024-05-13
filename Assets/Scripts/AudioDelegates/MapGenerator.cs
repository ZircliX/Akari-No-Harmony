using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    public bool create;
    public GameObject[] circlesType;

    [Header("Song Data")]
        public new AudioClip audio;
        public int bpm;
        public float offset;
        public string songName;
        private Song songData;
    
    [Space]
    [Header("Map Data")]
        public string mapName;
        public int typeChangeProba;
        private int currentTypeIndex;
        private Map mapData;
    
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
    }

    private void CreateMap()
    {
        mapData = new Map
        {
            mapName = mapName,
            songData = songData
        };

        foreach (var time in songData.songPositionInSeconds)
        {
            if (Random.Range(0, typeChangeProba) == 0) ChangeCircleType();
            
            var newCircle = new Circle
            {
                circlePrefab = circlesType[currentTypeIndex],
                id = currentTypeIndex,
                downSpeed = 4f,
                timeToSpawn = time - 2f,
                timeToBeat = time,
                columnIndex = Random.Range(0, 3)
            };
            
            mapData.circles.Add(newCircle);
        }
    }

    private void ChangeCircleType()
    {
        currentTypeIndex = Random.Range(0, circlesType.Length);
    }

    private void CreateSongData()
    {
        songData = new Song
        {
            songAudio = audio,
            songBPM = bpm,
            songOffset = offset,
            songName = songName
        };
    }

    private void AnalyseSong()
    {
        float secPerBeat = 60f / songData.songBPM;
        float currentBeat = 0f;
        float currentSecond = songData.songOffset;

        while (currentSecond < songData.songAudio.length)
        {
            if (currentBeat % 2 == 0 && currentSecond >= 4)
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
    public AudioClip songAudio;
    public int songBPM;
    public float songOffset;
    public string songName;
    
    public List<float> songPositionInSeconds = new();
}

[System.Serializable]
public class Map
{
    public string mapName;
    public Song songData;
    public List<Circle> circles = new();
}

[System.Serializable]
public class Circle
{
    public GameObject circlePrefab;
    
    public int id;
    public float downSpeed;
    
    public float timeToSpawn;
    public float timeToBeat;
    
    [Range(0, 2)]
    public int columnIndex;
}