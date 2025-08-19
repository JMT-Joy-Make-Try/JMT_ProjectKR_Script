using System.Collections.Generic;
using UnityEngine;

namespace JMT.System.EffectSystem
{
    public class EffectList : MonoBehaviour
    {
        [SerializeField] private List<EffectPlayer> effectPlayers;

        public void PlayAllEffects()
        {
            foreach (var effectPlayer in effectPlayers)
            {
                effectPlayer.PlayEffect();
            }
        }
    }
}