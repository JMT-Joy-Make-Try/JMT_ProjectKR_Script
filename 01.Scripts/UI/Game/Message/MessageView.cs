using TMPro;
using UnityEngine;

namespace JMT.UISystem.Message
{
    public class MessageView : FadeUI
    {
        [SerializeField] private TextMeshProUGUI messageText;

        public void SetMessage(string message)
        {
            messageText.text = message;
        }
    }
}