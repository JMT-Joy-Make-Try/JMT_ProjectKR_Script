using System;
using UnityEngine;

namespace JMT.System.AgentSystem.PlayerSystem.Component
{
    public class AnimationEndTrigger : MonoBehaviour
    {
        public event Action OnAnimationEnd;
        
        public void TriggerAnimationEnd()
        {
            OnAnimationEnd?.Invoke();
        }
    }
}