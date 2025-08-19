using JMT.System.AgentSystem.PlayerSystem.Component;
using JMT.System.AgentSystem.Enemy.Component;
using JMT.System.AgentSystem.Enemy.Boss;
using JMT.System.AgentSystem.Interface;
using JMT.System.CombatSystem.Event;
using JMT.System.EventChannelSystem;
using JMT.System.SoundSystem.Core;
using System.Collections.Generic;
using JMT.System.CombatSystem;
using JMT.System.EffectSystem;
using JMT.System.CameraSystem;
using JMT.System.SkillSystem;
using JMT.System.DataSystem;
using System.Collections;
using JMT.WaveSystem;
using JMT.Core.Tool;
using JMT.UISystem;
using DG.Tweening;
using UnityEngine;
using JMT.Core;
using System;

namespace JMT.System.AgentSystem.Enemy
{
    public class Enemy : MonoBehaviour, ISpawnable
    {
        [Header("Scriptable Objects")]
        [SerializeField] private EnemyStatSO enemyStatSO;
        [SerializeField] private EnemySO enemySO;
        [SerializeField] private EnemyTurnEventSO enemyTurnEventSO;

        [Header("Components")]
        [SerializeField] protected DefaultEnemyAnimator enemyAnimator;
        [SerializeField] private AnimationEndTrigger animationEndTrigger;
        [SerializeField] private AttackTrigger enemyAttackTrigger;
        [SerializeField] private Renderer enemyRenderer;
        [SerializeField] protected SoundPlayer soundPlayer;
        [SerializeField] protected EnemyParticle enemyParticle;
        [SerializeField] private float _enemyMoveDuration = 1f;
        [SerializeField] private bool _isSkillUseDark = false;

        [Header("Skill")]
        [SerializeField] private SkillDataListSO skillDataList;

        [Header("Tutorial")]
        [SerializeField] private bool isTutorialEnemy = false;
        [SerializeField] private EventsChannelSO eventsChannelSO;

        private AgentHealth _agentHealth;
        public AgentHealth AgentHealth => _agentHealth;
        public EnemyStatSO EnemyStatSO => enemyStatSO;

        private Material _enemyMaterial;
        private List<float> _skillProbabilities = new List<float>();

        private float _beforeHealth;
        private void Awake()
        {
            _agentHealth = GetComponent<AgentHealth>();
            animationEndTrigger = GetComponentInChildren<AnimationEndTrigger>();
            enemyParticle = GetComponent<EnemyParticle>();
        }

        protected virtual void Start()
        {
            enemyStatSO = Instantiate(enemyStatSO);
            skillDataList = Instantiate(skillDataList);

            _enemyMaterial = Instantiate(enemyRenderer.material);
            enemyRenderer.material = _enemyMaterial;
            _enemyMaterial.SetFloat(ShaderConst.Alpha, 1f);

            TurnManager.Instance.SetEnemy(enemyTurnEventSO);

            _agentHealth.Init(enemyStatSO.GetStat(EnemyStatType.Health).GetValue());
            enemyAnimator?.Init(EnemyAnimatorState.Idle);
            skillDataList.Init();

            _beforeHealth = _agentHealth.CurrentHealth;

            OnSpawn();
            AddEvents();
            Initialization();
        }

        private void Initialization()
        {
            foreach (var skill in skillDataList.skillData)
            {
                if (skill == null) continue;
                float probability = skill.probability;
                if (probability > 0f)
                {
                    _skillProbabilities.Add(probability);
                }
            }
        }

        private void AddEvents()
        {
            _agentHealth.OnDead += HandleDead;
            _agentHealth.OnHealthChange += HandleChangeHealth;
            enemyTurnEventSO.AddListener(HandleEnemyTurnEvent);
            animationEndTrigger.OnAnimationEnd += HandleAnimationEnd;
            if (enemyAttackTrigger != null)
            {
                enemyAttackTrigger.OnAttack += HandleAttackPlayer;
                enemyAttackTrigger.OnAttackEnd += HandleAttackEnd;
            }
            UIManager.Instance.LogCompo.OnFightIgnoreEvent += OnFightIgnoreEvent;
        }

        

        private void HandleAnimationEnd()
        {
            enemyAnimator.ChangeState(EnemyAnimatorState.Idle);
        }

        private void HandleEnemyTurnEvent(TurnPhase phase)
        {
            if (AgentHealth.IsDead)
            {
                Debug.LogWarning($"{enemySO.name} is already dead, cannot handle enemy turn event.");
                return;
            }
            if (phase == TurnPhase.Start)
            {
                AttackPlayer();
            }
        }

