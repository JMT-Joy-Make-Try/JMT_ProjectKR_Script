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
                        return "�������ݷ�";
                    case SkillType.PhysicalAttack:
                        return "�������ݷ�";
                    case SkillType.MagicDefense:
                        return "��������";
                    case SkillType.PhysicalDefense:
                        return "�������ݷ�";
                    case SkillType.CriticalChance:
                        return "ġ��ŸȮ��";
                    case SkillType.CriticalDamage:
                        return "ġ��Ÿ����";
                    case SkillType.Evasion:
                        return "ȸ����";
                }
                return string.Empty;
            }
        }
    }
}
