using AudioDelegates;
using GamePlay;
using Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{
    private static MapSelect selectedObject;

    private bool isSelected;
    public Map map;

    public void OnClick()
    {
        DisplayMapInfos.Instance.UpdateUI(map);

        if (isSelected)
        {
            GameManager.Instance.level = map;
            MenuManager.Instance.ChangeState(-1);
            LevelSelection.Instance.LoadLevel(1);
            isSelected = false;
        }
        else
        {
            if (selectedObject != null)
            {
                selectedObject.isSelected = false;
            }
            
            isSelected = true;
            selectedObject = this;
            Selected();
        }
    }

    private void Selected()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}