using System;
using JMT.Core;
using JMT.System.AgentSystem;
using JMT.System.AgentSystem.Enemy;
using JMT.System.AgentSystem.Interface;
using JMT.System.AgentSystem.PlayerSystem;
using JMT.System.SkillSystem;
using UnityEngine;

namespace JMT.System.CombatSystem
{
    public class CombatManager : MonoSingleton<CombatManager>
    {
        private Player player;
        private Enemy _currentEnemy;

        public Player Player => player;
        public Enemy CurrentEnemy => _currentEnemy;

        public event Action<AttackerType> OnDeathEvent;

        public void SetEnemy(Enemy enemy)
        {
            _currentEnemy = enemy;
            Debug.Log($"Current Enemy Set: {_currentEnemy.name}");
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
            Debug.Log($"Current Player Set: {player.name}");
        }

        public void TakeDamage(AttackerType attackerType, float damage, SkillType skillType)
        {
            if (attackerType == AttackerType.Player)
            {
                if (_currentEnemy != null)
                {
                    DamageResult damageResult = new DamageResult
                                        (damage, _currentEnemy.GetStatValue(EnemyStatType.Evasion),
                                        _currentEnemy.GetStatValue(EnemyStatType.MagicDefense),
                                        _currentEnemy.GetStatValue(EnemyStatType.PhysicalDefense),
                                        player.GetStatValue(PlayerStatType.CriticalChance),
                                        player.GetStatValue(PlayerStatType.CriticalDamage),
                                        skillType);
                    _currentEnemy.AgentHealth.TakeDamage(damageResult);
                }
                else
                {
                    Debug.LogWarning("No enemy set to take damage.");
                }
            }
            else if (attackerType == AttackerType.Enemy)
            {
                if (player != null)
                {
                    DamageResult damageResult = new DamageResult
                                        (damage, player.GetStatValue(PlayerStatType.Evasion),
                                        player.GetStatValue(PlayerStatType.MagicDefense),
                                        player.GetStatValue(PlayerStatType.PhysicalDefense),
                                        _currentEnemy.GetStatValue(EnemyStatType.CriticalChance),
                                        _currentEnemy.GetStatValue(EnemyStatType.CriticalDamage),
                                        skillType);
                    player.AgentHealth.TakeDamage(damageResult);
                }
                else
                {
                    Debug.LogWarning("No player set to take damage.");
                }
            }
        }

        public void DeathAgent(AttackerType attackerType)
        {
            OnDeathEvent?.Invoke(attackerType);
            Debug.Log($"{attackerType} has died.");
        }
    }

    public enum AttackerType
    {
        Player,
        Enemy
    }
}