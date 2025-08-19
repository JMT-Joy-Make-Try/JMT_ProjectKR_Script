using UnityEngine;

namespace JMT.System.EffectSystem
{
    public class EffectPlayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem effectPrefab;
        [SerializeField] private Transform effectSpawnPoint;

        [Header("Effect Settings")]
        [SerializeField] private float _lifeTime = 2f;

        public void PlayEffect()
        {
            if (effectPrefab != null && effectSpawnPoint != null)
            {
                ParticleSystem effectInstance = Instantiate(effectPrefab, effectSpawnPoint.position, effectSpawnPoint.rotation);
                effectInstance.Play();
                Destroy(effectInstance.gameObject, _lifeTime);
            }
            else
            {
                Debug.LogWarning("Effect prefab or spawn point is not assigned.");
            }
        }
    }
}