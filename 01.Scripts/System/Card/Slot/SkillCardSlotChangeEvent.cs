using JMT.System.EventChannelSystem;
using UnityEngine;

namespace JMT.System.Card.Slot
{
    [CreateAssetMenu(fileName = "SkillCardSlotChangeEvent", menuName = "SO/Event/SkillCardSlotChangeEvent")]
    public class SkillCardSlotChangeEvent : EventChannelSO<bool> {}
}