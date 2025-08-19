using System;
using System.Collections.Generic;
using JMT.System.AgentSystem.Enemy.Component;
using JMT.System.SkillSystem;
using JMT.System.SoundSystem.Core;
using UnityEngine;

namespace JMT.System.AgentSystem.Enemy.Boss
{
    public class Boss : Enemy
    {
        [SerializeField] private List<SkillSO> bossSkills;

        private int attackCount;

        protected override void AttackPlayer()
        {
            attackCount = UnityEngine.Random.Range(0, bossSkills.Count);
            if ((EnemyAnimatorState)attackCount + 2 == EnemyAnimatorState.Dead) attackCount -= 1;
            enemyAnimator.ChangeState((EnemyAnimatorState)attackCount + 2);
            Debug.Log($"Boss is attacking with skill: {(EnemyAnimatorState)attackCount + 2}");
        }

        protected override void HandleAttackPlayer()
        {
            soundPlayer?.PlaySound("Enemy_Attack", SoundType.SFX);
            if (EnemyStatSO.TryGetStat(EnemyStatType.PhysicalAttack, out var physicalAttackValue))
            {
                bossSkills[attackCount].Init(SkillType.PhysicalAttack, physicalAttackValue.GetValue());
            }
            else if (EnemyStatSO.TryGetStat(EnemyStatType.MagicAttack, out var magicAttackValue))
            {
                bossSkills[attackCount].Init(SkillType.MagicAttack, magicAttackValue.GetValue());
            }
            else
            {
                Debug.LogWarning("No valid attack type found for the boss.");
                //return;
            }
            bossSkills[attackCount].ExecuteSkill();
            enemyParticle?.PlayParticle(EnemyParticleType.Attack);  

        }
    }
}