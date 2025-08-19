using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.System.Card.UI
{
    public class SkillCardUseAnimation : MonoBehaviour
    {
        private Image _skillCardImage;

        private void Awake()
        {
            _skillCardImage = GetComponent<Image>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayUseAnimation();
            }
        }
        public void PlayUseAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() => transform.localScale = Vector3.zero);
            sequence.Append(transform.DOScale(Vector3.one * 2f, 0.2f));
            sequence.Append(transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine));
            sequence.Append(transform.DOPunchScale(Vector3.zero, 0.5f, 1, 1));
            sequence.Append(transform.DOScale(Vector3.one * 100f, 0.5f));
            sequence.Join(_skillCardImage.DOFade(0f, 0.5f).SetEase(Ease.InOutSine));
        }
    }
}