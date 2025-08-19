using UnityEngine;

namespace JMT.UISystem
{
    interface IOpenablePanel
    {
        public CanvasGroup Group { get; }
        public Transform GroupTrm { get; }
        public RectTransform GroupRectTrm { get; }
        void OpenPanel();
        void ClosePanel();
    }
}
