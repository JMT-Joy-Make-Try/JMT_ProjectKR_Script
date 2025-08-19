using JMT.UISystem;
using TMPro;
using UnityEngine;

namespace JMT.System.GameSystem.Timer
{
    public class TimerUI : FadeUI
    {
        [SerializeField] private TextMeshProUGUI timerText;

        public void UpdateTimerUI(int minutes, int seconds)
        {
            timerText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }
}
