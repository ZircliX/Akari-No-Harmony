using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerRelated
{
    public class PlayerManager : MonoBehaviour
    {
        private ClickIndex click;
        private enum ClickIndex
        {
            left = -1,
            middle = 0,
            right = 1
        }
        
        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) OnBeatClick(10);
        }

        public void LeftClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) OnBeatClick(-1);
        }
        public void MiddleClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) OnBeatClick(0);
        }
        public void RightClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) OnBeatClick(1);
        }

        private void OnBeatClick(int clickIndex)
        {
            // Calculate the timing difference between the click and the actual beat
            float timingDifference = Conductor.instance.OnBeatClick();

            // Check if the click was close enough to be considered successful
            switch (timingDifference)
            {
                case <= Conductor.perfectTiming:
                    Debug.Log("PERFECT ! ");
                    // Handle Perfect beat click

                    break;

                case <= Conductor.goodTiming:
                    Debug.Log("GOOD ! ");
                    // Handle good beat click

                    break;

                case > Conductor.missTiming:
                    Debug.Log("MISS ! ");
                    // Handle missed beat click

                    break;
            }

            Debug.Log(timingDifference);
        }
    }
}