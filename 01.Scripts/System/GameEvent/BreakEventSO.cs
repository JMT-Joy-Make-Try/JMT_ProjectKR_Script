using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JMT.EventSystem
{
    [Serializable]
    public struct SelectData
    {
        public List<GameEventSO> selectEvent;

        public GameEventSO GetRandomEvent()
            => selectEvent[Random.Range(0, selectEvent.Count)];
    }


    [CreateAssetMenu(fileName = "Event_", menuName = "SO/Events/BreakEventSO")]
    public class BreakEventSO : SelectEventSO
    {
        public SelectData FirstSelectData;
        public SelectData SecondSelectData;
    }
}
