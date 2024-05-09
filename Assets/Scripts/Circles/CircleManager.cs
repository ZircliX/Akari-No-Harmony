using Score;
using UnityEngine;

namespace Circles
{
    public class CircleManager : MonoBehaviour
    {
        public bool isHit;
        public Circle circleData;

        void Update()
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
                ScoreCombo.Instance.MissedHit();
                
                Destroy(gameObject);
                Spawners.Instance.RemoveCircle(this);
            }
        }
    }
}