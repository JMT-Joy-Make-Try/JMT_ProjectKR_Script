using JMT.System.AgentSystem.Interface;
using JMT.System.CombatSystem;
using JMT.System.SkillSystem;
using UnityEngine;

namespace JMT.EventSystem
{
    [CreateAssetMenu(fileName = "Event_", menuName = "SO/Events/HealEventSO")]
    public class HealEventSO : GameEventSO
    {
        public float HealAmount;

        public void ExecuteHeal()
        {
            if (HealAmount <= 0) return;

            DamageResult damageResult = new DamageResult(-HealAmount, 0, 0, 0, 0, 0, SkillType.None);
            CombatManager.Instance.Player.AgentHealth.TakeDamage(damageResult);
        }
    }
}