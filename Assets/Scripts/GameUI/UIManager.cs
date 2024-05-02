using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class UIManager : MonoBehaviour
    {
        public Slider progressBar;
    
        void Start()
        {
            progressBar.maxValue = Conductor.instance.songPositionInBeats[^1];
        }

        void Update()
        {
            progressBar.value = Conductor.instance.currentSongPositionInBeats;
        }
    }
}