using Circles;
using GamePlay;
using Score;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerRelated
{
    public class PlayerManager : MonoBehaviour
    {
        #region Class Variables

        private CircleManager currentCircle;
        private float timingDifference;

        private const float
            perfectTiming = 0.05f,
            goodTiming = 0.1f,
            missTiming = 0.5f;

        public int colorIndex;

        public static PlayerManager Instance;

        private void Awake()
        {
            Instance = this;
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
    }
}