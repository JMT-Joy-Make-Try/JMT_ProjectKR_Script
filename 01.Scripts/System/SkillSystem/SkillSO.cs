using UnityEngine;

namespace JMT.System.SkillSystem
{
    public abstract class SkillSO : ScriptableObject
    {
        public abstract void Init(SkillType skillType, float damage);
        public abstract void ExecuteSkill();
    }
}