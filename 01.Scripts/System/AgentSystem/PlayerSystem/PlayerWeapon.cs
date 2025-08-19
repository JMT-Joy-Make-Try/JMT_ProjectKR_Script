using JMT.Core;
using JMT.System.AgentSystem.PlayerSystem.Component;
using UnityEngine;

namespace JMT.System.AgentSystem.PlayerSystem
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private Animator _weaponAnimator;
        [SerializeField] private Renderer _weaponRenderer;
        [SerializeField] private AnimationEndTrigger _animationEndTrigger;

        private Material _weaponMaterial;

        void Awake()
        {
            _weaponMaterial = Instantiate(_weaponRenderer.material);
            _weaponRenderer.material = _weaponMaterial;
        }

        private void Start()
        {
            _animationEndTrigger.OnAnimationEnd += HandleAnimationEnd;
            _weaponAnimator.SetTrigger("Attack");
        }

        private void OnDestroy()
        {
            _animationEndTrigger.OnAnimationEnd -= HandleAnimationEnd;
        }

        private void HandleAnimationEnd()
        {
            Destroy(gameObject);
        }

        public void SetWeaponColor(Color color)
        {
            _weaponMaterial.SetColor(ShaderConst.Color, color);
            Debug.Log($"Weapon color set to: {color}");
        }
    }
}