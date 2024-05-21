using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenuAnimation : MonoBehaviour
    {
        public GameObject[] lanternPos;
        public GameObject[] buttonPos;

        public GameObject[] lanterns;
        public GameObject[] buttons;

        public float[] alpha;

        private int currentIndex;
        private int index;

        private Sequence animationSequence;
        private const float animationSpeed = 0.25f;

        private void Start()
        {
            OnMenuTurn(0);
        }

        private void OnMenuTurn(int axis)
        {
            animationSequence = DOTween.Sequence();
            currentIndex = (currentIndex + axis + 10) % 10;
            
            for (int i = 0; i < buttons.Length; i++)
            {
                index = (currentIndex + i + 10) % 10;

                buttons[i].SetActive(index is < 8 and > 1);
                lanterns[i].SetActive(index is < 8 and > 1);
                lanterns[i].GetComponent<Image>().color = new Color(alpha[index], alpha[index],alpha[index]);

                Tween positionTween = buttons[i].GetComponent<RectTransform>()
                    .DOAnchorPos(buttonPos[index].GetComponent<RectTransform>().localPosition, animationSpeed);
                Tween scaleTween = buttons[i].GetComponent<RectTransform>().
                    DOScale(buttonPos[index].GetComponent<RectTransform>().localScale, animationSpeed);

                animationSequence.Join(positionTween);
                animationSequence.Join(scaleTween);

                positionTween = lanterns[i].GetComponent<RectTransform>()
                    .DOAnchorPos(lanternPos[index].GetComponent<RectTransform>().localPosition, animationSpeed);
                scaleTween = lanterns[i].GetComponent<RectTransform>()
                    .DOScale(lanternPos[index].GetComponent<RectTransform>().localScale, animationSpeed);

                animationSequence.Join(positionTween);
                animationSequence.Join(scaleTween);
            }
            
            animationSequence.Play();
        }

        public void MenuInput(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            
            animationSequence.Kill();
            OnMenuTurn((int)ctx.ReadValue<float>());
        }
    }
}