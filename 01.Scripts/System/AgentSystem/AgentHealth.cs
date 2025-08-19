using System;
using JMT.Core.Tool;
using JMT.System.AgentSystem.Interface;
using JMT.System.SkillSystem;
using UnityEngine;

namespace JMT.System.AgentSystem
{
    public class AgentHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private AgentDamageVFX agentDamageVFX;
        private float _maxHealth = 100f;
        private float _currentHealth;
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public event Action<float, float, SkillType> OnHealthChange;
        public event Action OnTakeDamage;
        public event Action OnDead;
        public bool IsDead { get; private set; }

        

        public void Init(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            IsDead = false;
            OnHealthChange?.Invoke(_currentHealth, _maxHealth, SkillType.None);
        }
        



        public void TakeDamage(DamageResult damageResult)
        {
            
            // 최종 피해량 = 공격력 * 크리티컬 데미지 * (1 - 방어력 / (방어력 + K))
            Vector3 position = (Vector2)transform.position - new Vector2(0f, 0.5f) + UnityEngine.Random.insideUnitCircle * 0.3f;
            position.z = -3f;
            position.y += 4f;
            var damageVFX = Instantiate(agentDamageVFX, position, Quaternion.identity);

            if (damageResult.damageAmount <= 0)
            {
                damageVFX?.SetColor(Color.green);
                damageVFX?.ShowDamage((damageResult.damageAmount * -1f).ToString(), position);
                _currentHealth -= damageResult.damageAmount;
                _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
                OnHealthChange?.Invoke(_currentHealth, _maxHealth, damageResult.skillType);
                OnTakeDamage?.Invoke();
                return;
            }
            // 회피
            float evasionChance = UnityEngine.Random.Range(0f, 100f);
            if (evasionChance < damageResult.evasion)
            {
                Debug.Log("Attack evaded!");

                damageVFX?.SetColor(Color.blue);
                damageVFX?.ShowDamage("회피", position);
                OnTakeDamage?.Invoke();
                return;
            }
            var damage = damageResult.damageAmount;
            Debug.Log($"Damage dealt: {damage}");

            // 크리티컬
            float criticalChance = UnityEngine.Random.Range(0f, 100f);
            bool isCritical = criticalChance < damageResult.criticalChance;
            var criticalDamage = damageResult.criticalDamage;
            if (damageResult.skillType == SkillType.PhysicalAttack)
            {
                if (isCritical)
                {
                    damageVFX?.SetColor(Color.red);
                }
                else
                {
                    damageVFX?.SetGradient("#93a0ae".ToColor(), "#D6662F".ToColor());
                    criticalDamage = 1f; // 크리티컬이 아닐 경우 기본값
                }
                damage *= criticalDamage * (1 - damageResult.physicalDefense / (damageResult.physicalDefense + 100f));
            }
            else if (damageResult.skillType == SkillType.MagicAttack)
            {
                if (isCritical)
                {
                    damageVFX?.SetColor(Color.red);
                }
                else
                {
                    damageVFX?.SetGradient("#ccd1ff".ToColor(), "#3c45b7".ToColor());
                    criticalDamage = 1f; // 크리티컬이 아닐 경우 기본값
                }
                damage *= criticalDamage * (1 - damageResult.magicDefense / (damageResult.magicDefense + 100f));
            }

            Debug.Log($"Damage taken: {damage} (Critical: {isCritical}, Evasion: {damageResult.evasion}, Physical Defense: {damageResult.physicalDefense}, Magic Defense: {damageResult.magicDefense}), Skill Type: {damageResult.skillType}, Critical Damage: {criticalDamage}");

            damage = Mathf.FloorToInt(damage);

            // 체력 감소
            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            OnHealthChange?.Invoke(_currentHealth, _maxHealth, damageResult.skillType);
            OnTakeDamage?.Invoke();

            damageVFX?.ShowDamage(damage.ToString(), position);




            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                OnDead?.Invoke();
                IsDead = true;
                Dead();
            }
        }

        public void Dead()
        {
            Debug.Log("Dead");
        }
    }
}