using System.IO;
using UnityEngine;

public static class JsonSystem
{
    public static void SaveMapToJson(string name, Map map)
    {
        string json = JsonUtility.ToJson(map, true);
        File.WriteAllText(Application.dataPath + "/Resources/MapData/" + map.mapName + ".json", json);
    }
    
    public static Map LoadMapToJson(string name)
    {
        string json = File.ReadAllText(Application.dataPath + "/Resources/MapData/" + name + ".json");
        Map data = JsonUtility.FromJson<Map>(json);
        return data;
    }
}