using JMT.System.CombatSystem;
using JMT.System.MapSystem;
using UnityEngine;

namespace JMT.EventSystem
{
    [CreateAssetMenu(fileName = "Event_", menuName = "SO/Events/RepairEventSO")]
    public class RepairEventSO : SelectEventSO
    {
        public RepairEventType RepairEventType;
        public RepairMapObject RepairMapObject;

        private RepairMapObject _repairMapObject;

        public void Spawn()
        {
            _repairMapObject = Instantiate(RepairMapObject, RepairMapObject.transform.position, RepairMapObject.transform.rotation);
        }

    }

    public enum RepairEventType
    {
        DeckSetting,
        Shop,
        Heal
    }
}
