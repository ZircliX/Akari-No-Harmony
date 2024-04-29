using UnityEngine;

public class Conductor : MonoBehaviour
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
        public float songBpm;
        
    //The time window acceptable to accept a touch on the beat
        private const float 
            perfectTiming = 0.08f, 
            goodTiming = 0.1f,
            missTiming = 0.15f;

    //The number of seconds for each song beat
        public float secPerBeat;

    //Current song position, in seconds
        public float songPosition;

    //Current song position, in beats
        public float songPositionInBeats;

    //How many seconds have passed since the song started
        public float dspSongTime;
        
    //The offset to the first beat of the song in seconds
        public float firstBeatOffset;

    //an AudioSource attached to this GameObject that will play the music.
        public AudioSource musicSource;
        
    //Conductor instance
        public static Conductor instance;

    void Awake()
    {
        instance = this;
        
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();
    }
    
    void Update()
    {
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;
    }
    
    public void OnBeatClick()
    {
        // Calculate the timing difference between the click and the actual beat
        float timingDifference = Mathf.Abs(songPosition % secPerBeat - secPerBeat / 2);
    
        // Check if the click was close enough to be considered successful
        switch (timingDifference)
        {
            case <= perfectTiming:
                Debug.Log("PERFECT ! ");
                // Handle Perfect beat click
                
                break;
            
            case <= goodTiming:
                Debug.Log("GOOD ! ");
                // Handle good beat click
                
                break;
            
            case > missTiming:
                Debug.Log("MISS ! ");
                // Handle missed beat click
                
                break;
        }
    }
}