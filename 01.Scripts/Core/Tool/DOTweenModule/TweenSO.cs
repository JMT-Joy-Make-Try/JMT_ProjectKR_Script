using DG.Tweening;
using UnityEngine;

namespace JMT.Core.Tool.DOTweenModule
{
    public abstract class TweenDataSO : ScriptableObject
    {
        public abstract Tween GetTween(Transform target);
    }
}
