using System.Collections.Generic;
using UnityEngine;

namespace JMT.EventSystem
{
    [CreateAssetMenu(fileName = "EventList", menuName = "SO/Events/EventListSO")]
    public class GameEventListSO : ScriptableObject
    {
        public List<FightEventSO> fightEvents;
        public List<FightEventSO> bossEvents;
        public List<BreakEventSO> breakEvents;
        public List<RepairEventSO> repairEvents;

        public Color fightEventColor;
        public Color bossEventColor;
        public Color breakEventColor;
        public Color repairEventColor;
    }
}
