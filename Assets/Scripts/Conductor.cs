using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
        private float songBpm;
        
    //The time window acceptable to accept a touch on the beat
        public const float 
            perfectTiming = 0.08f, 
            goodTiming = 0.1f,
            missTiming = 0.15f; 

    //The number of seconds for each song beat
        private float secPerBeat;

    //Current song position, in seconds
        private List<float> songPositionInSeconds;
        private float currentSongPositionInSeconds;

    //Current song position, in beats
    [HideInInspector]
        public List<float> songPositionInBeats;
    [HideInInspector]
        public float currentSongPositionInBeats;

        private float elapsedTime;
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
        public static Conductor instance;

    void Awake()
    {
        instance = this;
        
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();
        
        // Load the precomputed data
        LoadPrecomputedData();
    }
    
    void Start()
    {
        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;
        //Debug.Log(dspSongTime);
        
        // Start the music playback
        musicSource.Play();
    }
    
    void Update()
    {
        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        // Determine the current song position based on the precomputed data
        int currentBeatIndex = GetCurrentBeatIndex();
        
        currentSongPositionInSeconds = songPositionInSeconds[currentBeatIndex];
        currentSongPositionInBeats = songPositionInBeats[currentBeatIndex];
    }
    
    public float OnBeatClick()
    {
        // Record the time of the user input
        lastUserInputTime = elapsedTime;

        // Calculate the timing difference between the click and the actual beat
        timingDifference = Mathf.Abs(lastUserInputTime - currentSongPositionInSeconds);

        return timingDifference;
    }
    
    private int GetCurrentBeatIndex()
    {
        // Binary search to find the current beat index
        int left = 0;
        int right = songPositionInSeconds.Count - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (songPositionInSeconds[mid] <= elapsedTime)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return right;
    }
    
    private void LoadPrecomputedData()
    {
        // Load the precomputed data from the file or serialized format
        Map mapData = JsonSystem.LoadMapToJson("ZircliX_Test");

        // Initialize the CONDUCTOR variables based on the loaded data
        songBpm = mapData.songData.songBPM;
        secPerBeat = 60f / songBpm;
        firstBeatOffset = mapData.songData.songOffset;
        songPositionInBeats = mapData.songData.songPositionInBeats;
        songPositionInSeconds = mapData.songData.songPositionInSeconds;
    }
}