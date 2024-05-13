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
            transform.Translate(Vector3.down * (Time.deltaTime * circleData.downSpeed));
            
            if (isHit)
            {
                Destroy(gameObject);
                Spawners.Instance.RemoveCircle(this);
            }
            if (transform.position.y <= -3f)
            {
                ScoreCombo.Instance.AddScore(0);
                
                Destroy(gameObject);
                Spawners.Instance.RemoveCircle(this);
            }
        }
    }
}