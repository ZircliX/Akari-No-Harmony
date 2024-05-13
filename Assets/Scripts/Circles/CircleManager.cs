using Score;
using UnityEngine;

namespace Circles
{
    public class CircleManager : MonoBehaviour
    {
        public bool isHit;
        public Circle circleData;

        private void Update()
        {
            if (transform.position.y >= -6f)
            {
                transform.Translate(Vector3.down * (Time.deltaTime * circleData.downSpeed));
            }
            if (isHit)
            {
                Destroy(gameObject);
                Spawners.Instance.RemoveCircle(this);
            }
            if (transform.position.y <= -4.5f)
            {
                ScoreCombo.Instance.AddScore(0);
                ScoreCombo.Instance.health -= 0.1f;
                
                Destroy(gameObject);
                Spawners.Instance.RemoveCircle(this);
            }
        }
    }
}