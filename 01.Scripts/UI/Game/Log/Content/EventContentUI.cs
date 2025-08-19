using DG.Tweening;
using JMT.EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.UISystem.Content
{
    public class EventContentUI : ContentUI
    {
        [Header("Content Settings")]
        [SerializeField] private Image content;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descText;

        public void SetContentUI(Color contentColor, GameEventSO eventSO)
        {
            content.color = contentColor;
            if (eventSO.EventName != null && nameText != null)
                nameText.text = eventSO.EventName;
            if (eventSO.EventDesc != null && descText != null)
                descText.text = eventSO.EventDesc;
        }
    }
}
