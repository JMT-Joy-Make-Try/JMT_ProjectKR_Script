using UnityEngine;

namespace JMT.System.TutorialSystem
{
    [CreateAssetMenu(fileName = "TutorialStep", menuName = "SO/Tutorial/Step")]
    public class TutorialStepSO : ScriptableObject
    {
        public string StepName;
        public string Description;
        public EventChannelSystem.EventType TriggerEvent;
    }
}
