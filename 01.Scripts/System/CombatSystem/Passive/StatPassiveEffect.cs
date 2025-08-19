using System;
using JMT.Core.Tool;
using JMT.System.StatSystem;
using JMT.UISystem.Tooltip;

namespace JMT.System.CombatSystem
{
    [Serializable]
    public class StatPassiveEffect : IPassiveEffect
    {
        public ItemTag tag;
        public NormalStatModifier statModifier;

        public float GetValue() => statModifier.GetSignedValue();
    }
}