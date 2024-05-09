using Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameState state = GameState.InMenu;
    public enum GameState
    {
        InMenu = 0,
        GamePause = 1,
        LevelInProgress = 5,
        LevelFinished = 6,
        PlayerDead = 10
    }
    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Optionally, find the GameManager object in the scene, if it's not already set.
                _instance = FindAnyObjectByType<GameManager>();
                if (_instance == null)
                {
                    // Create a new GameObject with a GameManager component if none exists.
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
            Destroy(gameObject); // Ensure there's only one instance by destroying duplicates.
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Optionally, make the GameManager persist across scenes.
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
                
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
                break;
            
            case GameState.LevelFinished:
                Debug.Log("Enemies killed go next !");
                
                Time.timeScale = 0f;
                MenuManager.Instance.SwitchState(3);
                
                if (PlayerPrefs.GetInt("levelAt", 1) < SceneManager.GetActiveScene().buildIndex)
                    PlayerPrefs.SetInt("levelAt", SceneManager.GetActiveScene().buildIndex);
                break;
            
            case GameState.PlayerDead:
                Debug.Log("Player dead go menu !");
                
                Time.timeScale = 0f;
                MenuManager.Instance.SwitchState(4);
                break;
            
            case GameState.InMenu:
                Debug.Log("Game is in menu !");
                break;
            
            case GameState.GamePause:
                Debug.Log("Game is paused !");
                
                Time.timeScale = 0f;
                break;
        }
    }
    
    public void HandleReset(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}