        protected virtual void AttackPlayer()
        {
            enemyAnimator.ChangeState(EnemyAnimatorState.Attack);
        }

        private void HandleAttackEnd()
        {
            enemyTurnEventSO?.Invoke(TurnPhase.End);
        }

        protected virtual void HandleAttackPlayer()
        {
            soundPlayer?.PlaySound("Enemy_Attack");
            SkillDataSO skillData = ProbabilitySelector.SelectByProbability(skillDataList.skillData, _skillProbabilities);
            if (skillData == null)
            {
                Debug.LogWarning("SkillData is null, cannot perform attack.");
                return;
            }
            if (skillData.skillSO is AttackSkill attackSkill)
            {
                if (enemyStatSO.TryGetStat(EnemyStatType.PhysicalAttack, out var physicalAttackValue))
                {
                    attackSkill.Init(SkillType.PhysicalAttack, physicalAttackValue.GetValue());
                }
                else if (enemyStatSO.TryGetStat(EnemyStatType.MagicAttack, out var magicAttackValue))
                {
                    attackSkill.Init(SkillType.MagicAttack, magicAttackValue.GetValue());
                }
            }
            skillData.UseSkill();

            int selectIndex = skillDataList.skillData.IndexOf(skillData);
            if (selectIndex < 0) return;
            int count = _skillProbabilities.Count;
            _skillProbabilities[selectIndex] -= (count - 1) * 5;
            for (int i = 0; i < count; i++)
            {
                if (i == selectIndex) continue;
                _skillProbabilities[i] += 5;
            }
            if (_isSkillUseDark)
            {
                VolumeManager.Instance.ChangeVolume(1, 0.4f, () =>
                {
                    VolumeManager.Instance.ChangeVolume(0, 4f);
                }, 0.3f);
                EffectManager.Instance.Fade(0.4f, 0.4f, () =>
                {
                    EffectManager.Instance.Fade(0.4f, 0f);
                });
            }
        }

        private void OnDestroy()
        {
            if (_agentHealth != null)
            {
                _agentHealth.OnDead -= HandleDead;
                _agentHealth.OnHealthChange -= HandleChangeHealth;
            }
            enemyTurnEventSO.RemoveListener(HandleEnemyTurnEvent);
            animationEndTrigger.OnAnimationEnd -= HandleAnimationEnd;
            if (enemyAttackTrigger != null)
            {
                enemyAttackTrigger.OnAttack -= HandleAttackPlayer;
                enemyAttackTrigger.OnAttackEnd -= HandleAttackEnd;
            }
            if (UIManager.Instance != null && UIManager.Instance.LogCompo != null)
            {
                UIManager.Instance.LogCompo.OnFightIgnoreEvent -= OnFightIgnoreEvent;
            }

        }

        private void OnFightIgnoreEvent()
        {
            _enemyMaterial.DOFloat(0f, ShaderConst.Alpha, 0.5f);
            enemyAnimator.IgnoreFight();
        }

        private void HandleChangeHealth(float curHealth, float maxHealth, SkillType skillType)
        {
            if (_beforeHealth > curHealth)
            {
                enemyAnimator.ChangeState(EnemyAnimatorState.Hit);
                soundPlayer?.PlaySound("Enemy_Hit", SoundType.SFX);
                enemyParticle?.PlayParticle(EnemyParticleType.Hit);
                CameraManager.Instance.ImpulseModule.TriggerImpulse(0.2f);
            }
            _beforeHealth = curHealth;
            
        }

        private void HandleDead()
        {
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            if (isTutorialEnemy)
            {
                eventsChannelSO?.Invoke(EventChannelSystem.EventType.EnemyDead);
            }

            CombatManager.Instance.DeathAgent(AttackerType.Enemy);
            if (isTutorialEnemy) yield break;

            DataManager.Instance.CalculateCoinCount(enemySO.goldValue);

            DataManager.Instance.AddSkillCard(enemySO.skillCardData);
            yield return WaitForSecondsCache.Get(0.5f);
            WaveManager.Instance.WaveStart();
            UIManager.Instance.LogCompo.OpenPanel();
            if (MapManager.Instance != null)
            {
                MapManager.Instance.StartMove();
            }

            Destroy(gameObject);
        }

        public void OnSpawn()
        {
            transform.DOMoveX(0f, _enemyMoveDuration);
        }

        public float GetStatValue(EnemyStatType statType)
        {
            return enemyStatSO.GetStat(statType).GetValue();
        }
    }
}

