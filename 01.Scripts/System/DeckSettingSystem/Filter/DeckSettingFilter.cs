using System;
using System.Collections.Generic;
using JMT.System.DeckSettingSystem.Filter;
using JMT.UISystem.Tooltip;
using UnityEngine;

namespace JMT.System.DeckSettingSystem
{
    public class DeckSettingFilter : MonoBehaviour
    {
        [SerializeField] private List<ItemFilter> itemFilters;
        [SerializeField] private List<CardFilter> cardFilters;

        private List<ItemTag> _itemTags;
        private List<CardType> _cardTypes;

        public event Action<List<ItemTag>, List<CardType>> OnFilterChanged;

        private void Start()
        {
            _itemTags = new List<ItemTag>();
            _cardTypes = new List<CardType>();

            foreach (var itemFilter in itemFilters)
            {
                itemFilter.OnFilterSelected += HandleItemFilterSelected;
                itemFilter.ResetItem();
            }

            foreach (var cardFilter in cardFilters)
            {
                cardFilter.OnFilterSelected += HandleCardFilterSelected;
                cardFilter.ResetItem();
            }
        }

        private void OnDestroy()
        {
            foreach (var itemFilter in itemFilters)
            {
                itemFilter.OnFilterSelected -= HandleItemFilterSelected;
            }

            foreach (var cardFilter in cardFilters)
            {
                cardFilter.OnFilterSelected -= HandleCardFilterSelected;
            }
        }

        private void HandleCardFilterSelected(CardType type, bool isSelected)
        {
            if (isSelected)
            {
                if (!_cardTypes.Contains(type))
                {
                    _cardTypes.Add(type);
                }
            }
            else
            {
                _cardTypes.Remove(type);
            }

            OnFilterChanged?.Invoke(_itemTags, _cardTypes);
        }

        private void HandleItemFilterSelected(ItemTag tag, bool isSelected)
        {
            if (isSelected)
            {
                if (!_itemTags.Contains(tag))
                {
                    _itemTags.Add(tag);
                }
            }
            else
            {
                _itemTags.Remove(tag);
            }

            OnFilterChanged?.Invoke(_itemTags, _cardTypes);
        }
    }
}