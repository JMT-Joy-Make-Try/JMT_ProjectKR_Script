using DG.Tweening;
using JMT.System.Card.Slot;
using JMT.System.EffectSystem;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.System.Card
{
    public class SkillUseArea : MonoBehaviour
    {
        [SerializeField] private Image _outlineImage;
        [SerializeField] private SkillCardSlotChangeEvent _skillCardSlotChangeEvent;

        private void Start()
        {
            _skillCardSlotChangeEvent.AddListener(EnableArea);
            EnableArea(false);
        }

        void OnDestroy()
        {
            _skillCardSlotChangeEvent.RemoveListener(EnableArea);
        }

        private void EnableArea(bool enable)
        {
            if (enable)
            {
                _outlineImage.transform.DOScale(1f, 0.2f)
                    .SetEase(Ease.OutBack);
                EffectManager.Instance.Fade(0.4f, 0.4f);
            }
            else
            {
                _outlineImage.transform.DOScale(0f, 0.2f)
                    .SetEase(Ease.InBack);
                EffectManager.Instance.Fade(0.4f, 0f);
            }
        }
    }
}