using JMT.EventSystem;
using JMT.System.Card;
using JMT.System.CombatSystem;
using JMT.System.DataSystem;
using JMT.System.DeckSettingSystem;
using JMT.WaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using JMT.Core.Tool;
using JMT.System.AgentSystem.Interface;
using JMT.System.SkillSystem;
using UnityEngine;
using UnityEngine.Events;

namespace JMT.UISystem.Content
{
    public class LogController : MonoBehaviour
    {
        [SerializeField] private GameEventListSO eventListSO;
        [SerializeField] private LogView view;

        public event Action OnFightIgnoreEvent;
        public event Action OnRepairEndEvent;

        private bool isBreakTurn = false;
        private Color currentColor = Color.white;

        private void Awake()
        {
            WaveManager.Instance.OnStartWaveEvent += HandleStartWaveEvent;
            WaveManager.Instance.OnWaveTypeEvent += HandleWaveTypeEvent;
        }

        private void OnDestroy()
        {
            if (WaveManager.Instance == null) return;
            WaveManager.Instance.OnStartWaveEvent -= HandleStartWaveEvent;
            WaveManager.Instance.OnWaveTypeEvent -= HandleWaveTypeEvent;
        }

        public void OpenPanel()
        {
            view.OpenPanel();
            OnRepairEndEvent?.Invoke();
        }

        public void ClosePanel()
        {
            view.ClosePanel();
        }

        private void HandleStartWaveEvent(int waveCount)
        {
            isBreakTurn = false;
            view.CreateWave(waveCount + 1);
        }

        private void HandleWaveTypeEvent(WaveType waveType, GameEventSO currentEventSO)
        {
            StartCoroutine(CreateWaveRoutine(waveType, currentEventSO));
        }

        private IEnumerator CreateWaveRoutine(WaveType waveType, GameEventSO currentEventSO)
        {
            List<ButtonEvent> currentEvents = new();
            List<UnityAction> testActions = new();
            
            if(!isBreakTurn && currentEventSO.eventColor != default)
                currentColor = currentEventSO.eventColor;

            switch (waveType)
            {
                case WaveType.Fight:
                case WaveType.Boss:
                    CardSlotManager.Instance.ResetCardSlots();
                    testActions.Add(() => WaveManager.Instance.StartFight());
                    testActions.Add(() =>
                    {
                        OnFightIgnoreEvent?.Invoke();
                        WaveManager.Instance.WaveStart();
                    });
                    break;
                case WaveType.Break:
                    if (currentEventSO is BreakEventSO breakEventSO)
                    {
                        testActions.Add(() => WaveManager.Instance.WaveStart(breakEventSO.FirstSelectData.GetRandomEvent()));
                        testActions.Add(() =>
                        {
                            WaveManager.Instance.WaveStart(breakEventSO.SecondSelectData
                                .GetRandomEvent());
                        });
                    }
                    break;
                case WaveType.Repair:
                    if (currentEventSO is RepairEventSO repairEventSO)
                    {
                        switch (repairEventSO.RepairEventType)
                        {
                            case RepairEventType.DeckSetting:
                                testActions.Add(() => {
                                    DeckSettingManager.Instance.DeckSettingUI.OpenPanel();
                                    UIManager.Instance.LogCompo.ClosePanel();
                                });
                                break;
                            case RepairEventType.Shop:
                                testActions.Add(() => {
                                    UIManager.Instance.ShopCompo.OpenPanel();
                                    UIManager.Instance.LogCompo.ClosePanel();
                                });
                                break;
                            case RepairEventType.Heal:
                                testActions.Add(() => {
                                    CombatManager.Instance.Player.AgentHealth.TakeDamage(new DamageResult
                                    {
                                        damageAmount = -50f,
                                        criticalChance = 0f,
                                        criticalDamage = 0f,
                                        skillType = SkillType.None,
                                        evasion = 0f
                                    });
                                    WaveManager.Instance.WaveStart();
                                    OnRepairEndEvent?.Invoke();
                                });
                                break;
                        }
                        repairEventSO.Spawn();
                    }
                    break;
            }
            testActions.Add(() =>
            {
                WaveManager.Instance.WaveStart();
                OnRepairEndEvent?.Invoke();
            });

            view.CreateEvent(currentColor, currentEventSO);
            yield return WaitForSecondsCache.Get(0.8f);
            if (!isBreakTurn && currentEventSO is SelectEventSO selectEventSO)
            {
                currentEvents = SetButtonEvents(selectEventSO, testActions);
                view.CreateSelect(currentColor, currentEvents);
                yield break;
            }
            else if (currentEventSO is ItemEventSO itemEventSO)
            {
                for (int i = 0; i < itemEventSO.ItemCount; i++)
                {
                    DataManager.Instance.AddItem(itemEventSO.ItemSO);
                }
            }
            else if (currentEventSO is HealEventSO heal)
            {
                heal.ExecuteHeal();
            }
            WaveManager.Instance.WaveStart();
        }

        private List<ButtonEvent> SetButtonEvents(SelectEventSO eventSO, List<UnityAction> actions)
        {
            List<ButtonEvent> currentEvents = eventSO.ButtonEvents;

            if (isBreakTurn)
                currentEvents.RemoveAt(1);

            if(currentEvents.Count > actions.Count)
            {
                Debug.LogError("actions은 currentEvents보다 많거나 같아야 합니다.");
                return null;
            }

            for(int i = 0; i < currentEvents.Count; i++)
            {
                currentEvents[i].Action = actions[i];
            }

            return currentEvents;
        }

        public void TestLog()
        {
            Debug.Log("테스트 로그입니다.");
        }
    }
}
