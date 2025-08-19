using DG.Tweening;
using UnityEngine;

namespace JMT.System.Card.CardData
{
    [CreateAssetMenu(fileName = "CardAnimatorData", menuName = "SO/Card/CardAnimatorData")]
    public class CardAnimatorData : ScriptableObject
    {
        [Header("회전 설정")]
        [Tooltip("회전 양")] public float rotationAmount = 10f;
        
        [Header("그림자 설정")]
        [Tooltip("그림자 이동 거리")] public float shadowOffset = 10f;
        [Tooltip("그림자 원상복구 시간")] public float returnDuration = 0.2f;
        
        [Header("스케일 설정")]
        [Tooltip("카드가 커지는 양")] public float scaleAmount = 1.2f;
        [Tooltip("크기 확대 및 축소 시간")] public float scaleDuration = 0.2f;
        
        [Header("진동 설정")]
        public float shakeDuration = 0.5f;
        public Vector3 shakeStrength = new Vector3(0f, 0f, 1f);
        public int shakeVibrato = 10;
        public float shakeRandomness = 90f;
        public bool shakeFadeOut = true;
        public Ease shakeEase = Ease.OutQuad;
    }
}