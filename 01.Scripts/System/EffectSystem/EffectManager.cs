using System;
using DG.Tweening;
using JMT.Core;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.System.EffectSystem
{
    public class EffectManager : MonoSingleton<EffectManager>
    {
        [Header("Image")]
        [SerializeField] private Image _darkImage;

        public void Fade(float duration, float targetAlpha, Action onComplete = null)
        {
            if (_darkImage == null)
            {
                Debug.LogWarning("Dark image is not assigned.");
                return;
            }

            _darkImage.DOFade(targetAlpha, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}