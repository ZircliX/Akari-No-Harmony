using AudioDelegates;
using GamePlay;
using Menu;
using UnityEngine;

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
            LevelSelection.Instance.LoadLevel(2);
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
        }
    }
}