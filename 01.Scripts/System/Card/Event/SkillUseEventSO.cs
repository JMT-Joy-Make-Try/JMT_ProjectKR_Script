using JMT.System.Card.CardData;
using JMT.System.EventChannelSystem;
using JMT.System.SkillSystem;
using UnityEngine;

namespace JMT.System.Card.Event
{
    [CreateAssetMenu(menuName = "SO/Event/SkillUseEvent")]
    public class SkillUseEventSO : EventChannelSO<SkillDataSO> { }
}