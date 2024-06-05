using System.Collections;
using Circles;
using TMPro;
using UnityEngine;

namespace GamePlay
{
    public class Conductor : MonoBehaviour
    {
        private float currentCirclePositionInSeconds;
        
        public float elapsedTime { get; private set; }
    
        private float lastUserInputTime;

        private float timingDifference;
    
        private float dspSongTime;
        private float pauseTime;

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

        private void UpdateCountdownText()
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
            dspSongTime = (float)AudioSettings.dspTime;
        }

        private void Update()
        {
            if (countdownTimer > 0) return;
            
            if (GameManager.Instance.state == GameManager.GameState.LevelInProgress)
            {
                elapsedTime = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset - pauseTime);
        
                if (elapsedTime >= musicSource.clip.length)
                {
                    GameManager.Instance.SwitchState(5);
                }
            }
            if (GameManager.Instance.state == GameManager.GameState.GamePause)
            {
                pauseTime += Time.deltaTime;
            }
        }
    
        public float OnBeatClick(CircleManager currentCircle)
        {
            lastUserInputTime = elapsedTime;
        
            currentCirclePositionInSeconds = currentCircle.circleData.timeToBeat;
        
            timingDifference = Mathf.Abs(currentCirclePositionInSeconds - lastUserInputTime);
        
            return timingDifference;
        }
    
        private void LoadPrecomputedData()
        {
            var mapData = GameManager.Instance.level;
        
            firstBeatOffset = mapData.songData.songOffset;
            musicSource.clip = JsonSystem.LoadAudioClip(Application.dataPath + "/StreamingAssets/MapData/" + mapData.songData.songName + ".mp3");
        }
    }
}