using System;
using System.Collections;
using JMT.System.Card.CardData;
using JMT.System.Card.Component;
using JMT.System.Card.Event;
using JMT.System.CombatSystem;
using JMT.System.DeckSettingSystem;
using JMT.System.EventChannelSystem;
using JMT.System.SkillSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JMT.System.Card
{
    public class SkillCard : BaseCard
    {
        [SerializeField] private SkillDataSO _skillCardData;
        [SerializeField] private SkillUseEventSO _skillUseEvent;

        [Header("Tutorial")]
        [SerializeField] private bool isTutorial = false;
        [SerializeField] private EventsChannelSO eventsChannelSO;
        private bool _isInvoked = false;
        public override CardDataSO CardData => _skillCardData;
        public SkillDataSO SkillData => _skillCardData;


        protected override void Start()
        {
            base.Start();
            CardVisual.FlipCard(true);
            CardVisual.SetVisual(CardData);
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _skillCardData.DestroySO();
        }

        public void UseSkillCard(CardStatType type)
        {
            if (isTutorial && !_isInvoked)
            {
                _isInvoked = true;
                eventsChannelSO.Invoke(EventChannelSystem.EventType.SkillCardUse);
            }
            _skillCardData.ApplySkillStat();

            _skillUseEvent?.Invoke(_skillCardData);
            Destroy(gameObject);
        }

        public void SetSkillCardData(SkillDataSO skillCardData, bool isInit = true)
        {
            _skillCardData = skillCardData;
            SetCardData(_skillCardData);
            _skillCardData = Instantiate(_skillCardData);
            if (isInit)
                _skillCardData.Init();
            CardVisual.SetVisual(_skillCardData);
        }

        public Tuple<SkillType, float> GetFinalSkillValue()
        {
            var skillType = _skillCardData.skillStats.Count > 0 ? _skillCardData.skillStats[0].Type : SkillType.None;
            float finalValue = _skillCardData.CalculateFinalSkillValue();
            return new Tuple<SkillType, float>(skillType, finalValue);
        }

        private void HandleMoveToPocket()
        {
            DeckSettingManager.Instance.RemoveSkillCard(_skillCardData);
            DeckSettingManager.Instance.DeckSettingUI.MoveCardToDeckContent(this);
        }

        private void HandleMoveToDeck()
        {
            DeckSettingManager.Instance.AddSkillCard(_skillCardData);
            DeckSettingManager.Instance.DeckSettingUI.MoveCardToSkillContent(this);
        }
    }
}