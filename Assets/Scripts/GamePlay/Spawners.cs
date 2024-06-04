using System.Collections.Generic;
using AudioDelegates;
using Circles;
using UnityEngine;

namespace GamePlay
{
    public class Spawners : MonoBehaviour
    {
        private double currentBeatTime;

        public Transform[] spawners;
        private GameObject[] circlesTypes;
    
        private List<Circle> circleList = new();
        public List<List<CircleManager>> spawnedCircles = new()
        {
            new List<CircleManager>(),
            new List<CircleManager>(),
            new List<CircleManager>()
        };

        public static Spawners Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            var mapData = GameManager.Instance.level;
            circleList = mapData.circles;
            circlesTypes = Resources.LoadAll<GameObject>("Prefabs/Circles");
        }

        private void Update()
        {
            currentBeatTime = Conductor.Instance.elapsedTime;
            SpawnCircles();
        }

        private void SpawnCircles()
        {
            //Spawn circles
            foreach (var circle in circleList)
            {
                double timeToSpawn = circle.timeToSpawn;

                if (!(currentBeatTime >= timeToSpawn)) continue;
                    
                // Instantiate the circle and add it to the dictionary
                var spawnedCircle = Instantiate(circlesTypes[circle.typeIndex],
                    spawners[circle.columnIndex].position, Quaternion.identity);

                var component = spawnedCircle.GetComponent<CircleManager>();
                component.circleData = circle;
            
                spawnedCircles[component.circleData.columnIndex].Add(component);

                circle.timeToSpawn = float.MaxValue;
            }
        }

        public void RemoveCircle(CircleManager cm)
        {
            spawnedCircles[cm.circleData.columnIndex].Remove(cm);
            Destroy(cm.gameObject);
        }
    }
}