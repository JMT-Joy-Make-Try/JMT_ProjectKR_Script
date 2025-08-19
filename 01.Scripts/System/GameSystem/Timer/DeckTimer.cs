using System;
using System.Collections;
using JMT.Core;
using JMT.Core.Tool;
using JMT.System.CombatSystem;
using JMT.System.CombatSystem.Event;
using UnityEngine;

namespace JMT.System.GameSystem.Timer
{
    public class DeckTimer : MonoSingleton<DeckTimer>
    {
        [SerializeField] private TimeData timeData;
        [SerializeField] private PlayerTurnEventSO playerTurnEventSO;
        [SerializeField] private TimerUI timerUI;
        public event Action<float, float> OnTimeUpdate;
        public bool IsTimerEnd { get; private set; }
        private EnemyTurnEventSO enemyTurnEventSO;

        private Coroutine timerCoroutine;

        private void Start()
        {
            playerTurnEventSO.AddListener(HandlePlayerTurnEvent);
        }

        void OnDestroy()
        {
            playerTurnEventSO.RemoveListener(HandlePlayerTurnEvent);
        }

        private void HandlePlayerTurnEvent(TurnPhase phase)
        {
            if (phase == TurnPhase.End)
            {
                StopTimer();
            }
        }

        public void StartTimer()
        {
            if (playerTurnEventSO == null)
            {
                Debug.LogError("PlayerTurnEventSO is not assigned.");
                return;
            }

            if (timerCoroutine == null)
            {
                timerCoroutine = StartCoroutine(TimerCoroutine());
            }
        }

        private void StopTimer()
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }

            IsTimerEnd = false;
            timerUI?.ClosePanel();
        }

        private IEnumerator TimerCoroutine()
        {
            IsTimerEnd = false;
            TimeData cloneTimeData = (TimeData)timeData.Clone();
            timerUI.OpenPanel();
            timerUI.UpdateTimerUI(cloneTimeData.minutes, cloneTimeData.seconds);
            while (cloneTimeData.minutes > 0 || cloneTimeData.seconds > 0)
            {
                yield return WaitForSecondsCache.Get(1f);

                if (cloneTimeData.seconds == 0)
                {
                    if (cloneTimeData.minutes > 0)
                    {
                        cloneTimeData.minutes--;
                        cloneTimeData.seconds = 59;
                    }
                }
                else
                {
                    cloneTimeData.seconds--;
                }

                Debug.Log($"Time left: {cloneTimeData.minutes}:{cloneTimeData.seconds}");
                OnTimeUpdate?.Invoke(cloneTimeData.minutes, cloneTimeData.seconds);
                timerUI?.UpdateTimerUI(cloneTimeData.minutes, cloneTimeData.seconds);
            }

            IsTimerEnd = true;
            timerUI?.ClosePanel();
            playerTurnEventSO?.Invoke(TurnPhase.End);
        }

        public void SetEnemyTurnEventSO(EnemyTurnEventSO enemyTurnEventSO)
        {
            enemyTurnEventSO?.RemoveListener(HandleEnemyTurnEvent);
            this.enemyTurnEventSO = enemyTurnEventSO;
            enemyTurnEventSO?.AddListener(HandleEnemyTurnEvent);
        }

        private void HandleEnemyTurnEvent(TurnPhase phase)
        {
            if (CombatManager.Instance.Player.AgentHealth.IsDead)
            {
                Debug.Log("Player is dead, stopping timer.");
                StopTimer();
                return;
            }
            if (phase == TurnPhase.End)
            {
                StartTimer();
            }
        }
    }

    [Serializable]
    public struct TimeData : ICloneable
    {
        public int minutes;
        public int seconds;

        public object Clone()
        {
            return new TimeData
            {
                minutes = this.minutes,
                seconds = this.seconds
            };
        }
    }
}
