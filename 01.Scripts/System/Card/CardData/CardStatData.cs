using System;
using JMT.System.CombatSystem;
using JMT.System.StatSystem;

namespace JMT.System.Card.CardData
{
    [Serializable]
    public class CardStatData : StatData<CardStatType>
    {
        public AttackerType AttackerType;
        public CardStatData(CardStatType type, float defaultValue) : base(type, defaultValue)
        {
        }
    }
}