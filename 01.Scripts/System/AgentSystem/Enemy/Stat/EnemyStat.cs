using System;
using JMT.System.StatSystem;

namespace JMT.System.AgentSystem.Enemy
{
    [Serializable]
    public class EnemyStat : StatData<EnemyStatType>
    {
        public EnemyStat(EnemyStatType type, float defaultValue) : base(type, defaultValue)
        {
        }
    }
}