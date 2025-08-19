using System;
using UnityEngine;

namespace JMT.System.MapSystem
{
    public class VisibilityTracker : MonoBehaviour
    {
        public event Action<bool> OnVisibilityChanged;

        private void OnBecameVisible()
        {
            OnVisibilityChanged?.Invoke(true);
        }

        private void OnBecameInvisible()
        {
            OnVisibilityChanged?.Invoke(false);
        }
    }
}