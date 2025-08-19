using System;
using System.Collections.Generic;
using JMT.System.Card.CardData;
using JMT.System.Card.Component;
using JMT.System.Card.Interface;
using JMT.System.DeckSettingSystem;
using JMT.System.DeckSettingSystem.Filter;
using JMT.System.SoundSystem.Core;
using JMT.UISystem.Tooltip;
using UnityEngine;

namespace JMT.System.Card
{
    public abstract class BaseCard : MonoBehaviour, ICard, ITooltipable
    {
        private CanvasGroup _canvasGroup;

        public virtual CardDataSO CardData { get; protected set; }
        public CardTransform CardTransform { get; protected set; }
        public CardVisual CardVisual { get; private set; }
        public CardAnimator CardAnimator { get; private set; }
        public CardDragger CardDragger { get; private set; }
        public CardClicker CardClicker { get; private set; }
        public CardType CardType => _cardType;

        [SerializeField] private CardType _cardType;

        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public List<ItemTag> Tags { get; protected set; }
        [field: SerializeField] public string Desc { get; protected set; }
        [SerializeField] protected SoundPlayer soundPlayer;

        private bool _isInPocket = false;

        protected event Action OnCardMoveToDeck;
        protected event Action OnCardMoveToPocket;

        private FilterCard filterCard;
        public bool IsInPocket
        {
            get => _isInPocket;
            set
            {
                _isInPocket = value;
                if (_isInPocket)
                {
                    OnCardMoveToPocket?.Invoke();
                }
                else
                {
                    OnCardMoveToDeck?.Invoke();
                }
            }
        }

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            CardTransform = GetComponent<CardTransform>();
            CardVisual = GetComponent<CardVisual>();
            CardAnimator = GetComponent<CardAnimator>();
            CardDragger = GetComponent<CardDragger>();
            CardClicker = GetComponent<CardClicker>();
            filterCard = GetComponent<FilterCard>();
            CardVisual.Init(this);
        }

        protected virtual void Start()
        {
            CardTransform.Init(this);
            CardAnimator.Init(this);
            CardDragger?.Init(this);
            CardClicker?.Init(this);
            filterCard?.Init(this);
            AddEvent();
            CardData = Instantiate(CardData);
        }

        protected virtual void OnDestroy()
        {
            RemoveEvent();
        }

        protected virtual void AddEvent()
        {
            if (CardDragger != null)
            {
                CardDragger.OnCardDrag += HandleCardDrag;
            }
            if (CardClicker != null)
            {
                CardClicker.OnRightButtonClick += HandleRightClick;
                CardClicker.OnLeftButtonClick += HandleMovePos;
            }
        }

        protected virtual void RemoveEvent()
        {
            if (CardDragger != null)
            {
                CardDragger.OnCardDrag -= HandleCardDrag;
            }
            if (CardClicker != null)
            {
                CardClicker.OnRightButtonClick -= HandleRightClick;
                CardClicker.OnLeftButtonClick -= HandleMovePos;
            }
        }

        private void HandleMovePos()
        {
            if (!_isInPocket) return;
            DeckSettingManager.Instance.AddCard(CardData);
            OnCardMoveToDeck?.Invoke();
            _isInPocket = false;
        }

        private void HandleRightClick()
        {
            if (_isInPocket) return;
            DeckSettingManager.Instance.RemoveCard(CardData);
            OnCardMoveToPocket?.Invoke();
            _isInPocket = true;
        }

        private void HandleCardDrag(bool isDrag)
        {
            if (_canvasGroup == null) return;
            _canvasGroup.blocksRaycasts = !isDrag;
            _canvasGroup.alpha = isDrag ? 0.5f : 1f;
            soundPlayer?.PlaySound(isDrag ? "Card_Up" : "Card_Down", SoundType.SFX);
        }
        
        public void SetInteractable(bool isInteractable)
        {
            if (_canvasGroup == null) return;
            _canvasGroup.interactable = isInteractable;
            _canvasGroup.blocksRaycasts = isInteractable;
        }
        
        protected void SetCardData(CardDataSO cardData)
        {
            CardData = cardData;
            Name = cardData.Name;
            Desc = cardData.Description;
            Tags = cardData.Tags;
        }        
    }
}
