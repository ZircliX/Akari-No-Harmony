using System.Collections;
using Circles;
using GameUI;
using Score;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay
{
    public class PlayerManager : MonoBehaviour
    {
        #region Class Variables
        
        public bool[] brokenIndex;

        private CircleManager currentCircle;
        private float timingDifference;

        private const float
            perfectTiming = 0.06f,
            goodTiming = 0.16f,
            missTiming = 0.5f;

        public int colorIndex;
        private string[] rang = { "Left", "Middle", "Right" };

        public static PlayerManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            brokenIndex = new[] { false, false, false };
        }
        
        #endregion

        public void ClickNoteLeft(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            OnBeatClick(0);
        }
        public void ClickNoteMiddle(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            OnBeatClick(1);
        }
        public void ClickNoteRight(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            OnBeatClick(2);
        }

        private void OnBeatClick(int clickIndex)
        {
            if (Spawners.Instance.spawnedCircles[clickIndex].Count <= 0) return;
            
            currentCircle = Spawners.Instance.spawnedCircles[clickIndex][0];
            
            timingDifference = Conductor.Instance.OnBeatClick(currentCircle);
            
            bool correctHit = currentCircle.circleData.columnIndex == clickIndex &&
                              colorIndex == currentCircle.circleData.typeIndex;

            if (timingDifference > missTiming) return;
            
            (int score, int health) = CalculateScoreAndHealth(timingDifference, correctHit);
            Hit(health, score);
            PlayFeedbacks(score, clickIndex);
            currentCircle.isHit = true;
        }

        private (int score, int health) CalculateScoreAndHealth(float timeDifference, bool correctHit)
        {
            return correctHit switch
            {
                true when timeDifference <= perfectTiming => (300, 10),
                true when timeDifference <= goodTiming => (100, 5),
                true when timeDifference <= missTiming => (-10, -10),
                false => (-10, -10),
                _ => (0, 0)
            };
        }

        public void Hit(int health, int points)
        {
            ScoreCombo.Instance.health += health;
            ScoreCombo.Instance.AddScore(points);
        }

        private void PlayFeedbacks(int score, int index)
        {
            switch (score)
            {
                case > 0:
                    VFXManager.Instance.PlayVFX("Splash" + rang[index]);
                    break;
                case <= 0:
                    StartCoroutine(SetBroken(index));
                    break;
            }
        }

        private IEnumerator SetBroken(int index)
        {
            brokenIndex[index] = true;
            yield return new WaitForSeconds(0.3f);
            brokenIndex[index] = false;
        }
    }
}