using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        private int buttonSelected;

        private Sequence animationSequence;
        private const float animationSpeed = 0.25f;
        private bool canTurn = true;
        
        private IEnumerator Timeout()
        {
            canTurn = false;
            yield return new WaitForSeconds(0.1f);
            canTurn = true;
        }

        private void OnMenuTurn(int axis)
        {
            StartCoroutine(Timeout());
            
            animationSequence = DOTween.Sequence();
            currentIndex = (currentIndex + axis + 10) % 10;
            
            for (int i = 0; i < buttons.Length; i++)
            {
                index = (currentIndex + i + 10) % 10;
                if (index == 4) buttonSelected = i;

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
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(buttons[buttonSelected]);
            MenuManager.Instance.defaultSelected[0] = buttons[buttonSelected];
            
            animationSequence.Play();
        }

        public void MenuInput(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed || MenuManager.Instance.state != MenuManager.MenuState.Main) return;
            if (!canTurn) return;
            
            OnMenuTurn((int)ctx.ReadValue<float>());
            AudioManager.Instance.PlaySFX("menuSwitch");
        }
    }
}