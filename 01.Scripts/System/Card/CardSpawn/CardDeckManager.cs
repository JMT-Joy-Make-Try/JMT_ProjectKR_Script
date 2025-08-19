using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JMT.Core;
using JMT.System.Card.CardData;
using JMT.System.Card.Interface;
using JMT.System.DataSystem;
using JMT.System.EventChannelSystem;
using JMT.UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.System.Card.CardSpawn
{
    public class CardDeckManager : MonoSingleton<CardDeckManager>
    {
        [SerializeField] private List<RectTransform> cardTrm = new();
        [SerializeField] private CardSetttingsSO cardSettings;

        [Header("UI Settings")]
        [SerializeField] private Button openButton;
        [SerializeField] private StatCard statCardPrefab;

        [Header("Tutorial")]
        [SerializeField] private bool isTutorial = false;
        [SerializeField] private EventsChannelSO eventsChannelSO;
        private bool _isInvoked = false;

        private DeckSpawnModule spawnModule;
        private Stack<StatCardDataSO> currentCardDatas = new();
        private Stack<StatCard> currentCards = new();

        protected override void Awake()
        {
            base.Awake();
            spawnModule = new(DataManager.Instance);
            openButton.onClick.AddListener(HandleOpenButtonClick);
            StartDeckSpawn();
        }

        private void Start()
        {
            UIManager.Instance.DeckCompo.OnOpenPanelEvent += StartDeckSpawn;
        }

        private void OnDestroy()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.DeckCompo.OnOpenPanelEvent -= StartDeckSpawn;
            }
            openButton.onClick.RemoveListener(HandleOpenButtonClick);
        }

        public void StartDeckSpawn()
        {
            if (currentCards.Count > 0)
            {
                foreach (var card in currentCards)
                {
                    Destroy(card.gameObject);
                }
                currentCards.Clear();
            }
            currentCardDatas = spawnModule.SpawnDeck(cardSettings.CardCount);
        }

        private void HandleOpenButtonClick()
        {
            if (!_isInvoked && isTutorial)
            {
                _isInvoked = true;
                eventsChannelSO.Invoke(EventChannelSystem.EventType.CardDraw);
            }
            for (int i = 0; i < cardSettings.SpawnCount; ++i)
            {
                if (i < currentCardDatas.Count)
                {
                    var statCard = Instantiate(statCardPrefab, openButton.transform.position, Quaternion.identity);
                    var card = currentCardDatas.Peek();
                    Debug.Log($"CardDeckManager.HandleOpenButtonClick: Spawning card {card.name} at position {openButton.transform.position}");
                    statCard.SetStatCardData(card);
                    statCard.CardVisual.FlipCard(true);
                    currentCards.Push(statCard);
                    currentCardDatas.Pop();
                    statCard.transform.name = i.ToString();
                }
            }
            SetCardPosition();
        }

        public void RemoveCard()
        {
            currentCards?.Pop();
            SetCardPosition();
        }

        private void SetCardPosition()
        {
            StatCard[] currentCards = this.currentCards.ToArray();
            int currentCardsCount = currentCards.Length;
            currentCards.Reverse();
            

            for (int i = currentCardsCount - 1; i >= 0; --i)
            {
                currentCards[i].CardTransform.SetParent(
                    cardTrm[Mathf.Clamp(i, 0, cardSettings.SpawnCount - 1)]);
                currentCards[i].RectTrm
                    .DOMove(currentCards[i].transform.parent.position, 0.5f);
                currentCards[i].CardDragger.SetDraggable(i == 0);
            }
        }

    }
}
