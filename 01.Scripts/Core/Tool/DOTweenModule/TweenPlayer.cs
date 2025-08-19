using UnityEngine;

namespace JMT.Core.Tool.DOTweenModule
{
    public class TweenPlayer : MonoBehaviour
    {
        [SerializeField] private SequenceSO sequenceSO;
        [SerializeField] private Transform target;
        [SerializeField] private bool playOnStart = true;

        private void Awake()
        {
            if (playOnStart)
            {
                PlaySequence();
            }
        }

        public void PlaySequence()
        {
            if (sequenceSO == null)
            {
                Debug.LogWarning("SequenceSO is not assigned.");
                return;
            }
            if (target == null)
            {
                target = transform;
            }

            sequenceSO.Play(target);
        }
    }
}