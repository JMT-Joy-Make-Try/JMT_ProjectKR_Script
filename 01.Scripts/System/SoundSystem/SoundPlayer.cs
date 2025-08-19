using System.Collections.Generic;
using JMT.System.SoundSystem.Data;
using UnityEngine;
using System.Linq;

namespace JMT.System.SoundSystem.Core
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundSO soundData;
        [SerializeField] private int audioSourcePoolSize = 10;

        private List<AudioSource> _audioSources;

        private void Awake()
        {
            _audioSources = new List<AudioSource>();
            for (int i = 0; i < audioSourcePoolSize; i++)
            {
                var source = gameObject.AddComponent<AudioSource>();
                _audioSources.Add(source);
            }
        }

        public void PlaySound(string key, SoundType soundType = SoundType.SFX)
        {
            if (!soundData.sounds.TryGetValue(key, out var sound))
            {
                Debug.LogWarning($"Sound with key {key} not found.");
                return;
            }

            var source = GetAvailableOrNewAudioSource();

            if (sound.audioResource is AudioClip)
            {
                source.pitch = sound.pitch;
                source.volume = sound.volume;
            }
            source.resource = sound.audioResource;
            source.loop = soundType == SoundType.BGM;
            source.Play();
        }

        public void StopSound(string key)
        {
            if (!soundData.sounds.TryGetValue(key, out var sound))
            {
                Debug.LogWarning($"Sound with key {key} not found.");
                return;
            }

            foreach (var source in _audioSources)
            {
                if (source.clip == sound.audioResource && source.isPlaying)
                {
                    source.Stop();
                }
            }
        }

        private AudioSource GetAvailableOrNewAudioSource()
        {
            foreach (var source in _audioSources)
            {
                if (!source.isPlaying)
                    return source;
            }

            // 모두 사용 중이면 새 AudioSource 추가 생성
            var newSource = gameObject.AddComponent<AudioSource>();
            _audioSources.Add(newSource);
            return newSource;
        }
    }

    public enum SoundType
    {
        BGM,
        SFX,
    }
}
