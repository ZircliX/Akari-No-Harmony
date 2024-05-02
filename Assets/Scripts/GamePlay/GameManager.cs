using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    /*
    circleQueue.Enqueue(gameObject); //add
    circleQueue.Dequeue(); //remove
    circleQueue.Contains(gameObject); //check
    circleQueue.Peek(); // see first
    */
    
    
    private Queue<GameObject> circleQueue = new();

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}