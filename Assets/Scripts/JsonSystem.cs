using System.IO;
using AudioDelegates;
using UnityEngine;
using UnityEngine.Networking;

public static class JsonSystem
{
    public static void SaveMapToJson(string name, Map map)
    {
        string json = JsonUtility.ToJson(map, true);
        File.WriteAllText(Application.dataPath  + "/StreamingAssets/MapData/" + map.mapName + ".json", json);
    }

    public static void SaveAudio(string originalPath, AudioClip clip)
    {
        var path = Application.dataPath + "/StreamingAssets/MapData/" + clip.name + ".mp3";
        File.Copy(originalPath, path, true);
    }
    
    public static Map LoadMapToJson(string path)
    {
        string jsonData = File.ReadAllText(path);
        var mapData = JsonUtility.FromJson<Map>(jsonData);
        return mapData;
    }

    public static AudioClip LoadAudioClip(string filePath)
    {
        AudioClip audioClip = null;
        string extension = Path.GetExtension(filePath).ToLower();

        switch (extension)
        {
            case ".mp3":
                audioClip = LoadMP3(filePath);
                break;
            case ".wav":
                audioClip = LoadWAV(filePath);
                break;
            // Add cases for other audio formats if needed
            default:
                Debug.LogError($"Unsupported audio file format: {extension}");
                break;
        }

        return audioClip;
    }

    private static AudioClip LoadMP3(string filePath)
    {
        AudioClip audioClip = null;
        using var www = UnityWebRequestMultimedia.GetAudioClip($"file://{filePath}", AudioType.MPEG);
        var downloadHandler = new DownloadHandlerAudioClip(www.url, AudioType.MPEG);
        www.downloadHandler = downloadHandler;

        www.SendWebRequest();
        while (!www.isDone) { } // Wait for the request to complete

        if (www.result == UnityWebRequest.Result.Success)
        {
            audioClip = DownloadHandlerAudioClip.GetContent(www);
        }
        else
        {
            Debug.LogError($"Failed to load MP3 file: {www.error}");
        }

        return audioClip;
    }
    
    private static AudioClip LoadWAV(string filePath)
    {
        AudioClip audioClip = null;
        using var www = UnityWebRequestMultimedia.GetAudioClip($"file://{filePath}", AudioType.WAV);
        var downloadHandler = new DownloadHandlerAudioClip(www.url, AudioType.WAV);
        www.downloadHandler = downloadHandler;

        www.SendWebRequest();
        while (!www.isDone) { } // Wait for the request to complete

        if (www.result == UnityWebRequest.Result.Success)
        {
            audioClip = DownloadHandlerAudioClip.GetContent(www);
        }
        else
        {
            Debug.LogError($"Failed to load WAV file: {www.error}");
        }

        return audioClip;
    }
}