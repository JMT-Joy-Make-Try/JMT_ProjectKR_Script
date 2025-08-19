using JetBrains.Annotations;
using JMT.Core;
using JMT.UISystem;
using JMT.UISystem.Dialogue;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JMT.DialogueSystem
{

    public class DialogueManager : MonoSingleton<DialogueManager>
    {
        [SerializeField] private DialogueController dialogueCompo;
        [SerializeField] private List<string> ranges;

        private int currentIndex = 0;

        protected override void Awake()
        {
            base.Awake();
            dialogueCompo.OnEndEvent += HandleEndEvent;
            StartDialogue();
        }

        public void StartDialogue()
        {
            StartDialogue(ranges[currentIndex]);
        }

        public async void StartDialogue(string range)
        {
            if (range == "" || range == null) return;
            await dialogueCompo.StartDialogue(range);
        }

        private void HandleEndEvent()
        {
            currentIndex++;
            if(currentIndex >= ranges.Count)
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}
