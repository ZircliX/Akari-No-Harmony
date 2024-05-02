using System.Collections.Generic;
using Circles;
using UnityEngine;

public class Spawners : MonoBehaviour
{
    private float currentBeatTime;

    public Transform[] spawners;
    
    private List<Circle> circleList = new();

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
            
            //Debug.Log(currentBeatTime + " | " + beatToSpawn);
            
            if (!(currentBeatTime >= beatToSpawn)) continue;
            
            // Instantiate the circle and add it to the dictionary
            var spawnedCircle = Instantiate(circle.circleData.circlePrefab,
                spawners[circle.columnIndex].position, Quaternion.identity);

            spawnedCircle.GetComponent<CircleManager>().circleData = circle;
                
            //Debug.Log(spawnedCircle);
            circleList.Remove(circle);
        }
    }
}