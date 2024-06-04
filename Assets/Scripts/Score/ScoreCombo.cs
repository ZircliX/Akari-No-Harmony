using GamePlay;
using UnityEngine;

namespace Score
{
    public class ScoreCombo : MonoBehaviour
    {
        public int score;
        public int combo;
        private int multi;

        public float health;

        public static ScoreCombo Instance;

        public int perfect, good, miss, maxCombo;

        private void Start()
        {
            Instance = this;

            combo = 0;
            score = 0;
            health = 100;

            maxCombo = 0;
        }

        private void Update()
        {
            health -= GameManager.Instance.level.mapDiff * Time.deltaTime;
            health = Mathf.Clamp(health, 0, 100);

            if (health <= 0 && GameManager.Instance.state != GameManager.GameState.PlayerDead)
            {
                GameManager.Instance.SwitchState(6);
            }
        }

        public void AddScore(int points)
        {
            switch (points)
            {
                case <= 0:
                    combo = 0;
                    miss++;
                    return;
                case 100:
                    good++;
                    break;
                case 300:
                    perfect++;
                    break;
            }

            combo += 1;
            if (combo > maxCombo) maxCombo = combo;

            multi = combo / 10;
            multi = Mathf.Clamp(multi, 1, 8);

            score += points * multi;
        }
    }
}