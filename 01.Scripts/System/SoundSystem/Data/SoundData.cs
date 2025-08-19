using System;
using UnityEngine;
using UnityEngine.Audio;

namespace JMT.System.SoundSystem.Data
{
    [Serializable]
    public class SoundData
    {
        public AudioResource audioResource;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0f, 10f)]
        public float pitch = 1f;
        [TextArea]
        public string description;
    }
}