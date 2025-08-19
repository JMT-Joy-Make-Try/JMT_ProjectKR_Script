using JMT.EventSystem;
using JMT.UISystem.Content;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JMT.UISystem
{

    public class SelectContentUI : ContentUI
    {
        [SerializeField] private Image contentImage;
        [SerializeField] private Button selectButton;
        [SerializeField] private TextMeshProUGUI selectButtonText;

        [SerializeField] private Button extraSelectButton;
        [SerializeField] private TextMeshProUGUI extraSelectButtonText;

        [SerializeField] private GameObject selectImage;
        [SerializeField] private TextMeshProUGUI imageText;


        private void OnDestroy()
        {
            selectButton.onClick.RemoveAllListeners();
            extraSelectButton.onClick.RemoveAllListeners();
        }

        public void SetContentUI(Color currentEventColor, List<ButtonEvent> buttonEvents)
        {
            contentImage.color = currentEventColor;
            selectButtonText.text = buttonEvents[0].ButtonText;
            selectButton.onClick.AddListener(buttonEvents[0].Action);
            selectButton.onClick.AddListener(() => SetSelectUI(buttonEvents[0].SelectText));

            extraSelectButton.gameObject.SetActive(buttonEvents.Count > 1);

            if (buttonEvents.Count > 1)
            {
                extraSelectButtonText.text = buttonEvents[1].ButtonText;
                extraSelectButton.onClick.AddListener(buttonEvents[1].Action);
                extraSelectButton.onClick.AddListener(() => SetSelectUI(buttonEvents[1].SelectText));
            }
        }

        public void SetSelectUI(string select)
        {
            selectImage.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
            extraSelectButton.gameObject.SetActive(false);

            imageText.text = select;
        }
    }
}
