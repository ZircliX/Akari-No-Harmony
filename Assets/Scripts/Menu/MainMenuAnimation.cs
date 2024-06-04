using System.Collections;
using Audio;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenuAnimation : MonoBehaviour
    {
        public float[] lanternsPos;
        public float[] lanternScale;
        public Color[] lanternColor;
        public GameObject[] lanterns;

        private int currentIndex;
        private int index;
        private int buttonSelected;

        private const float animationSpeed = 4f;

        private bool canTurn = true;
        private int axis;
        
        private IEnumerator Timeout()
        {
            canTurn = false;
            yield return new WaitForSeconds(0.1f);
            canTurn = true;
        }

        private void Update()
        {
            currentIndex = (currentIndex + axis + 10) % 10;
            
            for (int i = 0; i < lanterns.Length; i++)
            {
                index = (currentIndex + i + 10) % 10;
                if (index == 4) buttonSelected = i;
                
                lanterns[i].SetActive(index is < 8 and > 1);
                
                var spline = lanterns[i].GetComponent<SplineFollower>();
                spline.SetPercent(Mathf.Lerp((float)spline.GetPercent(), lanternsPos[index], animationSpeed * Time.deltaTime));
                
                var sr = lanterns[i].GetComponent<Image>();
                sr.color = Color.Lerp(sr.color, lanternColor[index], animationSpeed * Time.deltaTime * animationSpeed);

                var scale = lanterns[i].GetComponent<RectTransform>();
                scale.localScale = Vector3.Lerp(scale.localScale, new Vector3(lanternScale[index], lanternScale[index], lanternScale[index]), animationSpeed * Time.deltaTime);
            }
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(lanterns[buttonSelected]);
            MenuManager.Instance.defaultSelected[0] = lanterns[buttonSelected];
            
            axis = 0;
        }

        public void MenuInput(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed || MenuManager.Instance.state != MenuManager.MenuState.Main) return;
            if (!canTurn) return;
            StartCoroutine(Timeout());

            axis = (int)ctx.ReadValue<float>();
            AudioManager.Instance.PlaySFX("Hover");
        }
    }
}