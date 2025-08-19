using System;

namespace JMT.System.CombatSystem
{
    [Serializable]
    public class ChancePassiveEffect : IPassiveEffect
    {
        public string effectId;
        public float probability;
        public float GetProbability() => probability;
    }

}