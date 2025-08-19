using UnityEngine;

namespace JMT.EventSystem
{
    public class GameEventSystem
    {
        private GameEventListSO eventListSO;

        public GameEventSystem(GameEventListSO eventListSO)
        {
            this.eventListSO = eventListSO;
        }

        public FightEventSO GetRandomFightEventSO()
        {
            FightEventSO eventSO = eventListSO.fightEvents[Random.Range(0, eventListSO.fightEvents.Count)];
            eventSO.eventColor = eventListSO.fightEventColor;
            return eventSO;
        }

        public BreakEventSO GetRandomBreakEventSO()
        {
            BreakEventSO eventSO = eventListSO.breakEvents[Random.Range(0, eventListSO.breakEvents.Count)];
            eventSO.eventColor = eventListSO.breakEventColor;
            return eventSO;
        }

        public FightEventSO GetRandomBossEventSO()
        {
            FightEventSO eventSO = eventListSO.bossEvents[Random.Range(0, eventListSO.bossEvents.Count)];
            eventSO.eventColor = eventListSO.bossEventColor;
            return eventSO;
        }

        public RepairEventSO GetRandomRepairEventSO()
        {
            RepairEventSO eventSO = eventListSO.repairEvents[Random.Range(0, eventListSO.repairEvents.Count)];
            eventSO.eventColor = eventListSO.repairEventColor;
            return eventSO;
        }
    }
}
