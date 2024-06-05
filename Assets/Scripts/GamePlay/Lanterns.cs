using Audio;
using Dreamteck.Splines;
using GamePlay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lanterns
{
    public class Lanterns : MonoBehaviour
    {
        public float[] lanternsPos;
        public GameObject[] lanterns;
        public Color[] colors;

        private int currentIndex = 3;
        private int index;
        private const float animationSpeed = 4f;
        
        private int axis;

        void Start()
        {
            axis = 0;
        }

        private void Update()
        {
            if (GameManager.Instance.state == GameManager.GameState.LevelInProgress)
            {
                currentIndex = (currentIndex + axis + 9) % 9;

                for (int i = 0; i < lanterns.Length; i++)
                {
                    index = (currentIndex + i + 9) % 9;
                    if (index == 4) PlayerManager.Instance.colorIndex = (i +3) % 3;

                    lanterns[i].SetActive(index is < 9 and > 1);
                    var sr = lanterns[i].GetComponent<SpriteRenderer>();
                    sr.color = Color.Lerp(sr.color, colors[index], animationSpeed * Time.deltaTime * 2);

                    var spline = lanterns[i].GetComponent<SplineFollower>();
                    spline.SetPercent(Mathf.Lerp((float)spline.GetPercent(), lanternsPos[index], animationSpeed * Time.deltaTime));
                }

                axis = 0;
            }
        }

        public void ColorChangeLeft(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            axis = 1;
            AudioManager.Instance.PlaySFX("Switch");
        }

        public void ColorChangeRight(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            axis = -1;
            AudioManager.Instance.PlaySFX("Switch");
        }
    }
}