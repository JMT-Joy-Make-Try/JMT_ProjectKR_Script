using System;
using JMT.UISystem.Tooltip;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JMT.System.DeckSettingSystem.Filter
{
    public class ItemFilter : FilterItem
    {
        [SerializeField] private ItemTag itemTag;
        public event Action<ItemTag, bool> OnFilterSelected;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnFilterSelected?.Invoke(itemTag, _isSelected);
        }
    }
}