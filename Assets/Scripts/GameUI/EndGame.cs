using Score;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class EndGame : MonoBehaviour
    {
        private bool isDone;

        public TextMeshProUGUI perfect, good, miss, maxCombo;

        private void LateUpdate()
        {
            if (isDone) return;

            UpdateScoreUI();
            isDone = true;
        }

        private void UpdateScoreUI()
        {
            perfect.text = "Perfect : " + ScoreCombo.Instance.perfect;
            good.text = "Good : " + ScoreCombo.Instance.good;
            miss.text = "Miss : " + ScoreCombo.Instance.miss;
            maxCombo.text = "Max Combo : " + ScoreCombo.Instance.maxCombo;
        }
    }
}