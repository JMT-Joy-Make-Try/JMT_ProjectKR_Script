using System;
using UnityEngine;

namespace JMT.System.AgentSystem.Enemy.Component
{
    public class AttackTrigger : MonoBehaviour
    {
        public event Action OnAttack;
        public event Action OnAttackEnd;

        public void TriggerAttack()
        {
            OnAttack?.Invoke();
        }

        public void TriggerAttackEnd()
        {
            OnAttackEnd?.Invoke();
        }
    }
}