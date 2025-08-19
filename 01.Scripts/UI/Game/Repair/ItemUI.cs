using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JMT.UISystem
{
    public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public event Action<bool> OnItemFocusEvent;
        public event Action OnItemClickEvent;

        [SerializeField] private Image outline;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemClickEvent?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            outline.enabled = true;
            OnItemFocusEvent?.Invoke(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            outline.enabled = false;
            OnItemFocusEvent?.Invoke(false);
        }
    }
}
