using System.Collections.Generic;
using Circles;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
        private float songBpm;
        
    //The time window acceptable to accept a touch on the beat
        public const float 
            perfectTiming = 0.1f, 
            goodTiming = 0.25f,
            missTiming = 0.5f; 

    //The number of seconds for each song beat
        private float secPerBeat;

    //Current song position, in seconds
        public List<float> circlesPositionInSeconds;
        public float currentCirclePositionInSeconds;

        public float time;
        public float elapsedTime { get; private set; }
    [HideInInspector]
        public float lastUserInputTime;
    [HideInInspector]
        public float timingDifference;

    //How many seconds have passed since the song started
        private float dspSongTime;
        
    //The offset to the first beat of the song in seconds
        private float firstBeatOffset;

    //an AudioSource attached to this GameObject that will play the music.
        private AudioSource musicSource;
        
    //Conductor instance
        public static Conductor Instance;

    void Awake()
    {
        Instance = this;
        
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();
        
        // Load the precomputed data
        LoadPrecomputedData();
    }
    
    void Start()
    {
        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;
        
        // Start the music playback
        musicSource.Play();
    }
    
    void Update()
    {
        // Update the elapsed time
        elapsedTime = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        time += Time.deltaTime;
    }
    
    public float OnBeatClick()
    {
        // Record the time of the user input
        lastUserInputTime = elapsedTime;
        
        // Determine the current circle position 
        var currentCircle = Spawners.Instance.spawnedCircles.Peek();
        currentCirclePositionInSeconds = currentCircle.timeToBeat;

        // Calculate the timing difference between the click and the actual beat
        timingDifference = Mathf.Abs(lastUserInputTime - currentCirclePositionInSeconds);

        return timingDifference;
    }
    
    private void LoadPrecomputedData()
    {
        // Load the precomputed data from the file or serialized format
        Map mapData = JsonSystem.LoadMapToJson("ZircliX_Test");

        // Initialize the CONDUCTOR variables based on the loaded data
        songBpm = mapData.songData.songBPM;
        secPerBeat = 60f / songBpm;
        firstBeatOffset = mapData.songData.songOffset;
        circlesPositionInSeconds = mapData.songData.songPositionInSeconds;
    }
}