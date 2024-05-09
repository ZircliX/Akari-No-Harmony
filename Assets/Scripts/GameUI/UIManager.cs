using PlayerRelated;
using Score;
using UnityEngine;
using TMPro;

namespace GameUI
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI comboText;
        
        public TextMeshProUGUI colorText;

        public SpriteRenderer clickLine;
        public Color[] colors;

        private void Update()
        {
            colorText.text = PlayerManager.Instance.color.ToString();

            clickLine.color = colors[(int)PlayerManager.Instance.color];

            scoreText.text = ScoreCombo.Instance.score.ToString();
            comboText.text = ScoreCombo.Instance.combo + "x";
        }
    }
}