using Audio;
using AudioDelegates;
using DG.Tweening;
using GamePlay;
using GameUI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class MapSelect : MonoBehaviour
    {
       public Map map;

        public void OnClick()
        {
            GameManager.Instance.level = map;
            MenuManager.Instance.ChangeState(-1);
            LevelSelection.Instance.LoadLevel(1);
                
            AudioManager.Instance.PlaySFX("Select");
            
            AudioManager.Instance.StopSound();
        }
        
        public void OnHover()
        {
            DisplayMapInfos.Instance.UpdateUI(map);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameObject);
            
            AudioManager.Instance.PlaySFX("Hover");
        
            AudioManager.Instance.StopSound();
            AudioManager.Instance.PlaySound(JsonSystem.LoadAudioClip(Application.dataPath  + "/StreamingAssets/MapData/" + map.songData.songName + ".mp3"));
        }

    }
}