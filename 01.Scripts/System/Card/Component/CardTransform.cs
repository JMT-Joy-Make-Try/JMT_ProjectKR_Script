using System;
using JMT.System.Card.Interface;
using UnityEngine;

namespace JMT.System.Card.Component
{
    public class CardTransform : MonoBehaviour, ICardComponent
    {
        public ICard Card { get; private set; }
        public CardSlot CurrentSlot { get; private set; }
        public RectTransform CurParent { get; private set; }
        public RectTransform GetRectTransform() => transform as RectTransform;
        public event Action OnParentChange;
        
        public void Init(ICard card)
        {
            Card = card;
            CurParent = transform.parent as RectTransform;
        }

        public void SetParent(Transform parent, bool isInvokeEvent = true, bool isChangeCurParent = true)
        {
            transform.SetParent(parent);
            if (isChangeCurParent)
                CurParent = parent as RectTransform;
            if (isInvokeEvent)
                OnParentChange?.Invoke();
        }
        
        public void SetSlot(CardSlot slot)
        {
            CurrentSlot = slot;
            if (slot != null)
            {
                SetParent(slot.GetRectTransform());
            }
            else
            {
                SetParent(null);
            }
        }
    }
}