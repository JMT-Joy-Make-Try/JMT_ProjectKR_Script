using System;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.UISystem.Start
{
    public class StartView : MonoBehaviour
    {
        public event Action OnStartEvent;
        public event Action OnOptionEvent;
        public event Action OnExitEvent;

        [SerializeField] private Button startButton, optionButton, exitButton;

        private void Awake()
        {
            startButton.onClick.AddListener(HandleStartButton);
            optionButton?.onClick.AddListener(HandleOptionButton);
            exitButton.onClick.AddListener(HandleExitButton);
        }

        private void HandleStartButton()
        {
            OnStartEvent?.Invoke();
        }

        private void HandleOptionButton()
        {
            OnOptionEvent?.Invoke();
        }

        private void HandleExitButton()
        {
            OnExitEvent?.Invoke();
        }
    }
}
