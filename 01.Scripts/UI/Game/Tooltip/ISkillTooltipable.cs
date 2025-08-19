using JMT.System.AgentSystem.PlayerSystem;
using JMT.UISystem.Tooltip;
using System.Collections.Generic;

namespace JMT
{
    public interface ISkillTooltipable
    {
        string Name { get; }
        List<ItemTag> Tags { get; }
        List<PlayerStat> Stats { get; }
        string Desc { get; }
    }
}