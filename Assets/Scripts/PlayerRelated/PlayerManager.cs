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
        private double timingDifference;

        private const float
            perfectTiming = 0.05f,
            goodTiming = 0.1f,
            missTiming = 0.5f;
        
        private ClickIndex click;
        private enum ClickIndex
        {
            left = 0,
            middle = 1,
            right = 2
        }

        private int colorSwitch;
        public ColorIndex color;
        public enum ColorIndex
        {
            blue = 0,
            yellow = 1,
            red = 2
        }

        public static PlayerManager Instance;

        private void Awake()
        {
            Instance = this;
        }
        
        #endregion

        public void ChangeBanner(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;
            
            colorSwitch = (int)context.ReadValue<float>();
            color = (ColorIndex) (int)color + colorSwitch;

            if ((int)color >= 3) color = 0;
            if ((int)color < 0) color = (ColorIndex)2;
        }

        #region  HandleClick

        public void LeftClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) OnBeatClick(0);
        }
        public void MiddleClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) OnBeatClick(1);
        }
        public void RightClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) OnBeatClick(2);
        }
        
        #endregion

        private void OnBeatClick(int clickIndex)
        {
            currentCircle = Spawners.Instance.spawnedCircles[clickIndex][0];
            
            timingDifference = Conductor.Instance.OnBeatClick(currentCircle);
            
            bool correctHit = currentCircle.circleData.columnIndex == clickIndex &&
                              (int)color == currentCircle.circleData.typeIndex;

            if (timingDifference > missTiming) return;
            
            (int score, int health) = CalculateScoreAndHealth(timingDifference, correctHit);
            Hit(health, score);
            currentCircle.isHit = true;
        }

        private (int score, int health) CalculateScoreAndHealth(double timeDifference, bool correctHit)
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