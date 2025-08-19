using JMT.EventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JMT.UISystem.Content
{
    public class LogView : SideUI
    {
        [Header("UI Content Settings")]
        [SerializeField] private Transform logContentTrm;
        [SerializeField] private EventContentUI eventContent;
        [SerializeField] private SelectContentUI selectContent;
        [SerializeField] private WaveContentUI waveContent;

        public void CreateEvent(Color eventColor, GameEventSO eventSO)
        {
            EventContentUI content = Instantiate(eventContent, logContentTrm);
            content.SetContentUI(eventColor, eventSO);
            content.OpenPanel();
        }

        public void CreateSelect(Color currentEventColor, List<ButtonEvent> buttonEvents)
        {
            SelectContentUI content = Instantiate(selectContent, logContentTrm);
            content.SetContentUI(currentEventColor, buttonEvents);
            content.OpenPanel();
        }

        public void CreateWave(int waveCount)
        {
            WaveContentUI content = Instantiate(waveContent, logContentTrm);
            content.SetContentUI(waveCount);
            content.OpenPanel();
        }
    }
}
