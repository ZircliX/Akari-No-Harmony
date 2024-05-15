using System.IO;
using AudioDelegates;
using GamePlay;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    //The time window acceptable to accept a touch on the beat
        public const float 
            perfectTiming = 0.05f,
            goodTiming = 0.1f,
            missTiming = 0.2f;
    
    //Current song position, in seconds
        public double currentCirclePositionInSeconds;
        
        public double elapsedTime { get; private set; }
    [HideInInspector]
        public double lastUserInputTime;
    [HideInInspector]
        public double timingDifference;

    //How many seconds have passed since the song started
        private double dspSongTime;
        
    //The offset to the first beat of the song in seconds
        private float firstBeatOffset;

    //an AudioSource attached to this GameObject that will play the music.
        private AudioSource musicSource;
        
    //Conductor instance
        public static Conductor Instance;

    private void Awake()
    {
        Instance = this;
        
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();
        
        // Load the precomputed data
        LoadPrecomputedData();
    }

    private void Start()
    {
        //Record the time when the music starts
        dspSongTime = AudioSettings.dspTime;
        
        // Start the music playback
        musicSource.Play();
    }

    private void Update()
    {
        // Update the elapsed time
        elapsedTime = AudioSettings.dspTime - dspSongTime - firstBeatOffset;
    }
    
    public double OnBeatClick()
    {
        // Record the time of the user input
        lastUserInputTime = elapsedTime;
        
        // Determine the current circle position 
        var currentCircle = Spawners.Instance.spawnedCircles[0];
        currentCirclePositionInSeconds = currentCircle.circleData.timeToBeat;

        // Calculate the timing difference between the click and the actual beat
        timingDifference = Mathf.Abs((float)(currentCirclePositionInSeconds - lastUserInputTime));
        
        //Debug.Log(lastUserInputTime + " | " + currentCirclePositionInSeconds + " | " + timingDifference);

        return timingDifference;
    }
    
    private void LoadPrecomputedData()
    {
        // Load the precomputed data from the file or serialized format
        var mapData = JsonSystem.LoadMapToJson("ZiTest");

        // Initialize the CONDUCTOR variables based on the loaded data
        firstBeatOffset = mapData.songData.songOffset;
        musicSource.clip = mapData.songData.songAudio;
    }
}