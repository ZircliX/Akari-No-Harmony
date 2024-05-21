using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Animations;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] panelList;
        [SerializeField] private GameObject[] defaultSelected;

        [SerializeField] private AudioSource[] audioSources;
        [SerializeField] private Slider[] audioSliders;
    
        private MenuState state = MenuState.Menu;
        private int lastStateIndex;
        private enum MenuState
        {
            None = -1,
            Menu = 0,
            Options = 1,
            Credit = 2,
            Pause = 3,
            Won = 5,
            Lost = 6,
            LevelSelection = 10
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
            

            /*
            for (int i = 0; i < 2; i++)
            {
                audioSources[i].volume = PlayerPrefs.GetFloat("Volume" + i, 0.5f);
                audioSliders[i].value = PlayerPrefs.GetFloat("Volume" + i, 0.5f);
            }
            */
        }

        public void Play()
        {
            SceneManager.LoadScene(1);
            SwitchState(10);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SwitchState(0);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene(0);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void SwitchState(int newState)
        {
            lastStateIndex = (int)state;
            state = (MenuState)newState;
            CheckStateChange();
        }

        private void CheckStateChange()
        {
            foreach (GameObject panel in panelList)
            {
                panel.SetActive(false);
            }

            switch (state)
            {
                case MenuState.Menu:
                    GameManager.Instance.SwitchState(0);
                    break;
                case MenuState.Options:
                    break;
                case MenuState.Pause:
                    break;
                case MenuState.Lost:
                    break;
                case MenuState.Won:
                    break;
                case MenuState.None:
                    GameManager.Instance.SwitchState(5);
                    return;
                case MenuState.LevelSelection:
                    GameManager.Instance.SwitchState(0);
                    return;
            }
        
            panelList[(int)state].SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultSelected[(int)state]);
        }

        public void OpenPause(InputAction.CallbackContext context)
        {
            if (!context.performed || GameManager.Instance.state != GameManager.GameState.LevelInProgress) return;
        
            SwitchState(2);
            GameManager.Instance.SwitchState(1);
        }

        public void GoBack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (state == MenuState.Options)
            {
                SwitchState(lastStateIndex);
            }
        }

        public void UpdateSound(int index)
        {
            audioSources[index].volume = audioSliders[index].value;
            PlayerPrefs.SetFloat("Volume" + index, audioSliders[index].value);
        }
    
        public void Retry()
        {
            SwitchState(-1);
            GameManager.Instance.SwitchState(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    
        public void Lobby()
        {
            if (SceneManager.GetActiveScene().buildIndex == 4) return;
        
            SwitchState(-1);
            GameManager.Instance.SwitchState(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public Animator anim;

       /* public void Animate()
        {
            this.GetComponent<Animator>().SetTrigger("isHover");
        }*/
       
       Vector3 originalPos;

      


       public void ResetAfter()
       {
           gameObject.transform.position = originalPos;
       }
    }
}