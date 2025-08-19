using System.Collections.Generic;
using UnityEngine;

namespace JMT.UISystem.Tooltip
{
    public interface ITooltipable
    {
        string Name { get; }
        List<ItemTag> Tags { get; }
        string Desc { get; }
    }

    public enum ItemTag
    {
        DodgeRate,
        TotalAttack,
        MagicAttack,
        PhysicalAttack,
        LastingEffect,
        Stemina,
        Evasion, // 회피
        InterferenceEvasion, // 간섭 회피
        CriticalChance, // 치명타 확률
        CriticalDamage, // 치명타 피해
        PhysicalDefense, // 물리 방어
        MagicDefense, // 마법 방어
    }
}
