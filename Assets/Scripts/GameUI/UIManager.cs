using PlayerRelated;
using UnityEngine;
using TMPro;

namespace GameUI
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI comboText;

        public SpriteRenderer clickLine;
        public Color[] colors;

        void Update()
        {
            comboText.text = PlayerManager.Instance.color.ToString();

            clickLine.color = colors[(int)PlayerManager.Instance.color];
        }
    }
}