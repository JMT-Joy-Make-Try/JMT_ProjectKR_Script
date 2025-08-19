using JMT.System.CombatSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.UISystem.ItemPopup
{
    public class ItemGetView : FadeUI
    {
        public event Action OnExitEvent;

        [SerializeField] private ItemObject itemPrefab;
        [SerializeField] private Transform itemContentTrm;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            exitButton.onClick.AddListener(HandleExitButton);
        }

        private void HandleExitButton()
        {
            OnExitEvent?.Invoke();
        }

        public void SetItemPopup(List<ItemSO> items)
        {
            StartCoroutine(DelayDestroy(items));            
        }

        private IEnumerator DelayDestroy(List<ItemSO> items)
        {
            while (itemContentTrm.childCount > 0)
            {
                Destroy(itemContentTrm.GetChild(0).gameObject);
                yield return null;
            }
            yield return null;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null) continue;
                ItemObject itemObject = Instantiate(itemPrefab, itemContentTrm);
                itemObject.SetItemUI(items[i].Icon, items[i].Name);
                itemObject.SetTooltip(items[i].Name, items[i].Tags, items[i].Description);
                yield return null;
            }
        }
    }
}
