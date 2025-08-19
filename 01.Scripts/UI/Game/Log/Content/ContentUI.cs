using DG.Tweening;
using JMT.UISystem;
using System;
using UnityEngine;

namespace JMT.UISystem.Content
{
    public class ContentUI : MonoBehaviour, IOpenablePanel
    {
        public event Action OnOpenUIEvent;
        public event Action OnCloseUIEvent;

        [SerializeField] private CanvasGroup group;

        [Header("Duration Settings")]
        [SerializeField] private float fadeDuration = 0.3f;
        public CanvasGroup Group => group;
        public Transform GroupTrm => group.transform;
        public RectTransform GroupRectTrm => GroupTrm as RectTransform;

        private bool isOpen = false;

        public virtual void OpenPanel()
        {
            GroupTrm.localScale = new(1, 0);
            group.blocksRaycasts = true;
            group.interactable = true;

            Sequence seq = DOTween.Sequence();
            seq.Append(group.DOFade(1f, fadeDuration));
            seq.Join(GroupRectTrm.DOScaleY(1f, fadeDuration));
            seq.OnComplete(() =>
            {
                isOpen = true;
                OnOpenUIEvent?.Invoke();
            });
        }

        public virtual void ClosePanel()
        {
            group.blocksRaycasts = false;
            group.interactable = false  ;

            Sequence seq = DOTween.Sequence();
            seq.Append(group.DOFade(0f, fadeDuration));
            seq.Join(GroupRectTrm.DOScaleY(0f, fadeDuration));
            seq.OnComplete(() =>
            {
                isOpen = false;
                OnCloseUIEvent?.Invoke();
            });
        }
    }
}
