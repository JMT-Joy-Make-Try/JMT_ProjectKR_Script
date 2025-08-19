using JMT.System.AgentSystem.Enemy;
using JMT.System.EventChannelSystem;
using UnityEngine;
using EventType = JMT.System.EventChannelSystem.EventType;

namespace JMT.System.CombatSystem
{
    public class EnemySpawnModule : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [Header("Tutorial")]
        [SerializeField] private bool _isTutorial;
        [SerializeField] private EventsChannelSO eventsChannelSO;
        private EnemySO _currentEnemy;
        
        public void SetEnemy(EnemySO enemy)
        {
            _currentEnemy = enemy;
            //Debug.Log($"Current Enemy Set: {enemy.name}");
        }

        public Enemy SpawnEnemy(Vector3 spawnPosition)
        {
            return Instantiate(_currentEnemy.enemyPrefab, spawnPosition, Quaternion.identity);
        }
        
        public Enemy SpawnEnemy()
        {
            if (_isTutorial)
            {
                eventsChannelSO.Invoke(EventType.EnemySpawn);
            }
            if (_currentEnemy == null)
            {
                Debug.LogError("No enemy set to spawn.");
                return null;
            }

            if (_spawnPoint == null)
            {
                Debug.LogError("Spawn point is not set.");
                return null;
            }

            return Instantiate(_currentEnemy.enemyPrefab, _spawnPoint.position, Quaternion.identity);
        }
    }
}