using System;
using System.Collections.Generic;
using UnityEngine;

namespace JMT.System.AgentSystem.PlayerSystem.Component
{
    public class PlayerAnimator : MonoBehaviour, IPlayerComponent
    {
        [SerializeField] private Animator animator;

        private PlayerAnimatorState _currentState = PlayerAnimatorState.Idle;
        private Dictionary<PlayerAnimatorState, int> _stateHash = new Dictionary<PlayerAnimatorState, int>();

        public Player Player { get; private set; }

        public void Init(Player player)
        {
            Player = player;

            _stateHash.Clear();
            foreach (PlayerAnimatorState state in Enum.GetValues(typeof(PlayerAnimatorState)))
            {
                _stateHash[state] = Animator.StringToHash(state.ToString());
            }

            _currentState = PlayerAnimatorState.Run;
            SetBool(_stateHash[_currentState], true);
        }

        private void SetBool(string boolName, bool value) => animator?.SetBool(boolName, value);
        private void SetBool(int boolHash, bool value) => animator?.SetBool(boolHash, value);

        public void ChangeState(PlayerAnimatorState newState)
        {
            if (_currentState == newState) return;
            if (!_stateHash.ContainsKey(newState))
            {
                _stateHash[newState] = Animator.StringToHash(newState.ToString());
            }
            if (!_stateHash.ContainsKey(_currentState))
            {
                _stateHash[_currentState] = Animator.StringToHash(_currentState.ToString());
            }
            SetBool(_stateHash[_currentState], false);
            _currentState = newState;
            SetBool(_stateHash[_currentState], true);
        }

        public void ChangeStateImmediately(PlayerAnimatorState newState)
        {
            if (_currentState == newState) return;

            SetBool(_stateHash[_currentState], false);
            _currentState = newState;
            animator.Play(_stateHash[_currentState], 0, 0f);
        }
    }

    public enum PlayerAnimatorState
    {
        Idle,
        Run,
        Hit,
        CardAttack,
        Death
    }
}