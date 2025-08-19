using System;
using System.Collections.Generic;
using JMT.Core;
using JMT.Core.Tool;
using JMT.System.Card.CardData;
using JMT.System.CombatSystem;
using JMT.System.DeckSettingSystem;
using JMT.System.SkillSystem;
using JMT.System.StatSystem;
using JMT.UISystem;
using JMT.WaveSystem;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace JMT.System.DataSystem
{
    public class DataManager : MonoSingleton<DataManager>
    {
        #region Card Data
        [Header("스탯 카드 데이터")]
        [SerializeField] private StatCardListSO _notOwnStatCardList;
        [SerializeField] private StatCardListSO _ownStatCardList; // 덱세팅 완료 -> 이 SO로 들어감

        [Header("스킬 카드 데이터")]
        [SerializeField] private SkillDataListSO _notOwnSkillCardList;
        [SerializeField] private SkillDataListSO _ownSkillCardList; // 덱세팅 완료 -> 이 SO로 들어감

        private List<SkillDataSO> _pocketSkills = new List<SkillDataSO>(); // 덱 세팅 전 여기에 카드들이 저장됨
        private List<StatCardDataSO> _pocketStats = new List<StatCardDataSO>(); // 덱 세팅 전 여기에 카드들이 저장됨
        #endregion

        #region Item Data
        [Header("아이템 데이터")]
        [SerializeField] private List<ItemSO> _notOwnItems;
        [SerializeField] private List<ItemSO> _ownItems;
        #endregion

        #region Passive Data
        [Header("패시브 데이터")]
        [SerializeField] private PassiveListSO _notOwnPassiveList;
        [SerializeField] private PassiveListSO _ownPassiveList;
        #endregion

        [Header("코인 데이터")]
        [SerializeField] private int _coinCount;

        [Header("Debug")] // 튜토리얼용
        [SerializeField] private bool _isTutorialEnabled = false;


        #region Properties
        public StatCardListSO NotOwnStatCardList => _notOwnStatCardList;
        public List<StatCardDataSO> OwnStatCardList => _ownStatCardList.statCardList;
        public List<SkillDataSO> NotOwnSkillCardList => _notOwnSkillCardList.skillData;
        public SkillDataListSO OwnSkillCardList => _ownSkillCardList;
        public List<ItemSO> NotOwnItems => _notOwnItems;
        public List<ItemSO> OwnItems => _ownItems;
        public PassiveListSO OwnPassiveList => _ownPassiveList;
        public int CoinCount => _coinCount;
        public List<SkillDataSO> PocketSkills => _pocketSkills;
        public List<StatCardDataSO> PocketStats => _pocketStats;
        #endregion

        // 카드 추가 이벤트
        public event Action<List<CardDataSO>> OnAddCard;

        // 코인 수 변경 이벤트
        public event Action<int> OnCoinCountChanged;

        // 튜토리얼용
        private int _tutorialCardCount = 0;

        private List<PassiveSO> _tempPassiveList = new List<PassiveSO>();

        public event Action<SkillDataSO, bool> OnSkillCardChanged;
        public event Action<StatCardDataSO, bool> OnStatCardChanged;
        protected override void Awake()
        {
            base.Awake();
            InstantiateData();
            InitData();
        }

        private void InstantiateData()
        {
            _notOwnStatCardList = Instantiate(_notOwnStatCardList);
            _ownStatCardList = Instantiate(_ownStatCardList);
            _notOwnSkillCardList = Instantiate(_notOwnSkillCardList);
            _ownSkillCardList = Instantiate(_ownSkillCardList);
            _notOwnPassiveList = Instantiate(_notOwnPassiveList);
            _ownPassiveList = Instantiate(_ownPassiveList);
        }

        private void DestroyData()
        {
            Destroy(_notOwnStatCardList);
            Destroy(_ownStatCardList);
            Destroy(_notOwnSkillCardList);
            Destroy(_ownSkillCardList);
            Destroy(_notOwnPassiveList);
            Destroy(_ownPassiveList);
        }

        private void InitData()
        {
            _notOwnStatCardList.Init();
            _notOwnSkillCardList.Init();
            _notOwnPassiveList.Init();


            DeckSettingManager.Instance.SetCardLists(_ownSkillCardList, _ownStatCardList);
            DeckSettingManager.Instance.SetDeckCards(_pocketSkills, _pocketStats);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                AddPassive(RandomPassive());
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                AddPassiveToOwnList();
            }
        }

        /// <summary>
        /// OwnList에 데이터를 추가하고, NotOwnList에서 제거
        /// </summary>
        /// <typeparam name="T">추가할 데이터 타입</typeparam>
        /// <param name="data">추가할 데이터</param>
        /// <param name="ownList">가지고 있는 리스트(인벤토리 역할)</param>
        /// <param name="notOwnList">가지고 있지 않은 리스트</param>
        private void AddData<T>(T data, List<T> ownList, List<T> notOwnList, bool isOpenPanel = true, bool isOwn = true, bool isRemove = true, List<T> pocket = null)
            where T : Object
        {
            if (data == null)
            {
                // 넣을 데이터가 없을 경우
                Debug.LogWarning("Data is null, cannot add to the list.");
                return;
            }
            if (ownList.Contains(data) && isOwn)
            {
                // 이미 가지고있는 경우
                Debug.LogWarning($"{typeof(T).Name} {data.name} already exists in the list. Skipping.");
                return;
            }
            if (pocket != null && pocket.Contains(data) && isOwn)
            {
                // 덱에 이미 있는 경우
                Debug.LogWarning($"{typeof(T).Name} {data.name} already exists in the pocket. Skipping.");
                return;
            }

            // 아이템 이동(NotOwnList에서 OwnList로)
            ownList.Add(data);
            if (isRemove)
                notOwnList?.Remove(data);
            Debug.Log($"[Add] {typeof(T).Name}: {data.name}");
            // 아이템 팝업 열기
            if (data is ItemSO item && isOpenPanel)
                UIManager.Instance.ItemPopupCompo.OpenPanel(item);
        }

        /// <summary>
        /// 아이템들을 OwnList에 추가하고, NotOwnList에서 제거
        /// 여러 개의 아이템을 한 번에 추가할 때 사용
        /// </summary>
        /// <typeparam name="T">넣을 아이템 타입</typeparam>
        /// <param name="items">추가할 아이템 리스트</param>
        /// <param name="ownList">가지고 있는 리스트(인벤토리 역할)</param>
        /// <param name="notOwnList">가지고 있지 않은 리스트</param>
        public void AddItems<T>(List<T> items, List<T> ownList, List<T> notOwnList) where T : CardDataSO
        {
            if (items == null || items.Count == 0)
            {
                Debug.LogWarning("Items list is null or empty, cannot add to the list.");
                return;
            }

            // 아이템 리스트를 순회하며 각각의 아이템을 추가
            foreach (var item in items)
            {
                AddData(item, ownList, notOwnList);
            }

            OnAddCard?.Invoke(items as List<CardDataSO>);
        }


        /// <summary>
        /// OwnList에서 데이터를 제거하고, NotOwnList에 추가
        /// </summary>
        /// <typeparam name="T">지울 데이터 타입</typeparam>
        /// <param name="data">지울 데이터</param>
        /// <param name="ownList">가지고 있는 리스트(인벤토리 역할)</param>
        /// <param name="notOwnList">가지고 있지 않은 리스트</param>
        private void RemoveData<T>(T data, List<T> ownList, List<T> notOwnList) where T : Object
        {
            if (data == null)
            {
                Debug.LogWarning("Data is null, cannot remove from the list.");
                return;
            }

            // OwnList에 데이터가 있을 때만 제거
            if (ownList.Contains(data))
            {
                ownList.Remove(data);
                notOwnList?.Add(data);
                Debug.Log($"[Remove] {typeof(T).Name}: {data.name}");
            }
            else
            {
                Debug.LogWarning($"[Remove] {typeof(T).Name} {data.name} does not exist in the list.");
            }
        }

        /// <summary>
        /// 스탯 카드 추가
        /// _pocketStats에 추가하고, NotOwnStatCardList에서 제거
        /// </summary>
        /// <param name="cardData">추가할 카드</param>
        public void AddStatCard(StatCardDataSO cardData)
        {
            AddData(cardData, _pocketStats, _notOwnStatCardList.statCardList, pocket: _ownStatCardList.statCardList);
            OnStatCardChanged?.Invoke(cardData, true);
        }

        /// <summary>
        /// 스탯 카드 제거
        /// _pocketStats에서 제거하고, NotOwnStatCardList에 추가
        /// </summary>
        /// <param name="cardData">제거할 카드</param>
        public void RemoveStatCard(StatCardDataSO cardData)
        {
            RemoveData(cardData, _pocketStats, _notOwnStatCardList.statCardList);
            OnStatCardChanged?.Invoke(cardData, false);
        }

        /// <summary>
        /// 스킬 카드 추가
        /// _pocketSkills에 추가하고, NotOwnSkillCardList에서 제거
        /// </summary>
        /// <param name="cardData">추가할 카드</param>
        public void AddSkillCard(SkillDataSO cardData)
        {
            AddData(cardData, _pocketSkills, _notOwnSkillCardList.skillData, pocket: _ownSkillCardList.skillData);
            OnSkillCardChanged?.Invoke(cardData, true);
        }

        /// <summary>
        /// 스킬 카드 제거
        /// _pocketSkills에서 제거하고, NotOwnSkillCardList에 추가
        /// </summary>
        /// <param name="cardData">제거할 카드</param>
        public void RemoveSkillCard(SkillDataSO cardData)
        {
            RemoveData(cardData, _pocketSkills, _notOwnSkillCardList.skillData);
            OnSkillCardChanged?.Invoke(cardData, false);
        }

        /// <summary>
        /// 아이템 추가
        /// OwnItems에 추가하고, NotOwnItems에서 제거
        /// </summary>
        /// <param name="item">추가할 아이템</param>
        public void AddItem(ItemSO item)
        {
            AddData(item, _ownItems, _notOwnItems);
        }

        /// <summary>
        /// 아이템 제거
        /// OwnItems에서 제거하고, NotOwnItems에 추가
        /// </summary>
        /// <param name="item">제거할 아이템</param>
        public void RemoveItem(ItemSO item)
        {
            RemoveData(item, _ownItems, _notOwnItems);
        }

        /// <summary>
        /// 패시브 추가
        /// _tempPassiveList에 추가하고, NotOwnPassiveList에서 제거
        /// </summary>
        /// <param name="passive">추가할 패시브</param>
        public void AddPassive(PassiveSO passive)
        {
            AddData(passive, _tempPassiveList, _notOwnPassiveList.passiveList, true, false, false);
        }

        /// <summary>
        /// 패시브 제거
        /// _tempPassiveList에서 제거하고, NotOwnPassiveList에 추가
        /// </summary>
        /// <param name="passive">제거할 패시브</param>
        public void RemovePassive(PassiveSO passive)
        {
            RemoveData(passive, _tempPassiveList, _notOwnPassiveList.passiveList);
        }

        /// <summary>
        /// 임시 패시브 리스트에 있는 패시브를 OwnPassiveList에 추가
        /// 이 메소드는 상점이 끝났을 때 호출되어야 합니다.
        /// </summary>
        public void AddPassiveToOwnList()
        {
            if (_tempPassiveList.Count == 0)
            {
                Debug.LogWarning("No passive to add to the own list.");
                return;
            }

            foreach (var passive in _tempPassiveList)
            {
                AddData(passive, _ownPassiveList.passiveList, _notOwnPassiveList.passiveList, false, false);
            }
            _tempPassiveList.Clear();
        }

        /// <summary>
        /// 코인 수를 계산하고 변경 이벤트를 호출
        /// </summary>
        /// <param name="value">추가할 값</param>
        /// <param name="isAdd">더할 지 여부(true: 더하기, false: 빼기)</param>
        /// <returns>더해진 값</returns>
        public int CalculateCoinCount(int value, bool isAdd = true)
        {
            if (isAdd)
            {
                if (WaveManager.Instance.CurrentWave % 10 == 0)
                {
                    value = Mathf.FloorToInt(StatExtension.operations[StatModifierType.Percent](value, 20f));
                }
                var coinPercent = CombatManager.Instance.Player.PlayerPassive.PassiveList.GetChanceValue("CoinCount");
                value = Mathf.FloorToInt(StatExtension.operations[StatModifierType.Percent](value, coinPercent));
            }
            _coinCount = isAdd ? _coinCount + value : _coinCount - value;

            if (_coinCount < 0)
            {
                _coinCount = 0;
            }
            
            OnCoinCountChanged?.Invoke(_coinCount);
            return _coinCount;
        }


        #region Random
        /// <summary>
        /// 랜덤 스탯 카드 가져오기
        /// 튜토리얼이 활성화된 경우, 순차적으로 카드 반환
        /// </summary>
        /// <returns>랜덤 스탯 카드 SO</returns>
        public StatCardDataSO GetRandomStatCard()
        {
            if (_ownStatCardList == null || _ownStatCardList.statCardList.Count == 0)
            {
                Debug.LogWarning("StatCardList is empty or not assigned.");
                return null;
            }

            if (_isTutorialEnabled)
            {
                return Instantiate(_ownStatCardList.statCardList[_tutorialCardCount++ % _ownStatCardList.statCardList.Count]);
            }

            int randomIndex = Random.Range(0, _ownStatCardList.statCardList.Count);
            return _ownStatCardList.statCardList[randomIndex];
        }

        /// <summary>
        /// 랜덤 스킬 카드 가져오기
        /// </summary>
        /// <returns>랜덤 스킬 카드 SO</returns>
        public SkillDataSO GetRandomSkillCard()
        {
            if (_ownSkillCardList == null || _ownSkillCardList.skillData.Count == 0)
            {
                Debug.LogWarning("SkillCardList is empty or not assigned.");
                return null;
            }

            int randomIndex = Random.Range(0, _ownSkillCardList.skillData.Count);
            return _ownSkillCardList.skillData[randomIndex];
        }


        /// <summary>
        /// 랜덤 패시브 가져오기
        /// </summary>
        /// <returns>랜덤 패시브 SO</returns>
        public PassiveSO RandomPassive()
        {
            if (_notOwnPassiveList == null || _notOwnPassiveList.passiveList.Count == 0)
            {
                Debug.LogWarning("PassiveList is empty or not assigned.");
                return null;
            }

            int randomIndex = Random.Range(0, _notOwnPassiveList.passiveList.Count);
            var passive = _notOwnPassiveList.passiveList[randomIndex];
            return passive;
        }
        #endregion

        private void OnDestroy()
        {
            DestroyData();
        }
    }
}