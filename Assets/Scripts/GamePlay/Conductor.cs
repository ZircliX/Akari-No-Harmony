using System.Collections;
using Audio;
using Circles;
using TMPro;
using UnityEngine;

namespace GamePlay
{
    public class Conductor : MonoBehaviour
    {
        private float currentCirclePositionInSeconds;

        public float elapsedTime;
    
        private float lastUserInputTime;

        private float timingDifference;
    
        private float dspSongTime;
        private float pauseTime;

        private float firstBeatOffset;
    
        public float countdownDuration = 3f;
        public float countdownTimer;
        public TextMeshProUGUI countdownText;

        private AudioClip song;

        public static Conductor Instance;

        private void Awake()
        {
            Instance = this;
        
            LoadPrecomputedData();
        }

        private void OnEnable()
        {
            elapsedTime = 0f;
        }
    
        private IEnumerator CountdownCoroutine()
        {
            while (countdownTimer > 0)
            {
                yield return new WaitForSeconds(1f); // Wait for 1 second
                countdownTimer--;
                countdownText.text = countdownTimer.ToString();
            }

            StartMap();
            Destroy(countdownText.gameObject);
        }

        private void Start()
        {
            countdownTimer = countdownDuration;
            StartCoroutine(CountdownCoroutine());
        }

        private void StartMap()
        {
            AudioManager.Instance.PlaySound(song);
            dspSongTime = (float)AudioSettings.dspTime;
        }

        private void Update()
        {
            if (countdownTimer > 0) return;
            
            if (GameManager.Instance.state == GameManager.GameState.LevelInProgress)
            {
                elapsedTime = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset - pauseTime);
        
                if (elapsedTime >= song.length)
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
            song = GameManager.Instance.levelSong;
        }
    }
}