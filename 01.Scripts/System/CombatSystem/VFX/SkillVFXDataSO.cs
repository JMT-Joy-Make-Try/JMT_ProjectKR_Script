using DG.Tweening;
using UnityEngine;

namespace JMT.System.CombatSystem.VFX
{
    [CreateAssetMenu(fileName = "SkillVFXData", menuName = "SO/VFX/SkillVFXDataSO")]
    public class SkillVFXDataSO : ScriptableObject
    {
        [Header("Zoom Settings")]
        public float ZoomAmount = 25f;
        public float NormalZoomAmount = 27f;
        public float ZoomDuration = 0.2f;

        [Header("Darken Settings")]
        [ColorUsage(true)] public Color DarkenColor = new Color(0.04f, 0.04f, 0.26f, 0.4f);
        [ColorUsage(true)] public Color NormalColor = new Color(0.04f, 0.04f, 0.26f, 0f);
        public float DarkenDuration = 0.2f;

        [Header("Dissolve Settings")]
        public float DissolveDuration = 0.75f;

        [Header("Scale Settings")]
        public float ScaleAmount = 1.5f;
        public float ScaleDuration = 0.4f;
        public float ScaleEndDuration = 0.2f;
        public Ease ScaleEase = Ease.InBack;

        [Header("Shake Settings")]
        public float ShakeDuration = 0.4f;
        public Vector3 ShakeStrength = new Vector3(0, 0, 15f);
        public float CameraShakeDelay = 0.2f;
        public float CameraShakeDuration = 0.2f;
        public Vector3 CameraImpulseStrength = new Vector3(0.6f, 0f, 0f);

        [Header("Letter Animation Settings")]
        public float LetterAnimationDuration = 0.2f;
        public Ease LetterAnimationEase = Ease.InSine;
        public Ease LetterEndAnimationEase = Ease.OutSine;
    }
}