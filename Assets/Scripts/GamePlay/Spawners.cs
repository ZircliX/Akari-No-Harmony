using System.Collections.Generic;
using UnityEngine;

public class Spawners : MonoBehaviour
{
    private float currentBeatTime;
    
    private List<Circle> circleList = new();
    private List<GameObject> spawnedCircles = new();

    public static Spawners Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        var mapData = JsonSystem.LoadMapToJson("ZircliX_Test");
        circleList = mapData.circles;
    }

    // Update is called once per frame
    void Update()
    {
        currentBeatTime += Time.deltaTime;
        
        //Spawn circles
        foreach (var circle in circleList)
        {
            float beatToSpawn = circle.timeToSpawn;
            
            if (!(currentBeatTime >= beatToSpawn)) continue;
            
            // Instantiate the circle and add it to the dictionary
            var spawnedCircle = Instantiate(circle.circleData.circlePrefab,
                new Vector3(circle.columnIndex, 6f, 0f), Quaternion.identity);
                
            spawnedCircles.Add(spawnedCircle);
        }

        for (int i = 0; i < spawnedCircles.Count; i++)
        {
            // Move the circle down over time using Lerp or other animation techniques
            spawnedCircles[i].transform.position = Vector3.Lerp(
                spawnedCircles[i].transform.position,
                new Vector3(circleList[i].columnIndex, -3.5f, 0f),
                Time.deltaTime * circleList[i].downSpeed
            );
        }
    }
}