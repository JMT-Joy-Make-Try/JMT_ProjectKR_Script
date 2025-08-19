using System.Collections.Generic;
using JMT.UISystem.Tooltip;
using UnityEngine;
using JMT;

namespace JMT.System.CombatSystem
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Combat/ItemSO")]
    public class ItemSO : ScriptableObject, IGeneralTooltipable
    {
        public string Name;
        [TextArea]
        public string Description;
        [field: SerializeField] public List<ItemTag> Tags { get; set; }

        string IGeneralTooltipable.Name => Name;

        public string Desc => Description;

        public Sprite Icon;
    }
}