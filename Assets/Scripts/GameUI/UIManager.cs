using Dreamteck.Splines;
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
        public SplineFollower sparkes;

        public SpriteRenderer[] epuisettes;
        private Sprite[,] netSprites;
        public Sprite[] nets;
        public Sprite[] brokenNets;

        public static UIManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            netSprites = new Sprite[3, 2]; // 3 colors, 2 states (unbroken, broken)
            
            netSprites[0, 0] = nets[0];
            netSprites[1, 0] = nets[1];
            netSprites[2, 0] = nets[2];
            
            netSprites[0, 1] = brokenNets[0];
            netSprites[1, 1] = brokenNets[1];
            netSprites[2, 1] = brokenNets[2];
        }

        private void Update()
        {
            UpdateNetSprites(PlayerManager.Instance.colorIndex);

            healthBar.fillAmount = ScoreCombo.Instance.health / 100;
            sparkes.SetPercent(healthBar.fillAmount);

            scoreText.text = ScoreCombo.Instance.score.ToString();
            comboText.text = ScoreCombo.Instance.combo + "x";
        }

        private void UpdateNetSprites(int index)
        {
            for (var i = 0; i < epuisettes.Length; i++)
            {
                int stateIndex = PlayerManager.Instance.brokenIndex[i] ? 1 : 0;
                
                var sr = epuisettes[i];
                sr.sprite = netSprites[index, stateIndex];
            }
        }
    }
}