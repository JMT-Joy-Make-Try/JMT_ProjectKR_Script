using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace JMT.System.SoundSystem.Data
{
    [CreateAssetMenu(fileName = "SoundSO", menuName = "SO/Sound/SoundSO")]
    public class SoundSO : ScriptableObject
    {
        public SerializedDictionary<string, SoundData> sounds;
    }
}