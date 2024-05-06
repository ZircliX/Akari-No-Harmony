using System.Collections.Generic;
using Circles;
using UnityEngine;

public class Spawners : MonoBehaviour
{
    private float currentBeatTime;

    public Transform[] spawners;
    
    private List<Circle> circleList = new();
    public Queue<Circle> spawnedCircles = new();

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
            float timeToSpawn = circle.timeToSpawn;

            if (currentBeatTime >= timeToSpawn)
            {
                // Instantiate the circle and add it to the dictionary
                var spawnedCircle = Instantiate(circle.circlePrefab,
                    spawners[circle.columnIndex].position, Quaternion.identity);

                spawnedCircle.GetComponent<CircleManager>().circleData = circle;
            
                spawnedCircles.Enqueue(circle);
            }
        }

        foreach (var circle in spawnedCircles)
        {
            circleList.Remove(circle);
        }
    }
}