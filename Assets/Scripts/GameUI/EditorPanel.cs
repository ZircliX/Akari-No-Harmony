using AnotherFileBrowser.Windows;
using AudioDelegates;
using MapsGenerators;
using TMPro;
using UnityEngine;

public class EditorPanel : MonoBehaviour
{
    [Header("Inputs")] 
        public TMP_InputField songNameText;
        public TMP_InputField bpmText;
        public TMP_InputField offsetText;
    [Space]
        public TMP_InputField mapNameText;
        public TMP_InputField mapDiffText;
        public TMP_InputField mapSpeedText;
        public TMP_InputField beatSpeedText;

    [Header("Audio Data")] 
        private string songName;
        private int bpm;
        private float offset;
        private AudioClip mapSong;
        private Song songData;

    [Header("Map Data")] 
        private string mapName;
        private int mapDiff;
        private float mapSpeed;
        private float beatSpeed;
        private Map mapData;

    private string audioFilePath;
    
    public void OpenBrowser()
    {
        var bp = new BrowserProperties
        {
            filter = "mp3 files (*.mp3)|*.mp3|All Files (*.*)|*.*",
            filterIndex = 0
        };
        
        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            mapSong = JsonSystem.LoadAudioClip(path);
            audioFilePath = path;
            Debug.Log(audioFilePath);
        });
    }

    public void CreateMap()
    {
        GetData();
        
        songData = MapGenerator.CreateSongData(mapSong, songName, bpm, offset);
        MapGenerator.AnalyseSong(songData, beatSpeed, mapSpeed);
        
        mapData = MapGenerator.CreateMap(mapName, mapDiff, mapSpeed, songData);
        
        MapGenerator.SaveMap(mapData, mapSong, audioFilePath);
    }

    private void GetData()
    {
        songName = songNameText.text;
        mapSong.name = songName;
        int.TryParse(bpmText.text, out bpm);
        float.TryParse(offsetText.text, out offset);
        
        mapName = mapNameText.text;
        int.TryParse(mapDiffText.text, out mapDiff);
        float.TryParse(mapSpeedText.text, out mapSpeed);
        float.TryParse(beatSpeedText.text, out beatSpeed);
    }
}