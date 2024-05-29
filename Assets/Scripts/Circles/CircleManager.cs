using AudioDelegates;
using GamePlay;
using PlayerRelated;
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
            if (transform.position.y <= -5f)
            {
                PlayerManager.Instance.Hit(-10, -10);
                
                Destroy(gameObject);
                Spawners.Instance.RemoveCircle(this);
            }
        }
    }
}