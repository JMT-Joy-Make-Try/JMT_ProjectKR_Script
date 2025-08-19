using JMT.System.CombatSystem;
using JMT.System.DataSystem;
using UnityEngine;

namespace JMT.System.AgentSystem.PlayerSystem.Component
{
    public class PlayerPassive : MonoBehaviour, IPlayerComponent
    {
        public Player Player { get; private set; }
        private PassiveListSO passiveListSO;
        public PassiveListSO PassiveList => passiveListSO;


        public void Init(Player player)
        {
            Player = player;
            passiveListSO = DataManager.Instance.OwnPassiveList;
        }
    }
}