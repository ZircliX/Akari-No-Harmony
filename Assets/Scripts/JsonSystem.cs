using System;
using System.IO;
using AudioDelegates;
using UnityEngine;

public static class JsonSystem
{
    public static void SaveMapToJson(string name, Map map)
    {
        string json = JsonUtility.ToJson(map, true);
        File.WriteAllText(Application.dataPath  + "/StreamingAssets/MapData/" + map.mapName + ".json", json);
    }
    
    public static Map LoadMapToJson(string path)
    {
        string jsonData = File.ReadAllText(path);
        Map mapData = JsonUtility.FromJson<Map>(jsonData);
        return mapData;
    }

    public static AudioClip ConvertToAudioClip(SongAudio audio)
    {   
        byte[] audioBytes = Convert.FromBase64String(audio.base64AudioData);
        AudioClip audioClip = AudioClip.Create("AudioClip", audioBytes.Length, audio.channels, audio.sampleRate, false);
        
        float[] floatArray = new float[audioBytes.Length / 4];

        for (int i = 0; i < floatArray.Length; i++)
        {
            floatArray[i] = BitConverter.ToSingle(audioBytes, i * 4);
        }
        audioClip.SetData(floatArray, 0);
        return audioClip;
    }
}