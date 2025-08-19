using UnityEngine;
using DG.Tweening;

namespace JMT.Core.Tool
{
    public static class DOTweenExtension
    {
        public static Tweener DOColor(this Material material, Color endValue, float duration)
        {
            return DOTween.To(() => material.color, x => material.color = x, endValue, duration);
        }

        public static Tweener DOFloat(this float value, float endValue, float duration)
        {
            return DOTween.To(() => value, x => value = x, endValue, duration);
        }
    }
}
