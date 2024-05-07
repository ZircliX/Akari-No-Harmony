using System.Collections.Generic;
using Circles;
using UnityEngine;

public class Spawners : MonoBehaviour
{
    private float currentBeatTime;

    public Transform[] spawners;
    
    private List<Circle> circleList = new();
    public List<CircleManager> spawnedCircles = new();

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
        currentBeatTime = Conductor.Instance.elapsedTime;

        SpawnCircles();
    }

    private void SpawnCircles()
    {
        //Spawn circles
        foreach (var circle in circleList)
        {
            float timeToSpawn = circle.timeToSpawn;

            if (currentBeatTime >= timeToSpawn)
            {
                // Instantiate the circle and add it to the dictionary
                var spawnedCircle = Instantiate(circle.circlePrefab,
                    spawners[circle.columnIndex].position, Quaternion.identity);

                var component = spawnedCircle.GetComponent<CircleManager>();
                component.circleData = circle;
            
                spawnedCircles.Add(component);

                circle.timeToSpawn = float.MaxValue;
            }
        }
    }

    public void RemoveCircle(CircleManager cm)
    {
        spawnedCircles.Remove(cm);
        Destroy(cm.gameObject);
    }
}