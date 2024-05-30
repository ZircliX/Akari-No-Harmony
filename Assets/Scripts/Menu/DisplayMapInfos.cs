using AudioDelegates;
using TMPro;
using UnityEngine;

namespace Menu
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
            diff.text = "Difficulté : " + map.mapDiff;
            numCircles.text = "Nombre Cercles : " + map.circles.Count;
        
            bpm.text = "BPM : " + map.songData.songBPM;
        
            int totalSeconds = map.songData.songLength;
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            
            length.text = "Durée : " + $"{minutes:D2}m{seconds:D2}s";
        }
    }
}