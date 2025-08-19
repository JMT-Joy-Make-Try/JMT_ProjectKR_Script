using System;
using System.Collections.Generic;
using DG.Tweening;
using JMT.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace JMT.System.EffectSystem
{
    public class VolumeManager : MonoSingleton<VolumeManager>
    {
        [SerializeField] private List<Volume> _volumes;

        private Volume _currentVolume;

        private void Start()
        {
            if (_volumes.Count > 0)
            {
                _currentVolume = _volumes[0];
            }

            InitVolumes();
        }

        private void InitVolumes()
        {
            foreach (var volume in _volumes)
            {
                if (volume != null)
                {
                    volume.weight = 0f;
                }
            }

            _currentVolume.weight = 1f;
        }

        public void ChangeVolume(int index, float duration = 0.5f, Action onComplete = null, float delay = 0f)
        {
            if (index < 0 || index >= _volumes.Count)
            {
                Debug.LogWarning("Invalid volume index.");
                return;
            }

            DOTween.Kill(this); // 기존 트윈 중단
            var targetVolume = _volumes[index];
            targetVolume.weight = 0f; // 항상 0에서 시작

            // Sequence 사용
            DOTween.Sequence()
                .SetId(this)
                // 트윈
                .Append(DOVirtual.Float(_currentVolume.weight, 0f, duration, weight =>
                {
                    _currentVolume.weight = weight;
                    targetVolume.weight = 1f - weight;
                }))
                // 트윈이 끝난 뒤 delay
                .AppendInterval(delay)
                // onComplete 실행
                .OnComplete(() =>
                {
                    _currentVolume = targetVolume;
                    onComplete?.Invoke();
                });
        }

        public void SetVignette(float intensity, float duration)
        {
            if (_currentVolume.profile.TryGet<Vignette>(out var vignette))
            {
                DOTween.Kill(vignette);
                vignette.intensity.Override(0f); // 항상 0에서 시작
                DOTween.To(() => vignette.intensity.value, x => vignette.intensity.Override(x), intensity, duration)
                    .SetTarget(vignette);
            }
            else
            {
                Debug.LogWarning("Vignette component not found in the current volume profile.");
            }
        }

    }
}