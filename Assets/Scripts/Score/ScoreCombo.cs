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

        private void Start()
        {
            Instance = this;

            combo = 0;
            score = 0;
            health = 100;

        }

        private void Update()
        {
            health -= 5f * Time.deltaTime;
            health = Mathf.Clamp(health, 0, 100);
        }

        public void AddScore(int points)
        {
            if (points == 0)
            {
                combo = 0;
                return;
            }
            
            combo += 1;

            multi = multi >= 8 ? 8 : combo / 10;
            multi = Mathf.Clamp(multi, 1, 8);

            score += points * multi;
        }
    }
}