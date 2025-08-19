using System.Collections.Generic;
using System.Linq;
using JMT.System.Card.CardData;
using JMT.System.Card.Slot;
using JMT.System.CombatSystem;
using JMT.System.CombatSystem.Event;
using JMT.System.SkillSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JMT.System.Card.Component
{
    public class SkillCardDragger : CardDragger
    {
        [SerializeField] private PlayerTurnEventSO _playerTurnEvent;
        [SerializeField] private SkillCardSlotChangeEvent _skillCardSlotChangeEvent;
        private List<StatCard> _statCards;
        public override void RemoveCard()
        {
            Card.CardTransform.CurrentSlot?.RemoveSkillCard();
            _skillCardSlotChangeEvent?.Invoke(true);
        }

        public override void PlaceCard(CardSlot slot)
        {
            slot.AddSkillCard(Card.CardData as SkillDataSO);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log($"Dragging {Card.CardData.name} in {transform.parent.name}");
            _statCards = transform.parent.GetComponentsInChildren<StatCard>().ToList();
            _statCards.ForEach(card =>
            {
                card.transform.SetParent(transform);
            });
            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            Card.CardTransform.transform.localScale = Vector3.one;

            for (int i = 0; i < _statCards.Count; i++)
            {
                _statCards[i].transform.SetParent(Card.CardTransform.CurrentSlot.transform);
                _statCards[i].transform.localPosition = _statCards[i].CardTransform.CurrentSlot?.GetCardPosition(i) ?? Vector3.zero;
                _statCards[i].transform.rotation = Quaternion.identity;
                _statCards[i].transform.localScale = Vector3.one;
            }
            _skillCardSlotChangeEvent?.Invoke(false);
        }

        protected override bool UseCard(GameObject rayResultGameObject)
        {
            if (rayResultGameObject.TryGetComponent(out SkillUseArea area) && Card is SkillCard skillCard)
            {
                var skillCardData = skillCard.CardData as SkillDataSO;
                if (skillCardData.ContainsStat(SkillType.PhysicalAttack))
                {
                    skillCard.UseSkillCard(CardStatType.PhysicalAttack);
                }
                else if (skillCardData.ContainsStat(SkillType.MagicAttack))
                {
                    skillCard.UseSkillCard(CardStatType.MagicAttack);
                }
                
                _playerTurnEvent?.Invoke(TurnPhase.End);
                _statCards.ForEach(statCard =>
                {
                    if (statCard == null) return;
                    statCard.CardDragger.RemoveCard();
                    Destroy(statCard.gameObject);
                });
                
                return true;
            }
            return base.UseCard(rayResultGameObject);
        }

        public override void SetDraggable(bool isDraggable)
        {
            _isDraggable = isDraggable;
        }
    }
}