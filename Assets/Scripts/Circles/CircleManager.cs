using AudioDelegates;
using GamePlay;
using PlayerRelated;
using UnityEngine;

namespace Circles
{
    public class CircleManager : MonoBehaviour
    {
        public bool isHit;
        private bool canHit = true;
        public Circle circleData;

        private void Update()
        {
            transform.Translate(Vector3.down * (Time.deltaTime * circleData.downSpeed));
            
            if (isHit)
            {
                Spawners.Instance.RemoveCircle(this, 0f);
            }
            if (transform.position.y <= -4.25f && canHit)
            {
                canHit = false;
                PlayerManager.Instance.Hit(-10, -10);
                
                Spawners.Instance.RemoveCircle(this, 3f);
            }
        }
    }
}