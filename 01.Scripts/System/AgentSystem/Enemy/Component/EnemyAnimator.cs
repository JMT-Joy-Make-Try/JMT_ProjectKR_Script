using System;
using System.Collections.Generic;
using DG.Tweening;
using JMT.System.CombatSystem;
using UnityEngine;

namespace JMT.System.AgentSystem.Enemy.Component
{
    public class EnemyAnimator<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] private Animator _animator;

        [Header("Ignore Fight")]
        [SerializeField] private float _ignoreXPosition = -20f;
        [SerializeField] private float _ignoreYPosition = 0f;
        [SerializeField] private bool _isMapFirstMove = true;
        [SerializeField] private bool _isXPosition = true;

        private T _curState;
        private Dictionary<T, int> _stateHashes = new Dictionary<T, int>();

        public virtual void Init(T initialState)
        {
            if (_stateHashes.Count == 0)
            {
                foreach (T state in Enum.GetValues(typeof(T)))
                {
                    _stateHashes[state] = Animator.StringToHash(state.ToString());
                }
            }

            _curState = initialState;
            _animator.SetBool(_stateHashes[_curState], true);
        }

        public void ChangeState(T newState)
        {
            _animator.SetBool(_stateHashes[_curState], false);
            _curState = newState;
            _animator.SetBool(_stateHashes[_curState], true);
        }

        public void IgnoreFight()
        {
            if (_isMapFirstMove)
            {
                MapManager.Instance.StartMove();
                _isMapFirstMove = false;
            }

            if (_isXPosition)
            {
                transform.DOMoveX(_ignoreXPosition, 1f).OnComplete(DestroyGameObject);
            }
            else
            {
                transform.DOMoveY(_ignoreYPosition, 1f).OnComplete(DestroyGameObject);
            }
        }

        private void DestroyGameObject()
        {
            Destroy(gameObject);
        }
    }
}