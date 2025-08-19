using JMT.System.EventChannelSystem;
using UnityEngine;

namespace JMT.System.CombatSystem.Event
{
    [CreateAssetMenu(menuName = "SO/Event/EnemyTurnEvent")]
    public class EnemyTurnEventSO : EventChannelSO<TurnPhase> { }
}