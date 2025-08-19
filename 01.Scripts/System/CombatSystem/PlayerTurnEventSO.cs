using JMT.System.EventChannelSystem;
using UnityEngine;

namespace JMT.System.CombatSystem.Event
{
    [CreateAssetMenu(menuName = "SO/Event/PlayerTurnEvent")]
    public class PlayerTurnEventSO : EventChannelSO<TurnPhase> { }
}