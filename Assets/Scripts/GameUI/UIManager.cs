using PlayerRelated;
using Score;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GameUI
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI comboText;

        public Image healthBar;

        public SpriteRenderer clickLine;
        public Color[] colors;

        private void Update()
        {
            clickLine.color = colors[(int)PlayerManager.Instance.color];

            healthBar.fillAmount = ScoreCombo.Instance.health / 100;

            scoreText.text = ScoreCombo.Instance.score.ToString();
            comboText.text = ScoreCombo.Instance.combo + "x";
        }
    }
}