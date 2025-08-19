using System;
using System.Collections.Generic;
using JMT.System.SkillSystem;
using JMT.System.StatSystem;
using UnityEngine;

namespace JMT.System.Card.CardData
{
    [CreateAssetMenu(fileName = "StatCardData", menuName = "SO/Card/StatCardData")]
    public class StatCardDataSO : CardDataSO
    {
        [Header("버프 스탯")]
        public List<StatCardData> buffCardStats;

        [Header("디버프 스탯")]
        public List<StatCardData> deBuffCardStats;

        private bool _isCloned = false;

        public StatCardDataSO Clone()
        {
            var clone = Instantiate(this);
            clone._isCloned = true;

            return clone;
        }

        public bool IsCloned()
        {
            return _isCloned;
        }
    }

    [Serializable]
    public class StatCardData
    {
        public SkillType cardStatType;
        public StatModifier cardStatData;
    }
}