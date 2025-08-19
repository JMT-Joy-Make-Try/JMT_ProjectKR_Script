using System;
using DG.Tweening;
using JMT.System.Card.CardData;
using JMT.System.Card.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JMT.System.Card.Component
{
    public class CardAnimator : MonoBehaviour, ICardComponent, IPointerEnterHandler, IPointerExitHandler
    {
        public ICard Card { get; private set; }

        private RectTransform _rectTransform;
        private bool _isPointerOver = false;

        [SerializeField] private RectTransform _shadowRect;
        [SerializeField] private CardAnimatorData _data;

        private Tween _scaleTween;
        private Tween _rotateTween;
        private Tween _shakeTween;

        public void Init(ICard card)
        {
            Card = card;
            _rectTransform = Card.CardTransform.GetRectTransform();
            _rectTransform.localScale = Vector3.one;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;

            _rotateTween?.Kill(false);
            _scaleTween?.Kill(false);

            _scaleTween = _rectTransform.DOScale(_data.scaleAmount, _data.scaleDuration)
                .SetEase(Ease.OutBack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;

            _rotateTween?.Kill(false);
            _scaleTween?.Kill(false);

            _rotateTween = _rectTransform.DORotate(Vector3.zero, _data.returnDuration)
                .SetEase(Ease.OutQuad);

            _scaleTween = _rectTransform.DOScale(1f, _data.scaleDuration)
                .SetEase(Ease.InBack);

            if (_shadowRect != null)
            {
                _shadowRect.DOAnchorPos(Vector2.zero, _data.returnDuration)
                    .SetEase(Ease.OutQuad);
            }
        }

        private void Update()
        {
            if (!_isPointerOver || _rectTransform == null) return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, Input.mousePosition, null, out Vector2 localMousePos))
                return;

            float width = _rectTransform.rect.width;
            float height = _rectTransform.rect.height;

            float normalizedX = Mathf.Clamp((localMousePos.x / width) * 2f, -1f, 1f);
            float normalizedY = Mathf.Clamp((localMousePos.y / height) * 2f, -1f, 1f);

            float rotX = -normalizedY * _data.rotationAmount;
            float rotY = normalizedX * _data.rotationAmount;
            _rectTransform.localRotation = Quaternion.Euler(rotX, rotY, 0);

            if (_shadowRect != null)
            {
                Vector2 shadowOffset = new Vector2(-normalizedX, -normalizedY) * _data.shadowOffset;
                _shadowRect.anchoredPosition = shadowOffset;
            }
        }

        public void ShakeCard(bool isLoop = false)
        {
            if (_rectTransform == null)
            {
                Debug.LogError("CardAnimator.ShakeCard: RectTransform is not set.");
                return;
            }

            _shakeTween?.Kill(false);

            _shakeTween = _rectTransform.DOShakeRotation(
                duration: _data.shakeDuration,
                strength: _data.shakeStrength,
                vibrato: _data.shakeVibrato,
                randomness: _data.shakeRandomness,
                fadeOut: !isLoop
            ).SetEase(_data.shakeEase);

            if (isLoop)
            {
                _shakeTween.SetLoops(-1, LoopType.Yoyo);
            }
        }

        public void StopShake()
        {
            if (_rectTransform == null)
            {
                Debug.LogError("CardAnimator.StopShake: RectTransform is not set.");
                return;
            }

            _shakeTween?.Kill(false);
            _rotateTween?.Kill(false);
            _scaleTween?.Kill(false);

            _rectTransform.localRotation = Quaternion.identity;
        }

        private void OnDestroy()
        {
            _shakeTween?.Kill();
            _rotateTween?.Kill();
            _scaleTween?.Kill();
        }
    }
}

