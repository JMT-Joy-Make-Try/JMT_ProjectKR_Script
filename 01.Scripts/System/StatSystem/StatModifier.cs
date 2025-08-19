using System;
using System.Collections.Generic;
using JMT.Core.Tool;
using UnityEngine;

namespace JMT.System.StatSystem
{
    [Serializable]
    public class StatModifier
    {
        [Header("스탯의 기본값")]
        public StatModifierType ModifierType;
        public float Value;

        [Header("랜덤 적용 여부")]
        public bool IsRandom;

        public float MinValue;
        public float MaxValue;

        [Header("최소/최대값 중 하나만 선택")]
        public bool IsMinOrMax; // true면 Min 또는 Max, false면 Min~Max 사이 랜덤



        public StatModifier(StatModifierType modifierType, float value)
        {
            ModifierType = modifierType;
            Value = value;
            IsRandom = false;
            IsMinOrMax = false;
        }

        public StatModifier(StatModifierType modifierType, float minValue, float maxValue, bool isMinOrMax = false)
        {
            ModifierType = modifierType;
            IsRandom = true;
            MinValue = Mathf.Min(minValue, maxValue);
            MaxValue = Mathf.Max(minValue, maxValue);
            IsMinOrMax = isMinOrMax;
        }

        /// <summary>
        /// 실제 적용되는 값을 반환합니다. 랜덤이면 범위 내 랜덤값, 아니면 고정값.
        /// IsMinOrMax가 true면 Min 또는 Max 중 하나를 반환합니다.
        /// </summary>
        public float GetEffectiveValue()
        {
            if (IsRandom)
            {
                if (IsMinOrMax)
                    return UnityEngine.Random.value < 0.5f ? MinValue : MaxValue;
                else
                    return UnityEngine.Random.Range(MinValue, MaxValue);
            }
            else
            {
                return Value;
            }
        }

        public float CalculateModifier(float baseValue)
        {
            if (StatExtension.operations == null)
            {
                Debug.LogWarning("[StatModifier] Operations not initialized. Initializing now.");
            }
            if (StatExtension.operations.TryGetValue(ModifierType, out var operation))
            {
                return operation(baseValue, GetEffectiveValue());
            }
            throw new ArgumentOutOfRangeException();
        }

        internal float GetSignedValue()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class NormalStatModifier
    {
        public StatModifierType ModifierType;
        public float Value;

        public NormalStatModifier(StatModifierType modifierType, float value)
        {
            ModifierType = modifierType;
            Value = value;
        }

        public float CalculateModifier(float baseValue)
        {
            if (StatExtension.operations == null)
            {
                Debug.LogWarning("[NormalStatModifier] Operations not initialized. Initializing now.");
            }
            if (StatExtension.operations.TryGetValue(ModifierType, out var operation))
            {
                return operation(baseValue, Value);
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
