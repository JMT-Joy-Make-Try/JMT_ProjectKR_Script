using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JMT.EventSystem
{
    [Serializable]
    public class ButtonEvent
    {
        public string ButtonText;
        public string SelectText;
        public UnityAction Action;
    }

    [CreateAssetMenu(fileName = "Event_", menuName = "SO/Events/GameEventSO")]
    public class GameEventSO : ScriptableObject
    {
        public string EventName;
        [TextArea(3,1)]
        public string EventDesc;

        [HideInInspector] public Color eventColor;
    }
}
