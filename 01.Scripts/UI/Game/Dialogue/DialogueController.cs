using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace JMT.UISystem.Dialogue
{
    public struct DialogueData
    {
        public string Name;
        public string Description;

        public DialogueData(string name, string desc)
        {
            Name = name;
            Description = desc;
        }
    }
    public class DialogueController : MonoBehaviour
    {
        public event Action OnEndEvent;

        [SerializeField] private DialogueView view;
        [SerializeField] private TouchScreen touchScreen;

        private DialogueModel model = new();

        private Queue<string> dialogueDatas = new();

        private void Awake()
        {
            touchScreen.OnClickEvent += HandleClickEvent;
        }

        private void OnDestroy()
        {
            touchScreen.OnClickEvent -= HandleClickEvent;
        }

        public async Task StartDialogue(string range)
        {
            string data = await model.LoadDataAsync(range);
            dialogueDatas = model.SettingDialogueData(data);
            SetDialogue();
        }

        private void HandleClickEvent()
        {
            if (!view.IsEnd)
            {
                view.SkipDescription();
            }
            else
            {
                SetDialogue();
            }
        }

        private void SetDialogue()
        {
            if (dialogueDatas.Count > 0)
            {
                view.OpenPanel();
                view.SetDialogue(dialogueDatas.Dequeue());
            }
            else
            {
                view.ClosePanel();
                OnEndEvent?.Invoke();
            }
        }
    }
}
