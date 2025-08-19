using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using JMT.Core;
using JMT.System.Card.CardData;
using JMT.System.DataSystem;
using JMT.System.SkillSystem;
using JMT.UISystem.Tooltip;
using UnityEngine;

namespace JMT.System.DeckSettingSystem
{
    public class DeckSettingManager : MonoSingleton<DeckSettingManager>
    {
        [SerializeField] private DeckSettingUI deckSettingUI;
        [SerializeField] private DeckSettingFilter deckSettingFilter;
        [SerializeField] private int _requiredSkillCardCount = 2; // 필수 스킬 카드 개수
        [SerializeField] private int _requiredStatCardCount = 3; // 필수 스탯 카드 개수
        [SerializeField] private int maxSkillCards = 5; // 최대 스킬 카드 개수
        [SerializeField] private int maxStatCards = 10; // 최대 스탯 카드 개수


        // 카드 이름 -> 개수
        private Dictionary<string, int> _cardCounts = new();
        private List<CardDataSO> _cardDatabase = new(); // 전체 카드 정보 보관 (중복 없음)
        private SkillDataListSO _skillCardList;
        private StatCardListSO _statCardList;
        private int _currentSkillCardCount = 0;
        private int _currentStatCardCount = 0;

        public DeckSettingUI DeckSettingUI => deckSettingUI;
        public DeckSettingFilter DeckSettingFilter => deckSettingFilter;
        public SkillDataListSO SkillCardList => _skillCardList;
        public StatCardListSO StatCardList => _statCardList;
        public int MaxSkillCards => maxSkillCards;
        public int MaxStatCards => maxStatCards;
        public int CurrentSkillCardCount => _currentSkillCardCount;
        public int CurrentStatCardCount => _currentStatCardCount;
        public event Action<int, int> OnSkillCardCountChanged;
        public event Action<int, int> OnStatCardCountChanged;


        private void Start()
        {
            DataManager.Instance.OnSkillCardChanged += HandleSkillCardChanged;
            DataManager.Instance.OnStatCardChanged += HandleStatCardChanged;
        }

        private void OnDestroy()
        {
            if (DataManager.Instance != null)
            {
                DataManager.Instance.OnSkillCardChanged -= HandleSkillCardChanged;
                DataManager.Instance.OnStatCardChanged -= HandleStatCardChanged;
            }
        }

        public void Init()
        {
            _currentSkillCardCount = _skillCardList.skillData.Count;
            _currentStatCardCount = _statCardList.statCardList.Count;
        }

        private void HandleStatCardChanged(StatCardDataSO sO, bool isAdd)
        {
            if (isAdd)
            {
                AddCard(sO);
                _currentStatCardCount++;
                OnStatCardCountChanged?.Invoke(_currentStatCardCount, maxStatCards);
            }
            else
            {
                RemoveCard(sO);
                _currentStatCardCount--;
                OnStatCardCountChanged?.Invoke(_currentStatCardCount, maxStatCards);
            }
        }

        private void HandleSkillCardChanged(SkillDataSO sO, bool isAdd)
        {
            if (isAdd)
            {
                AddCard(sO);
                _currentSkillCardCount++;
                OnSkillCardCountChanged?.Invoke(_currentSkillCardCount, maxSkillCards);
            }
            else
            {
                RemoveCard(sO);
                _currentSkillCardCount--;
                OnSkillCardCountChanged?.Invoke(_currentSkillCardCount, maxSkillCards);
            }
        }

        /// <summary>
        /// 카드 데이터베이스 초기화
        /// </summary>
        /// <param name="skillCardList">InvenSkillCardListSO</param>
        /// <param name="statCardList">InvenStatCardListSO</param>
        public void SetCardLists(SkillDataListSO skillCardList, StatCardListSO statCardList)
        {
            _skillCardList = skillCardList;
            _statCardList = statCardList;
        }

        public void SetDeckCards(List<SkillDataSO> skillDatas, List<StatCardDataSO> statDatas)
        {
            foreach (var skillData in skillDatas)
            {
                AddCard(skillData);
            }
            foreach (var statData in statDatas)
            {
                AddCard(statData);
            }
        }

        /// <summary>
        /// 카드 추가
        /// </summary>
        /// <param name="card">추가할 카드</param>
        public void AddCard(CardDataSO card)
        {
            if (card == null) return;

            if (_cardCounts.TryAdd(card.Name, 1))
                _cardDatabase.Add(card); // 새로운 카드면 추가
            else
                _cardCounts[card.Name]++;
        }

        /// <summary>
        /// 카드 제거
        /// </summary>
        /// <param name="card">제거할 카드</param>
        public void RemoveCard(CardDataSO card)
        {
            if (card == null) return;

            if (_cardCounts.TryGetValue(card.Name, out int count))
            {
                if (count <= 1)
                {
                    _cardCounts.Remove(card.Name);
                    _cardDatabase.RemoveAll(c => c.Name == card.Name);
                }
                else
                {
                    _cardCounts[card.Name]--;
                }
            }
        }

