using JMT.System.AgentSystem.Enemy;
using UnityEngine;

namespace JMT.EventSystem
{
    [CreateAssetMenu(fileName = "Event_", menuName = "SO/Events/FightEventSO")]
    public class FightEventSO : SelectEventSO
    {
        // 적 SO 필요함
        public EnemySO EnemySO;
    }
}
