using System;
using System.Collections.Generic;
using JMT.UISystem.Tooltip;
using UnityEngine;

namespace JMT.System.CombatSystem
{
    public class ItemUI : MonoBehaviour, ITooltipable
    {
        [SerializeField] private ItemSO _item;
        public string Name { get; private set; }
        public List<ItemTag> Tags { get; private set; }
        public string Desc { get; private set; }

        private void Awake()
        {
            if (_item == null)
            {
                Debug.LogError("ItemSO is not assigned in ItemUI.");
                return;
            }

            Name = _item.Name;
            Tags = _item.Tags;
            Desc = _item.Description;
        }
    }
}