using System.Collections;
using Circles;
using GamePlay;
using TMPro;
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
    
    public float countdownDuration = 3f;
    public float countdownTimer;
    public TextMeshProUGUI countdownText;

    public static Conductor Instance;

    private void Awake()
    {
        Instance = this;
        
        musicSource = GetComponent<AudioSource>();
        
        LoadPrecomputedData();
    }
    
    private IEnumerator CountdownCoroutine()
    {
        while (countdownTimer > 0)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            countdownTimer--;
            UpdateCountdownText();
        }

        StartMap();
        Destroy(countdownText.gameObject);
    }

    void UpdateCountdownText()
    {
        // Update the countdown text with the remaining time
        countdownText.text = countdownTimer.ToString();
    }

    private void Start()
    {
        countdownTimer = countdownDuration;
        StartCoroutine(CountdownCoroutine());
    }

    private void StartMap()
    {
        musicSource.Play();
        dspSongTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        if (countdownTimer <= 0)
        {
            elapsedTime = AudioSettings.dspTime - dspSongTime - firstBeatOffset;
        
            if (elapsedTime >= musicSource.clip.length)
            {
                GameManager.Instance.SwitchState(5);
            }
        }
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
        var mapData = GameManager.Instance.level;
        
        firstBeatOffset = mapData.songData.songOffset;
        musicSource.clip = JsonSystem.LoadAudioClip(Application.dataPath + "/StreamingAssets/MapData/" + mapData.songData.songName + ".mp3");
    }
}