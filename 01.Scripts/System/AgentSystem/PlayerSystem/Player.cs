using JMT.System.AgentSystem.PlayerSystem.Component;
using JMT.System.AgentSystem.Enemy.Component;
using JMT.System.AgentSystem.Interface;
using JMT.System.SoundSystem.Core;
using System.Collections.Generic;
using JMT.System.Card.CardData;
using JMT.System.CombatSystem;
using JMT.System.SkillSystem;
using JMT.System.Card.Event;
using JMT.System.DataSystem;
using JMT.System.StatSystem;
using JMT.UISystem.Tooltip;
using JMT.WaveSystem;
using JMT.Core.Tool;
using JMT.UISystem;
using UnityEngine;
using System;
using JMT.System.CameraSystem;

namespace JMT.System.AgentSystem.PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerStatSO statSO;
        [SerializeField] private PlayerPassive playerPassive;
        [SerializeField] private AgentHealth agentHealth;
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private SkillUseEventSO skillUseEvent;
        [SerializeField] private AnimationEndTrigger animationEndTrigger;
        [SerializeField] private AttackTrigger attackTrigger;
        [SerializeField] private PlayerWeapon playerWeapon;
        [SerializeField] private PlayerParticle playerParticle;
        [SerializeField] private SoundPlayer soundPlayer;


        public AgentHealth AgentHealth => agentHealth;
        public PlayerAnimator PlayerAnimator => playerAnimator;
        public PlayerStatSO StatSO => statSO;
        public PlayerPassive PlayerPassive => playerPassive;

        private SkillDataSO _skillCardData;

        private float _beforeHealth;


        private void Start()
        {
            statSO = Instantiate(statSO);
            playerPassive?.Init(this);
            agentHealth.Init(statSO.GetStat(PlayerStatType.Health).GetValue());
            CombatManager.Instance.SetPlayer(this);
            playerAnimator?.Init(this);

            _beforeHealth = agentHealth.CurrentHealth;

            agentHealth.OnDead += HandleDead;
            agentHealth.OnHealthChange += HandleHealthChange;
            agentHealth.OnTakeDamage += HandleTakeDamage;
            MapManager.Instance.OnMapMoveEvent += HandleMove;
            animationEndTrigger.OnAnimationEnd += () => playerAnimator?.ChangeState(PlayerAnimatorState.Idle);
            skillUseEvent?.AddListener(HandleSkillUse);
            attackTrigger.OnAttack += HandleAttack;
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.A))
            {
                agentHealth.TakeDamage(new DamageResult
                {
                    damageAmount = -10f,
                    criticalChance = 0f,
                    criticalDamage = 0f,
                    skillType = SkillType.None,
                    evasion = 0f
                });
            }
