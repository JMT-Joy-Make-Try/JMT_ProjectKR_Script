using System;
using System.Collections.Generic;
using JMT.System.StatSystem;

namespace JMT.System.SkillSystem
{
    [Serializable]
    public class SkillStatData : StatData<SkillType>, ICloneable
    {
        public SkillStatData(SkillType type, float defaultValue) : base(type, defaultValue)
        {
        }

        public string StatTypeToString
        {
            get
            {
                switch (Type)
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

        public object Clone()
        {
            SkillStatData skillStatData = new SkillStatData(Type, DefaultValue);
            skillStatData.Modifiers = new List<StatModifier>(Modifiers);
            skillStatData.Icon = Icon;
            return skillStatData;
        }
    }
}