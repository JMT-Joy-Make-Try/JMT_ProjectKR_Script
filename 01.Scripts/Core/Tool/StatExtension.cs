using System;
using System.Collections.Generic;
using JMT.System.StatSystem;
using UnityEngine;

namespace JMT.Core.Tool
{
    public static class StatExtension
    {

        public static float GetSignedValue(this NormalStatModifier modifier)
        {
            return modifier.ModifierType switch
            {
                StatModifierType.Percent => modifier.Value,
                StatModifierType.Percent_Minus => -modifier.Value,
                _ => 0f
            };
        }

        public static Dictionary<StatModifierType, Func<float, float, float>> operations = new()
            {
                { StatModifierType.Addition,        (a, b) => a + b },
                { StatModifierType.Subtraction,     (a, b) => a - b },
                { StatModifierType.Multiplicative,  (a, b) => a * b },
                { StatModifierType.Division,        (a, b) => b != 0 ? a / b : throw new DivideByZeroException("StatModifier: Division by zero.") },
                { StatModifierType.Percent,      (a, b) => a * (1 + b / 100f) },
                { StatModifierType.Percent_Minus,   (a, b) => a * (1 - b / 100f) },
                { StatModifierType.Percent_Value,   (a, b) => a * b / 100f }
            };

    }
}