using UnityEngine;

namespace Score
{
    public class ScoreCombo : MonoBehaviour
    {
        public int score;
        public int combo;
        private int multi;

        public static ScoreCombo Instance;
    
        void Start()
        {
            Instance = this;

            combo = 0;
            score = 0;
        }

        public void AddScore(int points)
        {
            combo += 1;

            multi = multi >= 8 ? 8 : combo / 10;

            score += points * multi;
        }

        public void MissedHit()
        {
            combo = 0;
        }
    }
}