using System.Collections.Generic;
using UnityEngine;

namespace JMT.System.AgentSystem.Enemy
{
    [CreateAssetMenu(fileName = "EnemyStat", menuName = "SO/Agent/EnemyStat")]
    public class EnemyStatSO : AgentStat<EnemyStat, EnemyStatType>
    {
        public bool TryGetStat(EnemyStatType type, out EnemyStat stat)
        {
            stat = GetStat(type);
            if (stat == null)
            {
                Debug.LogWarning($"Stat of type {type} not found in {name}");
                return false;
            }
            return true;
        }
    }
}