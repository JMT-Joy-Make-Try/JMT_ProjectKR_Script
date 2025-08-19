using DG.Tweening;
using UnityEngine;

namespace JMT.Core.Tool.DOTweenModule
{
    public enum ScaleAxis
    {
        XYZ, X, Y, Z
    }

    [CreateAssetMenu(fileName = "ScaleTweenData", menuName = "SO/Tool/DOTween/Scale")]
    public class ScaleTweenDataSO : TweenDataSO
    {
        [Header("Scale Tween Settings")]
        public Vector3 targetScale = Vector3.one;
        public float duration = 1f;
        public Ease ease = Ease.Linear;
        public ScaleAxis scaleAxis = ScaleAxis.XYZ;

        public override Tween GetTween(Transform target)
        {
            Tween tween = null;

            switch (scaleAxis)
            {
                case ScaleAxis.X:
                    tween = target.DOScaleX(targetScale.x, duration);
                    break;
                case ScaleAxis.Y:
                    tween = target.DOScaleY(targetScale.y, duration);
                    break;
                case ScaleAxis.Z:
                    tween = target.DOScaleZ(targetScale.z, duration);
                    break;
                case ScaleAxis.XYZ:
                    tween = target.DOScale(targetScale, duration);
                    break;
            }

            return tween.SetEase(ease);
        }
    }
}
