using Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button[] lvlButtons;
    
    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 1);

        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i >= levelAt) continue;
            
            //Switch from locked to enabled
            /*
            lvlButtons[i].Interactable(true);
            lvlButtons[i].enableIcon = false;
            lvlButtons[i].enableText = true;
            
            lvlButtons[i].navigationMode = Navigation.Mode.Horizontal;
            lvlButtons[i].useUINavigation = true;
            lvlButtons[i].AddUINavigation();
        
            lvlButtons[i].SetText((i + 1).ToString());
            lvlButtons[i].UpdateUI();
            */
        }
    }

    public void LoadLevel(int index)
    {
        GameManager.Instance.SwitchState(5);
        SceneManager.LoadScene(index+1);
    }
    
    public void GoBack(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        SceneManager.LoadScene(0);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MenuManager.Instance.SwitchState(0);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public void ResetLevelAt(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        PlayerPrefs.SetInt("levelAt", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}