using System.Collections.Generic;
using UnityEngine;

namespace JMT.Core.Tool
{
    public static class WaitForSecondsCache
    {
        private static readonly Dictionary<float, WaitForSeconds> _cache
            = new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds Get(float time)
        {
            // 소수 둘째자리까지 반올림 (예: 1.234 → 1.23)
            float rounded = Mathf.Round(time * 100f) / 100f;

            if (!_cache.TryGetValue(rounded, out var wait))
            {
                wait = new WaitForSeconds(rounded);
                _cache[rounded] = wait;
            }
            return wait;
        }
    }
}

