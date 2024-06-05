using AudioDelegates;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class DisplayMapInfos : MonoBehaviour
    {
        public TextMeshProUGUI diff, numCircles, bpm, length;

        public static DisplayMapInfos Instance;
        private void Awake()
        {
            Instance = this;
        }

        public void UpdateUI(Map map)
        {
            diff.text = "difficulty : " + map.mapDiff;
            numCircles.text = "Circles : " + map.circles.Count;
        
            bpm.text = "BPM : " + map.songData.songBPM;
        
            int totalSeconds = map.songData.songLength;
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            
            length.text = "Length : " + $"{minutes:D2}m{seconds:D2}s";
        }
    }
}