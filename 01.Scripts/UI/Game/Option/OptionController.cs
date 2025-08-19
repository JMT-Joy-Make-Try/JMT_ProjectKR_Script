using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JMT.UISystem.Option
{
    public class OptionController : MonoBehaviour
    {
        [SerializeField] private OptionView view;

        private void Awake()
        {
            view.OnOptionEvent += OpenPanel;
            view.OnGameEvent += ClosePanel;
            view.OnRestartEvent += HandleRestartEvent;
            view.OnTitleEvent += HandleTitleEvent;
        }

        public void OpenPanel()
        {
            view.OpenPanel();
            Time.timeScale = 0;
        }

        public void ClosePanel()
        {
            view.ClosePanel();
            Time.timeScale = 1;
        }

        private void HandleRestartEvent()
        {
            // �� ���ε�?
            Time.timeScale = 1;
            SceneManager.LoadScene("JSY");
        }

        private void HandleTitleEvent()
        {
            // Ÿ��Ʋ ������ �̵�
            Time.timeScale = 1;
            SceneManager.LoadScene("Title");
        }
    }
}
