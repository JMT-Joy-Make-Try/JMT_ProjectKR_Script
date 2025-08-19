using System;
using System.Collections.Generic;
using UnityEngine;

namespace JMT.System.EventChannelSystem
{
    [CreateAssetMenu(fileName = "EventsChannel", menuName = "SO/Event/EventsChannel")]
    public class EventsChannelSO : ScriptableObject
    {
        private Dictionary<EventType, Delegate> _eventTable = new();

        public void AddListener(EventType type, Action listener)
        {
            if (_eventTable.TryGetValue(type, out var existing))
                _eventTable[type] = Delegate.Combine(existing, listener);
            else
                _eventTable[type] = listener;

            Debug.Log($"Listener added for EventType: {type}");
        }

        public void AddListener<T>(EventType type, Action<T> listener)
        {
            if (_eventTable.TryGetValue(type, out var existing))
                _eventTable[type] = Delegate.Combine(existing, listener);
            else
                _eventTable[type] = listener;
        }

        public void RemoveListener(EventType type, Action listener)
        {
            if (_eventTable.TryGetValue(type, out var existing))
            {
                existing = Delegate.Remove(existing, listener);
                if (existing == null) _eventTable.Remove(type);
                else _eventTable[type] = existing;
            }
        }

        public void RemoveListener<T>(EventType type, Action<T> listener)
        {
            if (_eventTable.TryGetValue(type, out var existing))
            {
                existing = Delegate.Remove(existing, listener);
                if (existing == null) _eventTable.Remove(type);
                else _eventTable[type] = existing;
            }
        }

        public void Invoke(EventType type)
        {
            if (_eventTable.TryGetValue(type, out var del) && del is Action action)
                action.Invoke();
            else
                Debug.LogWarning($"No Action found for EventType: {type}");
        }

        public void Invoke<T>(EventType type, T arg)
        {
            if (_eventTable.TryGetValue(type, out var del) && del is Action<T> action)
                action.Invoke(arg);
            else
                Debug.LogWarning($"No Action<{typeof(T).Name}> found for EventType: {type}");
        }

        public void ClearAllListeners()
        {
            _eventTable.Clear();
        }
    }
}