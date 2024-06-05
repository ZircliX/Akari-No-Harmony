using GamePlay;
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

        public SpriteRenderer[] epuisettes;
        public Sprite[] nets;
        public Sprite[] brokenNets;

        private void Update()
        {
            foreach (var sr in epuisettes)
            {
                sr.sprite = nets[PlayerManager.Instance.colorIndex];
            }

            healthBar.fillAmount = ScoreCombo.Instance.health / 100;

            scoreText.text = ScoreCombo.Instance.score.ToString();
            comboText.text = ScoreCombo.Instance.combo + "x";
        }
    }
}