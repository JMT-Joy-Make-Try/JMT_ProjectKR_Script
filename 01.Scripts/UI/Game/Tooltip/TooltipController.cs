using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JMT.UISystem.Tooltip
{
    public class TooltipController : MonoBehaviour
    {
        [SerializeField] private TooltipView view;

        private TooltipModel model = new();

        private bool isActiveTooltip;

        private void Awake()
        {
            view.OnTooltipActiveEvent += HandleTooltipActiveEvent;
        }

        private void Update()
        {
            if (!isActiveTooltip) return;

            bool isRight = model.IsRightPivot(view.TooltipAnchoredPos, view.TooltipSizeDelta, out Vector2 pivotVec);
            Vector3 rectVec = model.SetTooltipRectVec();
            view.SetTooltipPivot(isRight, pivotVec, rectVec);
        }

        public void ShowTooltip(bool isTrue, bool isStat, ITooltipable tooltipData = null)
            => view.ShowTooltip(isTrue, isStat, tooltipData);

        private void HandleTooltipActiveEvent(bool isTrue)
        {
            isActiveTooltip = isTrue;
        }
    }
}
