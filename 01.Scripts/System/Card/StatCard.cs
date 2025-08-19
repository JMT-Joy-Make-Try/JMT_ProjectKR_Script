using System;
using JMT.System.Card.CardData;
using JMT.System.DeckSettingSystem;
using UnityEngine;

namespace JMT.System.Card
{
    public class StatCard : BaseCard
    {
        public RectTransform RectTrm => transform as RectTransform;

        protected override void Start()
        {
            base.Start();
            CardVisual.SetVisual(CardData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if ((CardData as StatCardDataSO).IsCloned())
            {
                Destroy(CardData);
            }
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            OnCardMoveToDeck += HandleMoveToDeck;
            OnCardMoveToPocket += HandleMoveToPocket;
        }



        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            OnCardMoveToDeck -= HandleMoveToDeck;
            OnCardMoveToPocket -= HandleMoveToPocket;
        }

        public void SetStatCardData(StatCardDataSO cardData, bool isInit = true)
        {
            SetCardData(cardData);
            if (isInit)
                CardData.Init();
            CardVisual.SetVisual(cardData);
        }

        private void HandleMoveToPocket()
        {
            DeckSettingManager.Instance.RemoveStatCard(CardData as StatCardDataSO);
            DeckSettingManager.Instance.DeckSettingUI.MoveCardToDeckContent(this);
        }

        private void HandleMoveToDeck()
        {
            DeckSettingManager.Instance.AddStatCard(CardData as StatCardDataSO);
            DeckSettingManager.Instance.DeckSettingUI.MoveCardToStatContent(this);
        }
    }
}