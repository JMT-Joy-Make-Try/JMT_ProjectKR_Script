using JMT.System.AgentSystem.Enemy;
using UnityEngine;

namespace JMT.EventSystem
{
    [CreateAssetMenu(fileName = "Event_", menuName = "SO/Events/FightEventSO")]
    public class FightEventSO : SelectEventSO
    {
        // �� SO �ʿ���
        public EnemySO EnemySO;
    }
}
