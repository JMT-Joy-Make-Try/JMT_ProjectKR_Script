using System;
using System.Collections.Generic;
using UnityEngine;

namespace JMT.System.AgentSystem.PlayerSystem
{
    [CreateAssetMenu(fileName = "PlayerStat", menuName = "SO/Agent/PlayerStat")]
    public class PlayerStatSO : AgentStat<PlayerStat, PlayerStatType>
    {
        public void RemoveAllModifiers()
        {
            foreach (var stat in stats)
            {
                stat.Modifiers.Clear();
            }
        }
    }
}