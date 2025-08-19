using JMT.System.AgentSystem.PlayerSystem;
using JMT.System.Card;
using JMT.System.CombatSystem;
using JMT.System.SkillSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.UISystem.Tooltip
{
    public class TooltipView : MonoBehaviour
    {
        public event Action<bool> OnTooltipActiveEvent;

        [SerializeField] private RectTransform tooltip;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private Transform itemTagsTrm;

        [SerializeField] private RectTransform tooltipContentTrm;
        [SerializeField] private TooltipStatUI tooltipStatUI;

        public Vector2 TooltipAnchoredPos => tooltip.anchoredPosition;
        public Vector2 TooltipSizeDelta => tooltip.sizeDelta;

        private List<ItemTagUI> tags = new();

        private void Awake()
        {
            tags = itemTagsTrm.GetComponentsInChildren<ItemTagUI>().ToList();
        }

        public void SetTooltipPivot(bool isRight, Vector2 pivotVec, Vector3 rectPos)
        {
            tooltip.pivot = pivotVec;
            tooltip.position = rectPos;
            SetStatTooltipTrm(isRight);
        }

        private void SetStatTooltipTrm(bool isRight)
        {
            if (!isRight)
            {
                tooltip.SetSiblingIndex(0);
            }
            else
            {
                tooltipContentTrm.SetSiblingIndex(0);
            }
        }

        // 2. 스탯은 없는데 태그는 있는거.
        public void ShowTooltip(bool isActive, bool isStat, ITooltipable tooltipData = null)
        {
            ActiveTooltip(isActive);
            tooltipContentTrm.gameObject.SetActive(isStat);

            if (isStat && isActive)
            {
                Debug.Log("ShowTooltip : isStat true");
                //@TODO: PlayerStat를 GetAllData나 뭐 그런걸로 한번에 갖고와야함.
                List<SkillStatData> testStat = (tooltipData as SkillCard).SkillData.GetAllStats();
                for(int i = 0; i < testStat.Count; i++)
                {
                    TooltipStatUI tooltipUI = Instantiate(tooltipStatUI, tooltipContentTrm);
                    tooltipUI.SetStatTooltip(Color.black, testStat[i]);
                }
            }

            if (tooltipData != null)
            {
                nameText.text = tooltipData.Name;
                descText.text = tooltipData.Desc;
            }

            for (int i = 0; i < tags.Count; i++)
            {
                if (i < tooltipData.Tags.Count)
                    tags[i].SetTag(true, ItemTagToString(tooltipData.Tags[i]));
                else
                    tags[i].SetTag(false);
            }
        }
        
        

        private void ActiveTooltip(bool isActive)
        {
            OnTooltipActiveEvent?.Invoke(isActive);
            tooltip.gameObject.SetActive(isActive);

            // @TODO: 오브젝트 생성 후에는 주석을 해제해야함.
            if (!isActive && tooltipContentTrm.childCount > 0)
            {
                foreach (Transform child in tooltipContentTrm)
                    Destroy(child.gameObject);
            }
        }

        public string ItemTagToString(ItemTag tag)
        {
            switch (tag)
            {
                case ItemTag.DodgeRate:
                    return "#회피율";
                case ItemTag.TotalAttack:
                    return "#전체공격력";
                case ItemTag.MagicAttack:
                    return "#마법공격력";
                case ItemTag.PhysicalAttack:
                    return "#물리공격력";
                case ItemTag.LastingEffect:
                    return "#효과지속시간";
                case ItemTag.Stemina:
                    return "#체력";
            }
            return "";
        }
    }
}