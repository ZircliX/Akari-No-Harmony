using PlayerRelated;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameUI
{
    public class UIManager : MonoBehaviour
    {
        public Slider progressBar;
        public TextMeshProUGUI comboText;
    
        void Start()
        {
            progressBar.maxValue = Conductor.Instance.circlesPositionInSeconds[^1];
        }

        void Update()
        {
            progressBar.value = Conductor.Instance.elapsedTime;
            comboText.text = PlayerManager.Instance.color.ToString();
        }
    }
}