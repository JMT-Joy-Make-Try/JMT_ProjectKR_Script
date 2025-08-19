using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace JMT.Core.Tool.DOTweenModule
{
    [CreateAssetMenu(fileName = "SequenceData", menuName = "SO/Tool/DOTween/SequenceSO")]
    public class SequenceSO : ScriptableObject
    {
        [SerializeField]
        private List<TweenDataWrapper> _sequenceList = new();

        public Sequence CreateSequence(Transform target)
        {
            var sequence = DOTween.Sequence();

            foreach (var wrapper in _sequenceList)
            {
                if (wrapper.tweenData == null) continue;

                var tween = wrapper.tweenData.GetTween(target);
                if (tween == null) continue;

                switch (wrapper.insertType)
                {
                    case TweenInsertType.Append:
                        sequence.Append(tween);
                        break;
                    case TweenInsertType.Join:
                        sequence.Join(tween);
                        break;
                    case TweenInsertType.Insert:
                        sequence.Insert(wrapper.insertTime, tween);
                        break;
                }
            }

            return sequence;
        }

        public void Play(Transform target, TweenCallback onComplete = null)
        {
            CreateSequence(target).Play().OnComplete(onComplete);
        }
    }
}
