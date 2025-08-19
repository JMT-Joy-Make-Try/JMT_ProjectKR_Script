using UnityEngine;
using UnityEngine.InputSystem;

namespace JMT.UISystem.Tooltip
{
    public class TooltipModel
    {
        public bool IsRightPivot(Vector2 tooltipAnchoredPos, Vector2 tooltipSizeDelta, out Vector2 resultPivot)
        {
            float halfWidth = Screen.width / 2f;
            float halfHeight = Screen.height / 2f;
            float pivotX = 0f;
            float pivotY = 1f;

            if (halfWidth < tooltipAnchoredPos.x + tooltipSizeDelta.x)
            {
                pivotX = 1f;
            }

            if (halfHeight < tooltipAnchoredPos.y + tooltipSizeDelta.y)
            {
                pivotY = 1f; // 위쪽 기준
            }
            else if (-halfHeight > tooltipAnchoredPos.y - tooltipSizeDelta.y)
            {
                pivotY = 0f; // 아래쪽 기준
            }

            resultPivot = new Vector2(pivotX, pivotY);
            return pivotX == 1f;
        }


        public Vector3 SetTooltipRectVec()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            return new(mousePos.x, mousePos.y, 0);
        }
    }
}