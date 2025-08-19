using System;
using System.Collections.Generic;
using JMT.Core.Tool;
using JMT.System.Card.CardData;
using UnityEngine;

namespace JMT.System.SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Data", menuName = "SO/Skill/SkillData")]
    public class SkillDataSO : CardDataSO
    {
        [Header("카드용")]
        public List<SkillStatData> skillStats;
        public List<StatCardDataSO> stats;

        [Header("적 스킬용")]
        public SkillSO skillSO;
        public float probability;

        /// <summary>
        /// true면 모든 modifier를 합산 후 한 번에 적용
        /// false면 순차적으로 누적 적용
        /// </summary>
        public bool applyModifiersAdditive = true;

        public override void Init()
        {
            base.Init();

            if (skillStats == null)
                skillStats = new List<SkillStatData>();

            if (stats != null && stats.Count > 0 && stats[0] != null)
            {
                stats[0] = stats[0].Clone();
                stats[0].Color = Color;
            }
        }

        public void ApplySkillStat()
        {
            if (skillStats == null || skillStats.Count == 0) return;

            bool isFirst = true;
            foreach (var stat in stats)
            {
                if (stat == null) continue;

                bool isBuff = isFirst || stat.Color == Color;
                isFirst = false;

                ApplyCardStatsToSkillStats(isBuff ? stat.buffCardStats : stat.deBuffCardStats);
            }
        }

        /// <summary>
        /// 특정 카드 스탯 목록을 skillStats에 적용
        /// </summary>
        private void ApplyCardStatsToSkillStats(List<StatCardData> cardStats)
        {
            if (cardStats == null) return;

            foreach (var cardStat in cardStats)
            {
                if (cardStat == null || cardStat.cardStatData == null) continue;

                foreach (var skillStat in skillStats)
                {
                    if (skillStat == null) continue;
                    if (cardStat.cardStatType != skillStat.Type) continue;

                    skillStat.AddModifier(cardStat.cardStatData);
                }
            }
        }

        public SkillStatData GetStatData(SkillType type)
        {
            return skillStats?.Find(stat => stat.Type == type);
        }

        public bool ContainsStat(SkillType type)
        {
            return skillStats != null && skillStats.Exists(stat => stat.Type == type);
        }

        public bool TryGetStatDataValue(SkillType type, out float statData)
        {
            var stat = GetStatData(type);
            if (stat != null)
            {
                statData = stat.GetValue();
                return true;
            }
            statData = 0f;
            return false;
        }

        public void UseSkill()
        {
            if (skillSO == null)
            {
                Debug.LogWarning("SkillSO is not assigned.");
                return;
            }
            skillSO.ExecuteSkill();
        }

        /// <summary>
        /// 특정 SkillType의 modifier가 적용된 최종 값을 반환
        /// </summary>
        private float GetModifierValue(SkillType statType)
        {
            if (skillStats == null || skillStats.Count == 0 || stats == null)
                return 0f;

            float totalValue = 0f;
            bool isFirst = true;

            foreach (var stat in stats)
            {
                if (stat == null) continue;

                bool isBuff = isFirst || stat.Color == Color;
                isFirst = false;

                var cardStats = isBuff ? stat.buffCardStats : stat.deBuffCardStats;
                if (cardStats == null) continue;

                foreach (var cardStat in cardStats)
                {
                    if (cardStat.cardStatType != statType || cardStat.cardStatData == null)
                        continue;

                    var statData = GetStatData(statType);
                    if (statData == null) continue;

                    float baseValue = applyModifiersAdditive
                        ? statData.DefaultValue // additive면 항상 기본값에서 계산
                        : (totalValue == 0f ? statData.DefaultValue : totalValue); // 누적이면 이전 계산값 사용

                    if (StatExtension.operations.TryGetValue(cardStat.cardStatData.ModifierType, out var operation))
                    {
                        baseValue = operation(baseValue, cardStat.cardStatData.Value);
                    }

                    if (applyModifiersAdditive)
                        totalValue += baseValue;
                    else
                        totalValue = baseValue; // 누적 모드면 바로 교체
                }
            }

            return totalValue;
        }

        /// <summary>
        /// skillStats에 등록된 모든 타입의 modifier 적용값 합산
        /// </summary>
        public float CalculateFinalSkillValue()
        {
            if (skillStats == null || skillStats.Count == 0)
                return 0f;

            float finalValue = 0f;
            foreach (var statData in skillStats)
            {
                if (statData == null) continue;
                finalValue += GetModifierValue(statData.Type);
            }

            return finalValue;
        }

        public List<SkillStatData> GetAllStats()
        {
            List<SkillStatData> allStats = new List<SkillStatData>();
            foreach (var stat in skillStats)
            {
                if (stat == null) continue;

                float modifierValue = GetModifierValue(stat.Type);
                if (modifierValue == 0) continue;

                var statData = stat.Clone() as SkillStatData;
                statData.DefaultValue = modifierValue;
                allStats.Add(statData);

                Debug.Log($"<color=blue>Stat: {stat.Type}, Value: {modifierValue}</color>");
            }
            return allStats;
        }

        public void DestroySO()
        {
            foreach (var stat in stats)
            {
                if (stat != null && stat.IsCloned())
                {
                    Destroy(stat);
                }
            }
        }
    }
}
