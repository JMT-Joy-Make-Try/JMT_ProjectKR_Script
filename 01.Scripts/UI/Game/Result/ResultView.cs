using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.UISystem.Result
{
    public class ResultView : FadeUI
    {
        public event Action OnTitleEvent;
        [SerializeField] private Button titleButton;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI waveText;

        private void Awake()
        {
            titleButton.onClick.AddListener(HandleTitleButton);
        }

        private void HandleTitleButton()
        {
            OnTitleEvent?.Invoke();
        }

        public void SetPriceText(int price)
        {
            priceText.text = $"{price}냥";
        }
        
        public void SetWaveText(int wave)
        {
            waveText.text = $"웨이브 {wave}";
        }
    }
}
