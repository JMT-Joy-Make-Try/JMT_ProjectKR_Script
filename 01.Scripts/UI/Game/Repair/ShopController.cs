using JMT.System.CombatSystem;
using JMT.System.DataSystem;
using JMT.WaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JMT.UISystem.Shop
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private ShopView view;

        private void Awake()
        {
            view.OnExitEvent += ClosePanel;
            view.OnBuyEvent += HandleBuyEvent;
        }

        private void OnDestroy()
        {
            view.OnExitEvent -= ClosePanel;
            view.OnBuyEvent -= HandleBuyEvent;
        }

        public void OpenPanel()
        {
            view.OpenPanel();
            List<PassiveSO> passives = new();
            passives.Add(DataManager.Instance.RandomPassive());
            passives.Add(DataManager.Instance.RandomPassive());
            passives.Add(DataManager.Instance.RandomPassive());
            Debug.Log(passives.Count);
            view.ChangeAllItem(passives);
        }

        public void ClosePanel()
        {
            view.ClosePanel();
            UIManager.Instance.LogCompo.OpenPanel();
            WaveManager.Instance.WaveStart();
            DataManager.Instance.AddPassiveToOwnList(); 
        }
        private void HandleBuyEvent(PassiveSO passive)
        {
            if(DataManager.Instance.CoinCount >= passive.passivePrice)
            {
                DataManager.Instance.CalculateCoinCount(passive.passivePrice, false);
                DataManager.Instance.AddPassive(passive);
                view.ChangeItem(DataManager.Instance.RandomPassive());
            }
        }
    }
}
