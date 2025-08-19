using UnityEngine;

namespace JMT.System.AgentSystem.Enemy.Component
{
    public class DefaultEnemyAnimator : EnemyAnimator<EnemyAnimatorState>
    {
    }

    public enum EnemyAnimatorState
    {
        Idle,
        Hit,
        Attack,
        Attack2,
        Dead
    }
}