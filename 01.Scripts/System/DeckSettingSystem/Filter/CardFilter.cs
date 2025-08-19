using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JMT.System.DeckSettingSystem.Filter
{
    public class CardFilter : FilterItem
    {
        [SerializeField] private CardType cardType;
        public event Action<CardType, bool> OnFilterSelected;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnFilterSelected?.Invoke(cardType, _isSelected);
        }
    }

    public enum CardType
    {
        Skill,
        Stat
    }
}