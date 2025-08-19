using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JMT.Core.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.UISystem.Dialogue
{
    public class DialogueView : FadeUI
    {
        [SerializeField] private TextMeshProUGUI descText;

        private StringBuilder builder = new();
        private Coroutine dialogueRoutine;

        private string desc;

        private bool isEnd;
        public bool IsEnd => isEnd;

        public void SetDialogue(string data)
        {
            descText.text = "";
            dialogueRoutine = StartCoroutine(DialogueRoutine(data));
        }

        private IEnumerator DialogueRoutine(string desc)
        {
            var waitTime = WaitForSecondsCache.Get(0.04f);
            isEnd = false;
            builder.Clear();
            this.desc = desc;

            Stack<string> tagStack = new();
            StringBuilder visibleText = new();

            for (int i = 0; i < desc.Length; i++)
            {
                char c = desc[i];
                if (c == '<')
                {
                    int tagEnd = desc.IndexOf('>', i);
                    if (tagEnd == -1) break;

                    string fullTag = desc.Substring(i, tagEnd - i + 1);
                    bool isClosing = fullTag.StartsWith("</");

                    if (!isClosing)
                    {
                        tagStack.Push(fullTag);
                    }
                    else
                    {
                        if (tagStack.Count > 0) tagStack.Pop();
                    }

                    visibleText.Append(fullTag);
                    i = tagEnd;
                    continue;
                }

                visibleText.Append(c);

                builder.Clear();
                builder.Append(visibleText.ToString());

                foreach (var tag in tagStack)
                {
                    string closing = "</" + tag.Substring(1); // <b> -> </b>
                    builder.Append(closing);
                }

                descText.text = builder.ToString();
                yield return waitTime;
            }

            dialogueRoutine = null;
            isEnd = true;
        }

        public void SkipDescription()
        {
            isEnd = true;
            if(dialogueRoutine != null)
            {
                StopCoroutine(dialogueRoutine);
                dialogueRoutine = null;
            }
            descText.text = desc;
        }
    }
}
