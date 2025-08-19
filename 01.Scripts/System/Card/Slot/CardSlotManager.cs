using System.Collections.Generic;
using JMT.Core;
using JMT.System.AgentSystem.PlayerSystem;
using JMT.System.Card.CardData;
using JMT.System.SkillSystem;
using UnityEngine;

namespace JMT.System.Card
{
    public class CardSlotManager : MonoSingleton<CardSlotManager>
    {
        [SerializeField] private List<CardSlot> cardSlots;

        public List<CardSlot> CardSlots => cardSlots;


        public void ResetCardSlots()
        {
            foreach (var slot in cardSlots)
            {
                slot?.ResetSlot();
            }
        }

        public void SetSkillCard(int slotIndex, SkillDataSO cardData)
        {
            if (slotIndex < 0 || slotIndex >= cardSlots.Count)
            {
                Debug.LogError("Invalid slot index");
                return;
            }

            cardSlots[slotIndex].SetSkillCardData(cardData);
        }

        public SkillDataSO GetSkillCard(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= cardSlots.Count)
            {
                Debug.LogError("Invalid slot index");
                return null;
            }

            return cardSlots[slotIndex].GetSkillCardData();
        }

        public List<SkillDataSO> GetAnotherSkillCards(int index)
        {
            List<SkillDataSO> skillCards = new List<SkillDataSO>();
            for (int i = 0; i < cardSlots.Count; i++)
            {
                if (i != index)
                {
                    var skillCardData = cardSlots[i].GetSkillCardData();
                    if (skillCardData != null)
                    {
                        skillCards.Add(skillCardData);
                    }
                }
            }
            return skillCards;
        }
    }
}