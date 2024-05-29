using Circles;
using GamePlay;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    private double currentCirclePositionInSeconds;
        
    public double elapsedTime { get; private set; }
    
    private double lastUserInputTime;

    private double timingDifference;
    
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
        
        return timingDifference;
    }
    
    private void LoadPrecomputedData()
    {
        var mapData = JsonSystem.LoadMapToJson(Application.dataPath + "/StreamingAssets/MapData/" + GameManager.Instance.level + ".json");
        
        firstBeatOffset = mapData.songData.songOffset;
        musicSource.clip = JsonSystem.LoadAudioClip(Application.dataPath + "/StreamingAssets/MapData/" + mapData.songData.songName + ".mp3");
    }
}