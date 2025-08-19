using DG.Tweening;
using UnityEngine;

namespace JMT.Core.Tool.DOTweenModule
{
    [CreateAssetMenu(fileName = "FadeTweenData", menuName = "SO/Tool/DOTween/Fade")]
    public class FadeTweenDataSO : TweenDataSO
    {
        [Header("Fade Tween Settings")]
        [Range(0f, 1f)] public float targetAlpha = 1f;
        public float duration = 1f;
        public Ease ease = Ease.Linear;

        public override Tween GetTween(Transform target)
        {
            var canvasGroup = target.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogWarning($"[FadeTweenDataSO] CanvasGroup 없음: {target.name}");
                return null;
            }

            return canvasGroup.DOFade(targetAlpha, duration).SetEase(ease);
        }
    }
}
