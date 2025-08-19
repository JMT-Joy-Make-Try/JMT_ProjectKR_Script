using JMT.System.CombatSystem;
using System.Collections.Generic;
using UnityEngine;

namespace JMT.UISystem.Result
{
    public class ResultCardView : MonoBehaviour
    {
        [SerializeField] private ItemObject resultItemPrefab;

        [SerializeField] private Transform skillContentTrm;
        [SerializeField] private Transform generalContentTrm;

        public void SetSkillContentItem(List<ItemSO> items)
        {
            for(int i = 0; i < items.Count; i++)
            {
                ItemObject item = Instantiate(resultItemPrefab, skillContentTrm);
                item.SetItemUI(items[i].Icon);
                item.SetTooltip(items[i].Name, items[i].Tags, items[i].Description);
            }
        }

        public void SetGeneralContentItem(List<ItemSO> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                ItemObject item = Instantiate(resultItemPrefab, generalContentTrm);
                item.SetItemUI(items[i].Icon);
                item.SetTooltip(items[i].Name, items[i].Tags, items[i].Description);
            }
        }
    }
}
