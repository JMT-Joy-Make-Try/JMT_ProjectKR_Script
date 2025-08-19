using System;
using System.Collections.Generic;
using JMT.Core.Tool;
using UnityEngine;

namespace JMT.System.StatSystem
{
    [Serializable]
    public class StatData<T> where T : Enum
    {
        /// <summary>
        /// StatData의 타입.
        /// Enum 타입을 사용하여 다양한 종류의 Stat을 정의.
        /// </summary>
        [field: SerializeField] public T Type { get; private set; }

        /// <summary>
        /// StatData의 기본값.
        /// </summary>
        [SerializeField] public float DefaultValue;

        /// <summary>
        /// StatData에 적용될 수 있는 계산할 값들
        /// </summary>
        [SerializeField] public List<StatModifier> Modifiers = new();
        [SerializeField] public Sprite Icon;

        [Header("Clamp Setting")]
        [SerializeField] private bool _isClampValue;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;

        public StatData(T type, float defaultValue)
        {
            Type = type;
            DefaultValue = defaultValue;
            Modifiers ??= new List<StatModifier>();
        }

        public float GetValue()
        {
            float value = DefaultValue;

            if (Modifiers == null || Modifiers.Count == 0)
                return value;

            foreach (var modifier in Modifiers)
            {
                if (StatExtension.operations.TryGetValue(modifier.ModifierType, out var operation))
                {
                    value = operation(value, modifier.GetEffectiveValue());
                }
                else
                {
                    Debug.LogWarning($"[StatData] Unsupported modifier type: {modifier.ModifierType}");
                }
            }

            if (_isClampValue)
            {
                value = Mathf.Clamp(value, _minValue, _maxValue);
            }

            return value;
        }

        public void AddModifier(StatModifier modifier)
        {
            if (modifier == null) return;
            Modifiers ??= new List<StatModifier>();
            Modifiers.Add(modifier);
        }

        public void RemoveModifier(StatModifier modifier)
        {
            if (modifier == null || Modifiers == null) return;
            Modifiers.Remove(modifier);
        }
    }
}
