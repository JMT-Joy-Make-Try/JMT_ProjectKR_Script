using System;
using System.Collections;
using JMT.Core.Tool;
using UnityEngine;

namespace JMT.UISystem.Message
{
    public class MessageController : MonoBehaviour
    {
        [SerializeField] private MessageView view;
        [SerializeField] private float displayDuration = 2f;

        public void ShowMessage(string message)
        {
            view.SetMessage(message);
            view.OpenPanel();
            StartCoroutine(WaitForClose());
        }

        private IEnumerator WaitForClose()
        {
            yield return WaitForSecondsCache.Get(displayDuration);
            view.ClosePanel();
        }
    }
}