using System;
using System.Collections;
using DG.Tweening;
using JMT.Core;
using JMT.System.CameraSystem;
using JMT.System.Card.CardData;
using JMT.System.Card.Event;
using JMT.System.CombatSystem.Event;
using JMT.System.GameSystem.Timer;
using JMT.System.SkillSystem;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.System.CombatSystem.VFX
{
    public class SkillVFXController : MonoSingleton<SkillVFXController>
    {
        // 이벤트 SO
        [SerializeField] private PlayerTurnEventSO _playerTurnEvent;
        [SerializeField] private SkillUseEventSO _skillUseEvent;
        [SerializeField] private Material _darkenMaterial;

        // 카드 이미지
        [SerializeField] private Image _cardImage;
        [SerializeField] private Image _skillImage;
        [SerializeField] private Image _cardColorImage;

        // 화면 레터박스
        [SerializeField] private RectTransform _upLetterImage;
        [SerializeField] private RectTransform _downLetterImage;

        [Header("Animation Setting")]
        [SerializeField] private SkillVFXDataSO _skillVFXData;


        private Material _cardMaterial;

        private bool _isEnemyDead;

        public void ResetData()
        {
            _isEnemyDead = false;
        }

        private void Start()
        {
            _playerTurnEvent?.AddListener(HandlePlayerTurnEvent);
            _skillUseEvent?.AddListener(HandleSkillUseEvent);
            CombatManager.Instance.OnDeathEvent += HandleDeathEvent;
            _cardMaterial = _cardImage.material;
        }


        private void OnDestroy()
        {
            if (CombatManager.Instance != null)
                CombatManager.Instance.OnDeathEvent -= HandleDeathEvent;
            _playerTurnEvent?.RemoveListener(HandlePlayerTurnEvent);
            _skillUseEvent?.RemoveListener(HandleSkillUseEvent);
        }

        private void HandleSkillUseEvent(SkillDataSO so)
        {
            _skillImage.sprite = so.Icon;
            _cardColorImage.color = so.Color == CardColor.Red ? Color.red : Color.white;
            _skillImage.color = so.Color == CardColor.Red ? Color.red : Color.white;
        }

        private void HandleDeathEvent(AttackerType type)
        {
            if (type == AttackerType.Enemy && !_isEnemyDead)
            {
                _isEnemyDead = true;
                
            }
        }

        private void HandlePlayerTurnEvent(TurnPhase phase)
        {
            if (DeckTimer.Instance.IsTimerEnd)
            {
                return; // 타이머가 끝났으면 아무것도 하지 않음
            }
            if (phase == TurnPhase.End && !_isEnemyDead)
            {
                StartCoroutine(ZoomCoroutine());
            }
        }

        private IEnumerator ZoomCoroutine()
        {
            // 카메라 위치
            Vector3 playerPos = CombatManager.Instance.Player.transform.position + new Vector3(5, 3.5f);
            Vector3 cameraPos = CameraManager.Instance.CinemachineCamera.transform.position;

            // 어둡게 처리
            _darkenMaterial.DOColor(_skillVFXData.DarkenColor, "_Color", _skillVFXData.DarkenDuration);

            // 초기 상태 설정
            Transform cardTransform = _cardImage.transform;
            cardTransform.localScale = Vector3.one;
            cardTransform.localRotation = Quaternion.identity;
            _cardMaterial.SetFloat("_DissolveHeight", 1f);

            Sequence cardSequence = DOTween.Sequence();

            cardSequence.Append(_upLetterImage.DOAnchorPosY(0, _skillVFXData.LetterAnimationDuration).SetEase(_skillVFXData.LetterAnimationEase));
            cardSequence.Join(_downLetterImage.DOAnchorPosY(0, _skillVFXData.LetterAnimationDuration).SetEase(_skillVFXData.LetterAnimationEase));

            // 카드 애니메이션
            cardSequence.Join(cardTransform.DOScale(_skillVFXData.ScaleAmount, _skillVFXData.ScaleDuration)); // 커짐
            cardSequence.Join(cardTransform.DOShakeRotation(_skillVFXData.ShakeDuration, _skillVFXData.ShakeStrength)); // 흔들림
            cardSequence.JoinCallback(() => CameraManager.Instance.ImpulseModule.TriggerImpulse(0.1f));

            cardSequence.AppendInterval(0.2f);

            cardSequence.JoinCallback(() =>
            {
                _darkenMaterial.DOColor(_skillVFXData.NormalColor, "_Color", _skillVFXData.DarkenDuration);
                _cardMaterial.DOFloat(0f, "_DissolveHeight", _skillVFXData.DissolveDuration);
                _upLetterImage.DOAnchorPosY(200, _skillVFXData.LetterAnimationDuration).SetEase(_skillVFXData.LetterEndAnimationEase);
                _downLetterImage.DOAnchorPosY(-200, _skillVFXData.LetterAnimationDuration).SetEase(_skillVFXData.LetterEndAnimationEase);
            });

            cardSequence.AppendCallback(() =>
            {   // 카드 줌(플레이어 위치로 이동)
                CameraManager.Instance.ZoomModule.Zoom(_skillVFXData.ZoomAmount, _skillVFXData.ZoomDuration, playerPos);
            });
            cardSequence.AppendInterval(0.3f);
            cardSequence.AppendCallback(() =>
            {
                CameraManager.Instance.ZoomModule.Zoom(_skillVFXData.NormalZoomAmount, _skillVFXData.ZoomDuration, cameraPos);
            });

            cardSequence.Append(cardTransform.DOScale(0f, _skillVFXData.ScaleEndDuration).SetEase(_skillVFXData.ScaleEase)); // 확 작아짐

            

            yield return cardSequence.WaitForCompletion();
        }

    }
}