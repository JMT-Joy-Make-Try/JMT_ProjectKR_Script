using DG.Tweening;
using UnityEngine;

namespace JMT.UISystem
{
    public class SideUI : FadeUI
    {
        [SerializeField] private float outRange = -50f;

        public override void OpenPanel()
        {
            base.OpenPanel();
            GroupRectTrm.DOAnchorPosX(outRange, 0.3f).SetUpdate(true);
        }

        public override void ClosePanel()
        {
            base.ClosePanel();
            GroupRectTrm.DOAnchorPosX(GroupRectTrm.rect.width, 0.3f).SetUpdate(true);
        }
    }
}
