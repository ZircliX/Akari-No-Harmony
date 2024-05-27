using GamePlay;
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
        private bool isActive = true;
        
        [SerializeField] private GameObject[] panelList;
        [SerializeField] public GameObject[] defaultSelected;

        [SerializeField] private AudioMixerGroup[] audioSources;
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
            Pause = 10,
            Completed = 11,
            Died = 12,
            LevelSelection = 20
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

            for (int i = 0; i < 2; i++)
            {
                audioSources[i].audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("Volume" + i, 0f));
                audioSliders[i].value = PlayerPrefs.GetFloat("Volume" + i, 0f);
            }
        }

        public void Play()
        {
            SceneManager.LoadScene(1);
            ChangeState((int)MenuState.LevelSelection);
        }

        /*
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SwitchState(0);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        */

        public void Quit()
        {
            Application.Quit();
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene(0);
            //SceneManager.sceneLoaded += OnSceneLoaded;
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
            }
        
            panelList[(int)state].SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultSelected[(int)state]);
        }

        public void OpenPause(InputAction.CallbackContext context)
        {
            if (!context.performed || GameManager.Instance.state != GameManager.GameState.LevelInProgress) return;
        
            ChangeState((int)MenuState.Pause);
            GameManager.Instance.SwitchState(1);
        }

        public void GoBack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (state is MenuState.Options or MenuState.Credits)
            {
                ChangeState(lastStateIndex);
            }
        }

        public void UpdateSound(int index)
        {
            if (!isActive) return;
            
            audioSources[index].audioMixer.SetFloat("volume", audioSliders[index].value);
            PlayerPrefs.SetFloat("Volume" + index, audioSliders[index].value);
        }
        
        public void VolumeState(int index)
        {
            isActive = !isActive;
            audioSources[index].audioMixer.SetFloat("volume", isActive ? PlayerPrefs.GetFloat("Volume" + index, audioSliders[index].value) : -80);
        }
        
        public void SetFullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    
        public void Retry()
        {
            //SwitchState(MenuState.None);
            //GameManager.Instance.SwitchState(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}