#endif
        }



        private void OnDestroy()
        {
            agentHealth.OnDead -= HandleDead;
            agentHealth.OnHealthChange -= HandleHealthChange;
            agentHealth.OnTakeDamage -= HandleTakeDamage;
            if (MapManager.Instance != null)
                MapManager.Instance.OnMapMoveEvent -= HandleMove;
            skillUseEvent?.RemoveListener(HandleSkillUse);
            attackTrigger.OnAttack -= HandleAttack;
        }

        private void HandleTakeDamage()
        {
            statSO.RemoveAllModifiers();
        }

        private void HandleAttack()
        {
            soundPlayer?.PlaySound("Player_Attack", SoundType.SFX);
            if (_skillCardData.TryGetStatDataValue(SkillType.PhysicalAttack, out float attackValue))
            {
                Attack(SkillType.PhysicalAttack, ItemTag.PhysicalAttack, attackValue);
            }
            else if (_skillCardData.TryGetStatDataValue(SkillType.MagicAttack, out float magicAttackValue))
            {
                Attack(SkillType.MagicAttack, ItemTag.MagicAttack, magicAttackValue);
            }
            else
            {
                Debug.LogWarning("No attack value found in skill card data.");
            }
        }

        private void Attack(SkillType skillType, ItemTag itemTag, float value)
        {
            Debug.Log(value);
            value = StatExtension.operations[StatModifierType.Percent](value, playerPassive.PassiveList.GetValue(itemTag));
            if (agentHealth.CurrentHealth < agentHealth.MaxHealth * 0.4f)
            {
                int attack = Mathf.FloorToInt(playerPassive.PassiveList.GetChanceValue("Leaf"));
                for (int i = 0; i < attack; i++)
                {
                    value += 10;
                }
            }
            value = Mathf.Max(value, 0f);
            CombatManager.Instance.TakeDamage(AttackerType.Player, value, skillType);
        }

        private void HandleHealthChange(float curHealth, float maxHealth, SkillType skillType)
        {
            Debug.Log($"Player health changed: {curHealth}/{maxHealth}");

            if (curHealth < _beforeHealth)
            {
                playerAnimator?.ChangeStateImmediately(PlayerAnimatorState.Hit);
                playerParticle?.PlayParticle(PlayerParticleType.Hit);
                soundPlayer?.PlaySound("Player_Hit");
                CameraManager.Instance.ImpulseModule.TriggerImpulse(0.2f);
            }
            else if (curHealth >= _beforeHealth)
            {
                playerParticle?.PlayParticle(PlayerParticleType.Heal);
                soundPlayer?.PlaySound("Player_Heal");
            }
            _beforeHealth = curHealth;
        }

        private void HandleSkillUse(SkillDataSO so)
        {
            SetStatValue(so, SkillType.PhysicalDefense, PlayerStatType.PhysicalDefense);
            SetStatValue(so, SkillType.MagicDefense, PlayerStatType.MagicDefense);
            SetStatValue(so, SkillType.Evasion, PlayerStatType.Evasion, value => StatExtension.operations[StatModifierType.Percent](value, playerPassive.PassiveList.GetValue(ItemTag.Evasion)));
            SetStatValue(so, SkillType.CriticalChance, PlayerStatType.CriticalChance, value => StatExtension.operations[StatModifierType.Percent](value, playerPassive.PassiveList.GetValue(ItemTag.CriticalChance)));
            SetStatValue(so, SkillType.CriticalDamage, PlayerStatType.CriticalDamage, value => StatExtension.operations[StatModifierType.Percent](value, playerPassive.PassiveList.GetValue(ItemTag.CriticalDamage)));

            _skillCardData = so;
            playerAnimator?.ChangeState(PlayerAnimatorState.CardAttack);
            var playerWeaponInstance = Instantiate(playerWeapon);
            playerWeaponInstance.SetWeaponColor(so.Color == CardColor.Red ? Color.red : Color.white);
        }

        public void SetStatValue(SkillDataSO so, SkillType skillType, PlayerStatType playerStatType, Func<float, float> operation = null)
        {
            if (so.TryGetStatDataValue(skillType, out float value))
            {
                value = operation?.Invoke(value) ?? value;
                statSO.GetStat(playerStatType).AddModifier(new StatModifier(StatModifierType.Addition, value));
            }
        }

        private void HandleMove(bool isMove)
        {
            playerAnimator?.ChangeState(isMove ? PlayerAnimatorState.Run : PlayerAnimatorState.Idle);
            playerParticle?.SetParticleEnable(PlayerParticleType.Run, isMove);
        }

        private List<ItemSO> RemoveDuplicatesByName(List<ItemSO> list)
        {
            HashSet<string> seenNames = new();
            List<ItemSO> result = new();

            foreach (var item in list)
            {
                if (seenNames.Add(item.Name))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        private void HandleDead()
        {
            Debug.Log("Player has died.");
            CombatManager.Instance.DeathAgent(AttackerType.Player);

            var distinctSkillCards = RemoveDuplicatesByName(new List<ItemSO>(DataManager.Instance.OwnSkillCardList.skillData));
            var distinctStatCards = RemoveDuplicatesByName(new List<ItemSO>(DataManager.Instance.OwnStatCardList));

            UIManager.Instance.ResultCompo.OpenPanel(
                WaveManager.Instance.CurrentWave,
                DataManager.Instance.CoinCount,
                distinctSkillCards,
                distinctStatCards
            );
            playerAnimator?.ChangeState(PlayerAnimatorState.Death);
        }

        public float GetStatValue(PlayerStatType type)
        {
            var stat = statSO.GetStat(type);
            if (stat == null)
            {
                Debug.LogWarning($"Stat {type} not found in PlayerStatSO.");
                return 0f;
            }
            var statValue = stat.GetValue();
            return statValue;
        }
    }
}