using GamePlay;
using MapsGenerators;
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
            if (GameManager.Instance.state == GameManager.GameState.LevelInProgress)
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
}