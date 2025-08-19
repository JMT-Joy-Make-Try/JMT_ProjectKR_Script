using System;
using System.Collections.Generic;
using UnityEngine;

namespace JMT.System.EventChannelSystem
{
    public class EventChannelSO<T> : ScriptableObject
    {
        private event Action<T> OnEventRaised;

        public void AddListener(Action<T> listener)
        {
            OnEventRaised += listener;
        }

        public void RemoveListener(Action<T> listener)
        {
            OnEventRaised -= listener;
        }

        public void Invoke(T value)
        {
            OnEventRaised?.Invoke(value);
        }

        public void ClearListeners()
        {
            OnEventRaised = null;
        }

        private void OnDestroy()
        {
            ClearListeners();
        }
    }

    public class EventChannelSO : ScriptableObject
    {
        private event Action OnEventRaised;

        public void AddListener(Action listener)
        {
            OnEventRaised += listener;
        }

        public void RemoveListener(Action listener)
        {
            OnEventRaised -= listener;
        }

        public void Invoke()
        {
            OnEventRaised?.Invoke();
        }

        public void ClearListeners()
        {
            OnEventRaised = null;
        }
    }

    public enum EventType
    {
        EnemySpawn,
        CardDraw,
        CardAttach,
        SkillCardUse,
        EnemyDead
    }
}
