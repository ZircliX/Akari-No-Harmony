using Circles;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public double currentCirclePositionInSeconds;
        
    public double elapsedTime { get; private set; }
    
    [HideInInspector]
    public double lastUserInputTime;
    [HideInInspector]
    public double timingDifference;
    
    private double dspSongTime;

    private float firstBeatOffset;

    private AudioSource musicSource;
        

    public static Conductor Instance;

    private void Awake()
    {
        Instance = this;
        
        musicSource = GetComponent<AudioSource>();
        
        LoadPrecomputedData();
    }

    private void Start()
    {
        musicSource.Play();
        
        dspSongTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        elapsedTime = AudioSettings.dspTime - dspSongTime - firstBeatOffset;
    }
    
    public double OnBeatClick(CircleManager currentCircle)
    {
        lastUserInputTime = elapsedTime;
        
        currentCirclePositionInSeconds = currentCircle.circleData.timeToBeat;
        
        timingDifference = Mathf.Abs((float)(currentCirclePositionInSeconds - lastUserInputTime));
        
        //Debug.Log(lastUserInputTime + " | " + currentCirclePositionInSeconds + " | " + timingDifference);

        return timingDifference;
    }
    
    private void LoadPrecomputedData()
    {
        var mapData = JsonSystem.LoadMapToJson("ZircliX_Test");
        
        firstBeatOffset = mapData.songData.songOffset;
        musicSource.clip = mapData.songData.songAudio;
    }
}