using Audio;
using GamePlay;
using GameUI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        private bool[] isActive;
        
        [SerializeField] private GameObject[] panelList;
        [SerializeField] public GameObject[] defaultSelected;

        [SerializeField] private AudioMixer[] audioMixers;
        [SerializeField] private Slider[] audioSliders;
    
        private int lastStateIndex;
        public MenuState state = MenuState.Main;
        public enum MenuState
        {
            None = -1,
            Main = 0,
            Options = 1,
            Credits = 2,
            Editor = 3,
            LevelSelection = 4,
            Pause = 5,
            Completed = 6,
            Died = 7,
        }
    
        private static MenuManager _instance;
        public static MenuManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Optionally, find the GameManager object in the scene, if it's not already set.
                    _instance = FindAnyObjectByType<MenuManager>();
                    if (_instance == null)
                    {
                        // Create a new GameObject with a GameManager component if none exists.
                        GameObject menuManager = new GameObject("MenuManager");
                        _instance = menuManager.AddComponent<MenuManager>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject); // Ensure there's only one instance by destroying duplicates.
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject); // Optionally, make the GameManager persist across scenes.
            }
        }

        private void Start()
        {
            CheckStateChange();

            isActive = new []{false, false};
            
            for (int i = 0; i < 2; i++)
            {
                audioMixers[i].SetFloat("volume", PlayerPrefs.GetFloat("Volume" + i, 0f));
                audioSliders[i].value = PlayerPrefs.GetFloat("Volume" + i, 0f);
            }
        }

        public void ResumeMusic()
        {
            AudioManager.Instance.MusicResume();
        }
        
        private void OnSceneLoadedMenu(Scene scene, LoadSceneMode mode)
        {
            ChangeState(4);
            SceneManager.sceneLoaded -= OnSceneLoadedMenu;
        }
        
        public void GoToMenu()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            SceneManager.sceneLoaded += OnSceneLoadedMenu;
        }
        
        public void Quit()
        {
            Application.Quit();
        }

        public void ChangeState(int newState)
        {
            lastStateIndex = (int)state;
            state = (MenuState)newState;
            CheckStateChange();
        }

        private void CheckStateChange()
        {
            foreach (var panel in panelList)
            {
                panel.SetActive(false);
            }

            switch (state)
            {
                case MenuState.Main:
                    GameManager.Instance.SwitchState(-1);
                    break;
                case MenuState.None:
                    GameManager.Instance.SwitchState(1);
                    return;
                case MenuState.Died:
                    SceneManager.LoadScene(2, LoadSceneMode.Single);
                    break;
            }
            
            panelList[(int)state].SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultSelected[(int)state]);
        }

        public void OpenPause(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (state is not MenuState.None) return;
            
            ChangeState((int)MenuState.Pause);
            GameManager.Instance.SwitchState(10);
        }
        
        public void GoBack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if ((int)state is > 1 and < 6)
            {
                ChangeState(lastStateIndex);
            }
        }

        public void UpdateSound(int index)
        {
            if (!isActive[index]) return;
            
            audioMixers[index].SetFloat("volume", audioSliders[index].value);
            PlayerPrefs.SetFloat("Volume" + index, audioSliders[index].value);
        }
        
        public void VolumeState(int index)
        {
            isActive[index] = !isActive[index];
            audioMixers[index].SetFloat("volume", isActive[index] ? PlayerPrefs.GetFloat("Volume" + index, audioSliders[index].value) : -80);
        }
        
        public void SetFullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        
        private void OnSceneLoadedRetry(Scene scene, LoadSceneMode mode)
        {
            GameManager.Instance.level = LevelSelection.orderedMaps[GameManager.Instance.levelIndex];
            GameManager.Instance.levelSong = LevelSelection.clipsList[GameManager.Instance.levelIndex];
            
            ChangeState(-1);
            AudioManager.Instance.StopSound();
            SceneManager.sceneLoaded -= OnSceneLoadedRetry;
        }
        
        public void Retry()
        {
            GoToMenu();
            
            SceneManager.LoadScene(1);
            SceneManager.sceneLoaded += OnSceneLoadedRetry;
        }
    }
}