using System;
using System.Collections.Generic;
using UnityEngine;

namespace JMT.System.CombatSystem
{
    [CreateAssetMenu(fileName = "Passive", menuName = "SO/Combat/Passive")]
    public class PassiveSO : ItemSO
    {
        public string passiveName;
        public string passiveDescription;
        public Sprite passiveIcon;
        public int passivePrice;

        public List<PassiveEffect> passive;
    }

    [Serializable]
    public class PassiveEffect
    {
        public ChancePassiveEffect chanceEffect;
        public StatPassiveEffect statEffect;
    }
}