        /// <summary>
        /// 특정 카드의 개수를 반환. 어떤 카드도 없으면 0 반환
        /// </summary>
        /// <param name="cardName">카드의 이름. 예) 대감굿</param>
        /// <returns>카드의 개수</returns>
        public int GetCardCount(string cardName)
        {
            return _cardCounts.TryGetValue(cardName, out int count) ? count : 0;
        }

        /// <summary>
        /// 특정 태그를 가진 카드 목록을 반환
        /// </summary>
        /// <param name="tag">카드 태그</param>
        /// <returns>해당 태그를 가진 카드 목록</returns>
        public List<CardDataSO> GetCardsByTag(ItemTag tag)
        {
            return _cardDatabase.Where(c => c.Tags.Contains(tag)).ToList();
        }

        /// <summary>
        /// 특정 타입을 가진 카드 목록을 반환
        /// </summary>
        /// <typeparam name="T">카드 타입</typeparam>
        /// <returns>해당 타입을 가진 카드 목록</returns>
        public List<CardDataSO> GetCardsByType<T>() where T : CardDataSO
        {
            return _cardDatabase.OfType<T>().Cast<CardDataSO>().ToList();
        }

        /// <summary>
        /// 전체 카드 목록을 반환
        /// </summary>
        /// <returns>전체 카드 목록</returns>
        public List<CardDataSO> GetAllCards()
        {
            return _cardDatabase.ToList();
        }

        /// <summary>
        /// 정렬된 카드 목록을 반환
        /// SkillDataSO, StatCardDataSO 순으로 정렬
        /// </summary>
        /// <returns>정렬된 카드 목록</returns>
        public List<CardDataSO> GetSortedCards()
        {
            return _cardDatabase
                .OrderBy(card =>
                {
                    if (card is SkillDataSO) return 0;
                    if (card is StatCardDataSO) return 1;
                    return -1;
                })
                .ThenBy(card => card.name) // 이름 순 정렬 (옵션)
                .ToList();
        }

        public void AddSkillCard(SkillDataSO card)
        {
            if (card == null) return;
            if (_currentSkillCardCount >= maxSkillCards)
            {
                Debug.LogWarning("최대 스킬 카드 개수를 초과했습니다.");
                return;
            }
            if (_skillCardList.Contains(card))
            {
                Debug.LogWarning("이미 존재하는 스킬 카드입니다.");
                return;
            }

            _skillCardList.skillData.Add(card);
            DataManager.Instance.PocketSkills.Remove(card);
            _cardDatabase.Remove(card);
            _currentSkillCardCount++;
            OnSkillCardCountChanged?.Invoke(_currentSkillCardCount, maxSkillCards);
        }

        public void AddStatCard(StatCardDataSO card)
        {
            if (card == null) return;
            if (_currentStatCardCount >= maxStatCards)
            {
                Debug.LogWarning("최대 스탯 카드 개수를 초과했습니다.");
                return;
            }
            if (_statCardList.Contains(card))
            {
                Debug.LogWarning("이미 존재하는 스탯 카드입니다.");
                return;
            }


            _statCardList.statCardList.Add(card);
            DataManager.Instance.PocketStats.Remove(card);
            _cardDatabase.Remove(card);
            _currentStatCardCount++;
            OnStatCardCountChanged?.Invoke(_currentStatCardCount, maxStatCards);
        }

        public void RemoveStatCard(StatCardDataSO statCardDataSO)
        {
            if (statCardDataSO == null) return;

            _statCardList.statCardList.Remove(statCardDataSO);
            DataManager.Instance.PocketStats.Add(statCardDataSO);
            _cardDatabase.Remove(statCardDataSO);
            _currentStatCardCount--;
            OnStatCardCountChanged?.Invoke(_currentStatCardCount, maxStatCards);
        }

        public void RemoveSkillCard(SkillDataSO skillCardDataSO)
        {
            if (skillCardDataSO == null) return;

            _skillCardList.skillData.Remove(skillCardDataSO);
            DataManager.Instance.PocketSkills.Add(skillCardDataSO);
            _cardDatabase.Remove(skillCardDataSO);
            _currentSkillCardCount--;
            OnSkillCardCountChanged?.Invoke(_currentSkillCardCount, maxSkillCards);
        }

        public bool IsCountValid(string cardType)
        {
            if (cardType == "Skill")
            {
                return _currentSkillCardCount >= _requiredSkillCardCount && _currentSkillCardCount <= maxSkillCards;
            }
            else if (cardType == "Stat")
            {
                return _currentStatCardCount >= _requiredStatCardCount && _currentStatCardCount <= maxStatCards;
            }
            return false;
        }

        public bool IsCountValid()
        {
            return IsCountValid("Skill") && IsCountValid("Stat");
        }
    }
}
