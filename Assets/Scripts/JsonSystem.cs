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
    
    public static Map LoadMapToJson(string name)
    {
        string jsonData = File.ReadAllText(Application.dataPath  + "/StreamingAssets/MapData/" + name + ".json");
        Map mapData = JsonUtility.FromJson<Map>(jsonData);
        return mapData;
    }
}