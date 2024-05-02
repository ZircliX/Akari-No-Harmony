using UnityEngine;

namespace Circles
{
    [CreateAssetMenu(menuName = "CircleType")]
    public class CircleData : ScriptableObject
    {
        public new string name;
        public GameObject circlePrefab;
        public int index;
    }
}