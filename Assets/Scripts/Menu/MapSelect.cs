using Audio;
using AudioDelegates;
using GamePlay;
using GameUI;
using MapsGenerators;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MapSelect : MonoBehaviour
    {
       public Map map;
       public int mapIndex;

       private AudioClip clip;

        public void OnClick()
        {
            GameManager.Instance.level = map;
            GameManager.Instance.levelIndex = mapIndex;
            GameManager.Instance.levelSong = LevelSelection.clipsList[mapIndex];
            
            MenuManager.Instance.ChangeState(-1);
            SceneManager.LoadScene(1);
                
            AudioManager.Instance.PlaySFX("Select");
            AudioManager.Instance.StopSound();
        }
        
        public void OnHover()
        {
            Selected();
        }

        public void OnSelect()
        {
            Selected();
        }

        private void Selected()
        {
            DisplayMapInfos.Instance.UpdateUI(map);
            
            AudioManager.Instance.PlaySFX("Hover");
            AudioManager.Instance.StopSound();

            clip = LevelSelection.clipsList[mapIndex];
            AudioManager.Instance.PlaySound(clip);
        }
    }
}