using System;
using System.Collections.Generic;
using JMT.System.StatSystem;
using UnityEngine;

namespace JMT.System.AgentSystem
{
    public class AgentStat<T, TU> : ScriptableObject where T : StatData<TU> where TU : Enum
    {
        public List<T> stats;

        public T GetStat(TU type)
        {
            return stats.Find(stat => EqualityComparer<TU>.Default.Equals(stat.Type, type));
        }
    }
}