using Circles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerRelated
{
    public class PlayerManager : MonoBehaviour
    {
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

        public void ChangeBanner(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed) return;
            
            colorSwitch = (int)context.ReadValue<float>();
            color = (ColorIndex) (int)color + colorSwitch;

            if ((int)color >= 3) color = 0;
            if ((int)color < 0) color = (ColorIndex)2;
        }
        
        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) OnBeatClick(10);
        }

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

        private void OnBeatClick(int clickIndex)
        {
            // Calculate the timing difference between the click and the actual beat
            float timingDifference = Conductor.Instance.OnBeatClick();
            var currentCircle = Spawners.Instance.spawnedCircles.Peek();

            bool validInput = currentCircle.columnIndex != clickIndex;

            // Check if the click was close enough to be considered successful
            switch (timingDifference)
            {
                // Handle Perfect beat click
                case <= Conductor.perfectTiming:
                    Debug.Log("PERFECT ! ");
                    Spawners.Instance.spawnedCircles.Dequeue();
                    break;
                
                // Handle good beat click
                case <= Conductor.goodTiming:
                    Debug.Log("GOOD ! ");
                    Spawners.Instance.spawnedCircles.Dequeue();
                    break;
                
                default:
                {
                    // Handle missed beat click
                    if (timingDifference > Conductor.missTiming || !validInput)
                    {
                        Debug.Log("MISS ! ");
                        Spawners.Instance.spawnedCircles.Dequeue();
                    }
                    break;
                }
            }
        }
    }
}