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
        
        public TextMeshProUGUI colorText;

        public Image clickLine;
        public Color[] colors;

        private void Update()
        {
            colorText.text = PlayerManager.Instance.color.ToString();

            clickLine.color = colors[(int)PlayerManager.Instance.color];

            scoreText.text = ScoreCombo.Instance.score.ToString();
            comboText.text = ScoreCombo.Instance.combo + "x";

            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, ScoreCombo.Instance.health, 2f);
        }
    }
}