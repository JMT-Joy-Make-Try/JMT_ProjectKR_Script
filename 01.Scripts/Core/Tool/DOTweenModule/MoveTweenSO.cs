using DG.Tweening;
using UnityEngine;

namespace JMT.Core.Tool.DOTweenModule
{
    [CreateAssetMenu(fileName = "MoveTweenData", menuName = "SO/Tool/DOTween/Move")]
    public class MoveTweenDataSO : TweenDataSO
    {
        public Vector3 endValue;
        public float duration = 1f;
        public float delay = 0f;
        public Ease ease = Ease.Linear;
        public bool isLocal = false;

        public override Tween GetTween(Transform target)
        {
            var tween = isLocal
                ? target.DOLocalMove(endValue, duration)
                : target.DOMove(endValue, duration);

            return tween.SetEase(ease).SetDelay(delay);
        }
    }
}
