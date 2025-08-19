using System;
using System.Collections.Generic;
using JMT.System.Card;
using UnityEngine;

namespace JMT.UISystem.Deck
{
    public class DeckController : MonoBehaviour
    {
        [SerializeField] private DeckView view;
        private bool isPanelOpen;
        
        public event Action OnOpenPanelEvent;

        public void OpenPanel()
        {
            if (isPanelOpen) return;
            view.OpenPanel();
            isPanelOpen = true;
            OnOpenPanelEvent?.Invoke();
        }

        public void ClosePanel()
        {
            if (!isPanelOpen) return;
            view.ClosePanel();
            isPanelOpen = false;
        }
    }
}