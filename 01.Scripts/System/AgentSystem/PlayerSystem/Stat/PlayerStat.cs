using System;
using JMT.System.StatSystem;

namespace JMT.System.AgentSystem.PlayerSystem
{
    [Serializable]
    public class PlayerStat : StatData<PlayerStatType>
    {
        public PlayerStat(PlayerStatType type, float defaultValue) : base(type, defaultValue)
        {
        }
    }
}