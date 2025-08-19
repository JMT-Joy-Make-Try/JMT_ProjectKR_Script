using System;
using System.Collections.Generic;
using System.Linq;
using JMT.System.Card.CardData;
using JMT.System.DataSystem;
using JMT.System.SkillSystem;
using JMT.UISystem;
using UnityEngine;

namespace JMT.System.Card
{
    public class CardSlot : MonoBehaviour
    {
        [SerializeField] private SkillCard _skillCardPrefab;
        [SerializeField] private RectTransform _baseSpawnPoint;
        [SerializeField] private float _yOffset = 100f;
        [SerializeField] private bool _forcePlaceCard = false;
        [SerializeField] private int _maxCardCount = 6;

        private RectTransform _rectTransform;

        private SkillDataSO _skillCardData;
        private bool _isAceCardEnabled = false;
        private List<StatCard> _statCards = new List<StatCard>();
        private List<StatCard> _tempStatCards = new List<StatCard>();
        private SkillCard _currentSkillCard;
        private int _currentCardCount = 0;

        public SkillDataSO skillDataSO => _skillCardData;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            UIManager.Instance.DeckCompo.OnOpenPanelEvent += CardSpawn;
        }

        private void OnDestroy()
        {
            if (UIManager.Instance == null || UIManager.Instance.DeckCompo == null) return;
            UIManager.Instance.DeckCompo.OnOpenPanelEvent -= CardSpawn;
        }

        public void CardSpawn()
        {
            if (_isAceCardEnabled) return;
            _isAceCardEnabled = true;
            _currentSkillCard = Instantiate(_skillCardPrefab, _baseSpawnPoint.position, Quaternion.identity);
            _currentSkillCard.CardTransform.SetSlot(this);
            _currentSkillCard.SetSkillCardData(DataManager.Instance.GetRandomSkillCard());
            AddSkillCard(_currentSkillCard.CardData as SkillDataSO);
            _skillCardData?.Init();
            _currentCardCount = 0;
        }

        public bool CanPlaceCard(CardDataSO cardData)
        {
            if (_forcePlaceCard)
            {
                Debug.LogWarning($"CardSlot.CanPlaceCard: Force placing card {cardData.name} in slot {name}");
                return true;
            }
            if (_currentCardCount >= _maxCardCount)
            {
                Debug.LogWarning($"CardSlot.CanPlaceCard: Slot {name} is full. Cannot place card {cardData.name}");
                return false;
            }
            if (_skillCardData == null)
            {
                Debug.LogWarning("CardSlot.CanPlaceCard: Skill card data is null");
                return true;
            }
            if (_skillCardData != null)
            {
                if (cardData is SkillDataSO)
                {
                    Debug.LogWarning($"CardSlot.CanPlaceCard: Cannot place skill card {cardData.name} in slot {name}");
                    return false;
                }
            }
            var list = _skillCardData.stats;

            if (list.Last().Color != cardData.Color)
            {
                return true;
            }

            Debug.LogWarning($"Card {cardData.name} cannot be placed in slot {name}");
            return false;
        }

        public void PlaceCard(StatCard cardData)
        {
            if (_skillCardData == null)
            {
                Debug.LogWarning("CardSlot.PlaceCard: Skill card data is null");
                return;
            }
            _skillCardData?.stats.Add(cardData.CardData as StatCardDataSO);
            _statCards.Add(cardData);
            foreach (var card in _statCards)
            {
                card.CardDragger.SetDraggable(false);
            }
            _statCards.Last().CardDragger.SetDraggable(true);
            _currentCardCount = _skillCardData.stats.Count - 1;
        }

        public void RemoveCard(StatCard cardData)
        {
            if (cardData == null)
            {
                Debug.LogWarning("CardSlot.RemoveCard: CardData is null");
                return;
            }
            if (_skillCardData == null)
            {
                Debug.LogWarning("CardSlot.RemoveCard: Skill card data is null");
                return;
            }
            if (_skillCardData.stats.Contains(cardData.CardData))
            {
                _skillCardData.stats.RemoveAt(_skillCardData.stats.Count - 1);
            }
            else
            {
                Debug.LogWarning($"Card {cardData.name} is not in slot {name}");
            }
            _statCards.Remove(cardData);
            if (_statCards.Count > 0)
                _statCards.Last().CardDragger.SetDraggable(true);

            _currentCardCount = _skillCardData.stats.Count - 1;
        }

