using JMT.System.CombatSystem;
using JMT.System.SkillSystem;
using UnityEngine;

namespace JMT.System.AgentSystem.Enemy.Boss
{
    [CreateAssetMenu(fileName = "AttackSkill", menuName = "SO/Agent/Boss/Skills/AttackSkill")]
    public class AttackSkill : SkillSO
    {
        [SerializeField] private float damageAmount;
        [SerializeField] private SkillType skillType;
        [SerializeField] private bool _isUseSO;

        public override void Init(SkillType skillType, float damage)
        {
            if (_isUseSO) return;
            this.damageAmount = damage;
            this.skillType = skillType;
        }

        public override void ExecuteSkill()
        {
            CombatManager.Instance.TakeDamage(AttackerType.Enemy, damageAmount, skillType);
        }

        public void SetDamageAmount(float amount)
        {
            damageAmount = amount;
        }
    }
}