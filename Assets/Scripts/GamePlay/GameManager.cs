using Audio;
using AudioDelegates;
using Menu;
using UnityEngine;

namespace GamePlay
{
    public class GameManager : MonoBehaviour
    {
        public Map level;
        
        public GameState state = GameState.None;
        public enum GameState
        {
            None = -1,
            GamePause = 10,
            LevelInProgress = 1,
            LevelFinished = 5,
            PlayerDead = 6
        }
    
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<GameManager>();
                    if (_instance == null)
                    {
                        GameObject gameManager = new GameObject("GameManager");
                        _instance = gameManager.AddComponent<GameManager>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void SwitchState(int newState)
        {
            state = (GameState)newState;
            CheckGameChange();
        }

        private void CheckGameChange()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1f;
        
            switch (state)
            {
                case GameState.LevelInProgress:
                    Debug.Log("Game started / resumed !");
                    
                    AudioManager.Instance.MusicResume();
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = false;
                    break;
            
                case GameState.LevelFinished:
                    Debug.Log("Level Complete");
       
                    Time.timeScale = 0;
                    MenuManager.Instance.ChangeState((int)MenuManager.MenuState.Completed);
                    break;
            
                case GameState.PlayerDead:
                    Debug.Log("Player Died");

                    AudioManager.Instance.MusicPause();
                    Time.timeScale = 0;
                    MenuManager.Instance.ChangeState((int)MenuManager.MenuState.Died);
                    break;

                case GameState.GamePause:
                    Debug.Log("Game is paused !");
                
                    AudioManager.Instance.MusicPause();
                    Time.timeScale = 0f;
                    break;
            }
        }
    }
}