        public void AddSkillCard(SkillDataSO skillCardData)
        {
            if (skillCardData == null)
            {
                Debug.LogWarning("CardSlot.AddSkillCard: Skill card data is null");
                return;
            }
            _skillCardData = skillCardData;
            _isAceCardEnabled = true;
            _currentSkillCard.CardDragger.SetDraggable(true);
            _currentCardCount = _skillCardData.skillStats.Count - 1;
            _statCards.AddRange(_tempStatCards);
            _tempStatCards.Clear();
        }

        public void RemoveSkillCard()
        {
            if (_skillCardData == null)
            {
                Debug.LogWarning("CardSlot.RemoveSkillCard: Skill card data is null");
                return;
            }
            _skillCardData = null;
            _isAceCardEnabled = false;
            _currentCardCount = -1;
            _tempStatCards.AddRange(_statCards);
            _statCards.Clear();
        }

        public Vector2 GetCardPosition(BaseCard card = null)
        {
            if (_baseSpawnPoint == null)
            {
                Debug.LogError("CardSlot.GetCardPosition: Base spawn point is not set.");
                return Vector2.zero;
            }

            int count = _skillCardData?.stats.Count ?? 0;
            Debug.Log($"CardSlot.GetCardPosition: Current card count is {count}");
            if (card != null)
            {
                if (card is SkillCard skillCard)
                    count = 0; // 현재 카드 제외
            }
            if (count <= 0)
                return _baseSpawnPoint.anchoredPosition;

            // 카드 간격
            float spacing = -220f;

            // 중앙 기준으로 정렬된 카드들의 Y 시작점
            float startY = -spacing * (count - 1) / 2f;

            // 현재 마지막 카드의 인덱스 = count - 1
            float posY = startY + spacing * (count - 1) + _yOffset;

            return _baseSpawnPoint.anchoredPosition + new Vector2(0, posY);
        }

        public Vector2 GetCardPosition(int index)
        {
            if (_baseSpawnPoint == null)
            {
                Debug.LogError("CardSlot.GetCardPosition: Base spawn point is not set.");
                return Vector2.zero;
            }

            if (index < 0)
            {
                Debug.LogError($"CardSlot.GetCardPosition: Invalid index {index}");
                return Vector2.zero;
            }

            // 카드 간격
            float spacing = -220f;

            // 중앙 기준으로 정렬된 카드들의 Y 시작점
            float startY = -spacing * (index - 1) / 2f;

            // 현재 카드의 인덱스에 따른 Y 위치
            float posY = startY + spacing * index + _yOffset;

            return _baseSpawnPoint.anchoredPosition + new Vector2(0, posY);
        }

        public RectTransform GetRectTransform()
        {
            return _rectTransform;
        }

        public void SetSkillCardData(SkillDataSO skillCardData)
        {
            if (skillCardData == null)
            {
                Debug.LogWarning("CardSlot.SetSkillCardData: Skill card data is null");
                return;
            }
            _skillCardData = skillCardData;
            _currentSkillCard.SetSkillCardData(skillCardData, false);
        }

        public SkillDataSO GetSkillCardData()
        {
            if (_skillCardData == null)
            {
                Debug.LogWarning("CardSlot.GetSkillCardData: Skill card data is null");
                return null;
            }
            return _skillCardData;
        }

        public void ResetSlot()
        {
            if (_statCards != null)
            {
                foreach (var slot in _statCards)
                {
                    if (slot == null) continue;
                    Destroy(slot?.gameObject);
                }
                _statCards?.Clear();
            }
            _isAceCardEnabled = false;
            _skillCardData = null;
            _currentCardCount = 0;
            if (_currentSkillCard != null)
            {
                Destroy(_currentSkillCard?.gameObject);
                _currentSkillCard = null;
            }
            CardSpawn();
        }
    }
}