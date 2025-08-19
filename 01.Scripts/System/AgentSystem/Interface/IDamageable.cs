using System;
using JMT.System.SkillSystem;

namespace JMT.System.AgentSystem.Interface
{
    public interface IDamageable
    {
        void Init(float maxHealth);
        void TakeDamage(DamageResult damageResult);
        void Dead();
    }

    public struct DamageResult
    {
        public float damageAmount;
        public float evasion;
        public float magicDefense;
        public float physicalDefense;
        public float criticalChance;
        public float criticalDamage;
        public SkillType skillType;

        public DamageResult(float damageAmount, float evasion, float magicDefense, float physicalDefense, float criticalChance, float criticalDamage, SkillType skillType)
        {
            this.damageAmount = damageAmount;
            this.evasion = evasion;
            this.magicDefense = magicDefense;
            this.physicalDefense = physicalDefense;
            this.criticalChance = criticalChance;
            this.criticalDamage = criticalDamage;
            this.skillType = skillType;
        }
    }
}