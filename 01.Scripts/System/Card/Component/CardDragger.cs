using JMT.System.Card.Interface;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using UnityEngine;
using JMT.System.Card.CardSpawn;
using JMT.System.EventChannelSystem;

namespace JMT.System.Card.Component
{
    public class CardDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, ICardComponent
    {
        [SerializeField] protected bool _isDraggable = true;

        [Header("Tutorial")]
        [SerializeField] protected bool isTutorial = false;
        [SerializeField] protected EventsChannelSO eventsChannelSO;

        public event Action<bool> OnCardDrag;
        public event Action<bool> OnChangeDraggable;

        private RectTransform _rectTransform;
        private Canvas _parentCanvas;

        private Vector2 _originalPosition;
        private bool _isDragging;

        public ICard Card { get; private set; }

        public void Init(ICard card)
        {
            Card = card;
            _rectTransform = GetComponent<RectTransform>();
            _parentCanvas = GetComponentInParent<Canvas>();
            _originalPosition = _rectTransform.anchoredPosition;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!_isDraggable) return;
            if (_isDragging) return;
            _isDragging = true;
            RemoveCard();
            Card.CardTransform.SetParent(_parentCanvas.transform, false, false);
            _originalPosition = _rectTransform.anchoredPosition;
            OnCardDrag?.Invoke(true);
            Card.CardAnimator.ShakeCard();
        }

        public virtual void RemoveCard()
        {
            Card.CardTransform.CurrentSlot?.RemoveCard(Card as StatCard);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform,
                eventData.position,
                _parentCanvas.worldCamera,
                out var localPoint
            );
            _rectTransform.anchoredPosition = localPoint;
            Card.CardAnimator.ShakeCard(true);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDragging) return;
            _isDragging = false;

            // 전체 Raycast 결과 얻기
            var raycastResults = new List<RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, raycastResults);

            foreach (var result in raycastResults)
            {
                if (UseCard(result.gameObject)) return;
            }

            // 못 붙였으면 제자리로 돌아가기
            SetPosition();
            Card.CardTransform.SetParent(Card.CardTransform.CurParent);
            PlaceCard(Card.CardTransform.CurrentSlot);
        }


        protected virtual bool UseCard(GameObject rayResultGameObject)
        {
            if (rayResultGameObject.TryGetComponent(out CardSlot slot))
            {
                if (slot.CanPlaceCard(Card.CardData))
                {
                    if (isTutorial)
                    {
                        eventsChannelSO.Invoke(EventChannelSystem.EventType.CardAttach);
                    }
                    PlaceCard(slot);
                    if (Card is StatCard && Card.CardTransform.CurrentSlot == null)
                        CardDeckManager.Instance.RemoveCard();
                    Card.CardTransform.SetSlot(slot);
                    _originalPosition = slot.GetCardPosition(Card as BaseCard);
                    SetPosition();
                        
                    Debug.Log("CardDragger.OnEndDrag: Card placed in slot");
                        
                    return true;
                }

                Debug.LogWarning($"Cannot place card {Card.CardData.name} in slot {slot.name}");
            }

            return false;
        }

        private void SetPosition()
        {
            _rectTransform.anchoredPosition = _originalPosition;
            Card.CardAnimator.StopShake();
            Card.CardAnimator.ShakeCard();
            OnCardDrag?.Invoke(false);
        }

        public virtual void PlaceCard(CardSlot slot)
        {
            slot?.PlaceCard(Card as StatCard);
            SetDraggable(true);
        }

        public virtual void SetDraggable(bool isDraggable)
        {
            _isDraggable = isDraggable;
            if (_isDraggable)
            {
                Debug.Log("CardDragger: Card is now draggable");
            }
            else
            {
                Debug.Log("CardDragger: Card is no longer draggable");
            }
            OnChangeDraggable?.Invoke(_isDraggable);
        }
    }
}
