using System;
using JMT.System.Card.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JMT.System.Card.Component
{
    public class CardClicker : MonoBehaviour, IPointerClickHandler, ICardComponent
    {
        public ICard Card { get; private set; }

        public event Action OnLeftButtonClick;
        public event Action OnRightButtonClick;

        public void Init(ICard card)
        {
            Card = card;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftButtonClick?.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightButtonClick?.Invoke();
            }
        }
    }
}