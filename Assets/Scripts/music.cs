using UnityEngine;

public class Music : MonoBehaviour
{
    public void OffMusic()
    {
        AudioManager.Instance.StopMusic("Theme");
    }
}
