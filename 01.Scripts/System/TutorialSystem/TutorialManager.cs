using System.Collections.Generic;
using UnityEngine;
using JMT.System.EventChannelSystem;
using JMT.DialogueSystem;

namespace JMT.System.TutorialSystem
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private List<TutorialStepSO> steps;
        [SerializeField] private EventsChannelSO eventChannel;

        private int _currentStepIndex = 0;

        private void OnEnable()
        {
            RegisterCurrentStepListener();
        }

        private void OnDisable()
        {
            UnregisterCurrentStepListener();
        }

        private void RegisterCurrentStepListener()
        {
            if (_currentStepIndex >= steps.Count) return;
            var currentStep = steps[_currentStepIndex];
            eventChannel.AddListener(currentStep.TriggerEvent, OnStepCompleted);
        }

        private void UnregisterCurrentStepListener()
        {
            if (_currentStepIndex >= steps.Count) return;
            var currentStep = steps[_currentStepIndex];
            eventChannel.RemoveListener(currentStep.TriggerEvent, OnStepCompleted);
        }

        private void OnStepCompleted()
        {
            Debug.Log($"튜토리얼 단계 완료: {steps[_currentStepIndex].StepName}");
            if (_currentStepIndex >= 1)
                DialogueManager.Instance.StartDialogue();

            UnregisterCurrentStepListener();
            _currentStepIndex++;

            if (_currentStepIndex < steps.Count)
            {
                RegisterCurrentStepListener();
                Debug.Log($"다음 튜토리얼 단계 시작: {steps[_currentStepIndex].StepName}");
            }
            else
            {
                Debug.Log("튜토리얼 완료!");
            }
        }
    }
}
