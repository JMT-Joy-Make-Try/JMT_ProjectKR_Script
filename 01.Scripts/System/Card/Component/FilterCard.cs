using System;
using System.Collections.Generic;
using JMT.System.Card;
using JMT.System.Card.Interface;
using JMT.System.DeckSettingSystem.Filter;
using JMT.UISystem.Tooltip;
using UnityEngine;

namespace JMT.System.DeckSettingSystem
{
    public class FilterCard : MonoBehaviour, ICardComponent
    {
        public ICard Card { get; private set; }

        public void Init(ICard card)
        {
            Card = card;
        }

        private void Start()
        {
            DeckSettingManager.Instance.DeckSettingFilter.OnFilterChanged += HandleFilterChanged;
        }

        private void OnDestroy()
        {
            if (DeckSettingManager.Instance == null) return;
            DeckSettingManager.Instance.DeckSettingFilter.OnFilterChanged -= HandleFilterChanged;
        }

        private void HandleFilterChanged(List<ItemTag> itemTags, List<CardType> cardTypes)
        {
            if (Card == null) return;
            if ((Card as BaseCard).IsInPocket) return;

            bool tagMatch = itemTags.Count == 0 || Card.CardData.Tags.Exists(tag => itemTags.Contains(tag));
            bool typeMatch = cardTypes.Count == 0 || cardTypes.Contains(Card.CardType);

            bool isVisible = tagMatch && typeMatch;

            Card.CardVisual.gameObject.SetActive(isVisible);
        }

    }
}