using UnityEngine;

namespace Circles
{
    public class CircleManager : MonoBehaviour
    {
        public bool isHit = false;
        public Circle circleData;

        void Update()
        {
            if (transform.position.y >= -4.5f)
            {
                transform.Translate(Vector3.down * (Time.deltaTime * circleData.downSpeed));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}