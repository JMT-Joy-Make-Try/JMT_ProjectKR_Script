using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace JMT.System.EffectSystem
{
    public class ParticlePlayer<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] private SerializedDictionary<T, ParticleSystem> _particles;

        public void PlayParticle(T type)
        {
            if (_particles.TryGetValue(type, out ParticleSystem particle))
            {
                particle.Play();
            }
            else
            {
                Debug.LogWarning($"Particle of type {type} not found.");
            }
        }

        public void StopParticle(T type)
        {
            if (_particles.TryGetValue(type, out ParticleSystem particle))
            {
                particle.Stop();
            }
            else
            {
                Debug.LogWarning($"Particle of type {type} not found.");
            }
        }

        public void SetParticleEnable(T type, bool enable)
        {
            if (enable)
            {
                PlayParticle(type);
            }
            else
            {
                StopParticle(type);
            }
        }
    }
}