using JMT.System.CombatSystem;
using UnityEngine;

namespace JMT.EventSystem
{
    [CreateAssetMenu(fileName = "Event_", menuName = "SO/Events/ItemEventSO")]
    public class ItemEventSO : GameEventSO
    {
        public ItemSO ItemSO;
        public int ItemCount = 1;
    }
}