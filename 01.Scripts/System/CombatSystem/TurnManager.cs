using System.Collections;
using JMT.Core;
using JMT.Core.Tool;
using JMT.EventSystem;
using JMT.System.CombatSystem.Event;
using JMT.System.CombatSystem.VFX;
using JMT.System.GameSystem.Timer;
using JMT.UISystem;
using JMT.WaveSystem;
using UnityEngine;

namespace JMT.System.CombatSystem
{
    public class TurnManager : MonoSingleton<TurnManager>
    {
        [SerializeField] private PlayerTurnEventSO _playerTurnEventSO;
        [SerializeField] private EnemyTurnEventSO _enemyTurnEventSO;
        [SerializeField] private TurnType _currentTurn;
        

        
        private EnemySpawnModule _enemySpawnModule;

        public EnemyTurnEventSO EnemyTurnEventSO => _enemyTurnEventSO;
        public EnemySpawnModule SpawnModule => _enemySpawnModule;

        protected override void Awake()
        {
            base.Awake();
            _enemySpawnModule = GetComponent<EnemySpawnModule>();
        }

        private void Start()
        {
            _playerTurnEventSO?.AddListener(HandlePlayerTurnEvent);
            
            WaveManager.Instance.OnWaveTypeEvent += HandleWaveTypeEvent;
        }

        private void OnDestroy()
        {
            _playerTurnEventSO?.RemoveListener(HandlePlayerTurnEvent);
            _enemyTurnEventSO?.RemoveListener(HandleEnemyTurnEvent);
            if (WaveManager.Instance != null)
                WaveManager.Instance.OnWaveTypeEvent -= HandleWaveTypeEvent;
        }

        private void HandleWaveTypeEvent(WaveType type, GameEventSO currentEventSO)
        {
            if(currentEventSO is FightEventSO fightSO)
            {
                _enemySpawnModule.SetEnemy(fightSO.EnemySO);
                CombatManager.Instance.SetEnemy(_enemySpawnModule.SpawnEnemy());
                SkillVFXController.Instance.ResetData();
                SetTurnType(TurnType.Player);
            }
        }

        private void HandlePlayerTurnEvent(TurnPhase phase)
        {
            if (phase == TurnPhase.End)
            {
                Debug.Log("End Player Turn, Starting Enemy Turn");
                StartCoroutine(StartTurnDelay(TurnType.Enemy, 2f));
            }
            else
            {
                Debug.Log($"Player Turn Phase: {phase}");
            }
        }
        
        private void HandleEnemyTurnEvent(TurnPhase phase)
        {
            if (phase == TurnPhase.End)
            {
                Debug.Log("End Enemy Turn, Starting Player Turn");
                StartCoroutine(StartTurnDelay(TurnType.Player, 1.5f));
            }
            else
            {
                Debug.Log($"Enemy Turn Phase: {phase}");
            }
        }
        
        public IEnumerator StartTurnDelay(TurnType turnType, float delay)
        {
            if (turnType == TurnType.Enemy)
            {
                UIManager.Instance.DeckCompo.ClosePanel();
            }
            else if (turnType == TurnType.Player)
            {
                UIManager.Instance.DeckCompo.OpenPanel();
            }
            yield return WaitForSecondsCache.Get(delay);
            StartTurn(turnType);
        }

        public void StartTurn(TurnType turnType)
        {
            _currentTurn = turnType;
            if (turnType == TurnType.Player)
            {
                _playerTurnEventSO?.Invoke(TurnPhase.Start);
            }
            else if (turnType == TurnType.Enemy)
            {
                _enemyTurnEventSO?.Invoke(TurnPhase.Start);
            }
        }
        
        private void SetTurnType(TurnType turnType)
        {
            _currentTurn = turnType;
            Debug.Log($"Current Turn: {_currentTurn}");
        }
        public void SetEnemy(EnemyTurnEventSO enemyTurnEventSO)
        {
            _enemyTurnEventSO?.RemoveListener(HandleEnemyTurnEvent);
            _enemyTurnEventSO = enemyTurnEventSO;
            DeckTimer.Instance.SetEnemyTurnEventSO(_enemyTurnEventSO);
            _enemyTurnEventSO?.AddListener(HandleEnemyTurnEvent);
        }
    }

    public enum TurnType
    {
        Player,
        Enemy
    }

    public enum TurnPhase
    {
        Start,
        End
    }
}
