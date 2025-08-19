using JMT.System.CombatSystem;
using JMT.System.DataSystem;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.UISystem.Shop
{
    public class ShopView : SideUI
    {
        public event Action<PassiveSO> OnBuyEvent;
        public event Action OnExitEvent;

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI currentCoinText;
        [SerializeField] private Button exitButton;
        [SerializeField] private List<ItemObject> passives = new();

        private Dictionary<ItemObject, PassiveSO> passiveDict = new();
        private KeyValuePair<ItemObject, PassiveSO> currentDict;
        private List<Action> handler = new();

        private void Awake()
        {
            handler = new List<Action>(new Action[passives.Count]);
            exitButton.onClick.AddListener(HandleExitButton);
            for(int i = 0; i < passives.Count; i++)
            {
                int value = i;
                Debug.Log($"Handler for item {value} set.");
                handler[value] = () => HandleItemClickEvent(passives[value]);
                passives[value].OnClickEvent += handler[value];
            }
        }

        private void OnDestroy()
        {
            exitButton.onClick.RemoveListener(HandleExitButton);
            for (int i = 0; i < passives.Count; i++)
            {
                int value = i;
                passives[value].OnClickEvent -= handler[value];
            }
        }

        private void HandleExitButton()
        {
            OnExitEvent?.Invoke();
        }

        public void HandleItemClickEvent(ItemObject item)
        {
            passiveDict.TryGetValue(item, out PassiveSO passive);
            OnBuyEvent?.Invoke(passive);
            currentDict = new(item, passive);
        }

        public void ChangeItem(PassiveSO passiveSO)
        {
            currentCoinText.text = $"{DataManager.Instance.CoinCount}냥";
            passiveDict[currentDict.Key] = passiveSO;
            currentDict.Key.SetItemUI(passiveSO.passiveIcon, passiveSO.passiveName);
        }

        public void ChangeAllItem(List<PassiveSO> passives)
        {
            passiveDict.Clear();
            Debug.Log($"ChangeAllItem: {passives.Count} items to display.");
            currentCoinText.text = $"{DataManager.Instance.CoinCount}냥";
            for (int i = 0; i < passives.Count; i++)
            {
                this.passives[i].SetItemUI(passives[i].passiveIcon, passives[i].passiveName, $"{passives[i].passivePrice}냥");
                passiveDict.Add(this.passives[i], passives[i]);
            }
        }
    }
}
