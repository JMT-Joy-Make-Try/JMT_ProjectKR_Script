using DG.Tweening;
using System;
using UnityEngine;

namespace JMT.UISystem
{
    public class FadeUI : MonoBehaviour, IOpenablePanel
    {
        public event Action OnOpenUIEvent;
        public event Action OnCloseUIEvent;

        [SerializeField] private CanvasGroup group;
        [SerializeField] private float fadeDuration = 0.3f;
        public CanvasGroup Group => group;
        public Transform GroupTrm => group.transform;
        public RectTransform GroupRectTrm => GroupTrm as RectTransform;

        private bool isOpen = false;

        public virtual void OpenPanel()
        {
            group.blocksRaycasts = true;
            group.interactable = true;
            group.DOFade(1f, fadeDuration)
                .SetUpdate(true)
                .OnComplete(() =>
                    {
                        isOpen = true;
                        OnOpenUIEvent?.Invoke();
                    });
        }

        public virtual void ClosePanel()
        {
            group.blocksRaycasts = false;
            group.interactable = false;
            group.DOFade(0f, fadeDuration)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    isOpen = false;
                    OnCloseUIEvent?.Invoke();
                });
        }
    }
}
