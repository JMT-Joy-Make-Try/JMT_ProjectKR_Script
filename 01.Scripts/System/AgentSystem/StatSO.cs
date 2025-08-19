using JMT.System.SkillSystem;
using UnityEngine;
using UnityEngine.UI;

namespace JMT
{
    [CreateAssetMenu(fileName = "StatSO", menuName = "SO/Data/StatSO")]
    public class StatSO : ScriptableObject
    {
        public Sprite StatIcon;
        public SkillType StatType;

        public string StatTypeToString
        {
            get
            {
                switch (StatType)
                {
                    case SkillType.MagicAttack:
                        return "마법공격력";
                    case SkillType.PhysicalAttack:
                        return "물리공격력";
                    case SkillType.MagicDefense:
                        return "마법방어력";
                    case SkillType.PhysicalDefense:
                        return "물리공격력";
                    case SkillType.CriticalChance:
                        return "치명타확률";
                    case SkillType.CriticalDamage:
                        return "치명타배율";
                    case SkillType.Evasion:
                        return "회피율";
                }
                return string.Empty;
            }
        }
    }
}
