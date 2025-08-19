using JMT.System.CombatSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JMT.UISystem.ItemPopup
{
    public class ItemGetController : MonoBehaviour
    {
        [SerializeField] private ItemGetView view;

        private void Awake()
        {
            view.OnExitEvent += HandleExitEvent;
        }

        private void OnDestroy()
        {
            view.OnExitEvent -= HandleExitEvent;
        }

        private void HandleExitEvent()
        {
            ClosePanel();
        }

        public void OpenPanel(ItemSO item)
        {
            List<ItemSO> list = new();
            list.Add(item);
            view.OpenPanel();
            view.SetItemPopup(list);
            //Time.timeScale = 0;
        }

        public void OpenPanel(List<ItemSO> items)
        {
            view.OpenPanel();
            view.SetItemPopup(items);
            //Time.timeScale = 0;
        }

        public void ClosePanel()
        {
            view.ClosePanel();
            //Time.timeScale = 1;
        }
    }

}