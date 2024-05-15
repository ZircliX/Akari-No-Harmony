using GamePlay;
using Score;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerRelated
{
    public class PlayerManager : MonoBehaviour
    {
        #region Class Variables
        
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
            // Calculate the timing difference between the click and the actual beat
            double timingDifference = Conductor.Instance.OnBeatClick();
            var currentCircle = Spawners.Instance.spawnedCircles[0];

            Spawners.Instance.RemoveCircle(currentCircle);
            
            bool correctHit = currentCircle.circleData.columnIndex == clickIndex
                                && (int)color == currentCircle.circleData.typeIndex;
            if (!correctHit)
            {
                MissHit();
                return;
            }

            // Check if the click was close enough to be considered successful
            switch (timingDifference)
            {
                // Handle Perfect beat click
                case <= Conductor.perfectTiming:
                    Debug.Log("PERFECT ! ");
                    ScoreCombo.Instance.health += 10;
                    ScoreCombo.Instance.AddScore(300);
                    break;
                
                // Handle good beat click
                case <= Conductor.goodTiming:
                    Debug.Log("GOOD ! ");
                    ScoreCombo.Instance.health += 5;
                    ScoreCombo.Instance.AddScore(100);
                    break;
                
                case <= Conductor.missTiming:
                    MissHit();
                    break;
            }

            currentCircle.isHit = true;
        }

        public void MissHit()
        {
            Debug.Log("MISS ! ");
            ScoreCombo.Instance.health -= 10;
            ScoreCombo.Instance.AddScore(0);
        }
    }
}