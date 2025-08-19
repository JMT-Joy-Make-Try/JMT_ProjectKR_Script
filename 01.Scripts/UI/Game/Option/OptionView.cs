using System;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.UISystem.Option
{
    public class OptionView : FadeUI
    {
        public event Action OnOptionEvent;
        public event Action OnGameEvent;
        public event Action OnRestartEvent;
        public event Action OnTitleEvent;

        [SerializeField] private Button optionButton;
        [SerializeField] private Button gameButton, restartButton, titleButton;

        private void Awake()
        {
            optionButton.onClick.AddListener(HandleOptionButton);
            gameButton.onClick.AddListener(HandleGameButton);
            restartButton.onClick.AddListener(HandleRestartButton);
            titleButton.onClick.AddListener(HandleTitleButton);
        }

        private void HandleOptionButton()
        {
            OnOptionEvent?.Invoke();
        }

        private void HandleGameButton()
        {
            OnGameEvent?.Invoke();
        }

        private void HandleRestartButton()
        {
            OnRestartEvent?.Invoke();
        }

        private void HandleTitleButton()
        {
            OnTitleEvent?.Invoke();
        }
    }
}
