using AudioDelegates;
using TMPro;
using UnityEngine;

public class DisplayMapInfos : MonoBehaviour
{
    public TextMeshProUGUI diff, length, numCircles;

    public static DisplayMapInfos Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void UpdateUI(Map map)
    {
        diff.text = "Difficulty : " + map.mapDiff;
        //length.text = "Map Length : " + map.songData.songBPM;
        numCircles.text = "Total Circles : " + map.circles.Count;
    }
}
