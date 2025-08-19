using JMT.Core;
using JMT.EventSystem;
using JMT.UISystem;
using System;
using System.Collections;
using System.Collections.Generic;
using JMT.Core.Tool;
using JMT.System.CombatSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using JMT.DialogueSystem;
using JMT.System.GameSystem.Timer;

namespace JMT.WaveSystem
{
    public class WaveManager : MonoSingleton<WaveManager>
    {
        public event Action<int> OnStartWaveEvent;
        public event Action<WaveType, GameEventSO> OnWaveTypeEvent;

        [SerializeField] private List<WaveType> waveProgress;
        [SerializeField] private GameEventListSO gameEventListSO;
        [SerializeField] private bool _isTutorial;

        private GameEventSystem eventSystem;

        private int waveCount = -1;
        private int waveProgressCount => waveCount % waveProgress.Count;

        public int CurrentWave => waveCount + 1;

        protected override void Awake()
        {
            base.Awake();
            eventSystem = new(gameEventListSO);
        }

        private void Start()
        {
            WaveStart();
        }

        public void WaveStart()
        {
            StartCoroutine(WaveStartRoutine());
        }

        private IEnumerator WaveStartRoutine()
        {
            waveCount++;
            OnStartWaveEvent?.Invoke(waveCount);

            GameEventSO currentEventSO = null;

            yield return WaitForSecondsCache.Get(1.2f);
            switch (waveProgress[waveProgressCount])
            {
                case WaveType.Fight:
                    currentEventSO = eventSystem.GetRandomFightEventSO();
                    break;
                case WaveType.Boss:
                    currentEventSO = eventSystem.GetRandomBossEventSO();
                    break;
                case WaveType.Break:
                    currentEventSO = eventSystem.GetRandomBreakEventSO();
                    break;
                case WaveType.Repair:
                    currentEventSO = eventSystem.GetRandomRepairEventSO();
                    break;
            }
            OnWaveTypeEvent?.Invoke(waveProgress[waveProgressCount], currentEventSO);
        }

        public void WaveStart(GameEventSO currentEventSO)
        {
            WaveType waveType = WaveType.Fight;
            switch (currentEventSO)
            {
                case FightEventSO:
                    waveType = WaveType.Fight;
                    break;
                case BreakEventSO:
                    waveType = WaveType.Break;
                    break;
                case RepairEventSO:
                    waveType = WaveType.Repair;
                    break;
                case GameEventSO:
                    waveType = WaveType.Text;
                    break;
            }
            Debug.Log($"Wave Start: {waveType}");
            OnWaveTypeEvent?.Invoke(waveType, currentEventSO);
        }

        public void StartFight()
        {
            if (_isTutorial)
            {
                DialogueManager.Instance.StartDialogue();
            }
            StartCoroutine(FightRoutine());
        }

        private IEnumerator FightRoutine()
        {
            yield return WaitForSecondsCache.Get(0.8f);
            UIManager.Instance.LogCompo.ClosePanel();
            StartCoroutine(TurnManager.Instance.StartTurnDelay(TurnType.Player, 0f));
            DeckTimer.Instance.StartTimer();
        }
    }
}
