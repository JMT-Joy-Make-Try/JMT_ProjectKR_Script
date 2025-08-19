using JMT.EventSystem;
using TMPro;
using UnityEngine;

namespace JMT.UISystem.Content
{
    public class WaveContentUI : ContentUI
    {
        [Header("Content Settings")]
        [SerializeField] private TextMeshProUGUI waveText;

        public void SetContentUI(int wave)
        {
            if (waveText != null)
                waveText.text = $"¿þÀÌºê {wave}";
        }
    }
}
