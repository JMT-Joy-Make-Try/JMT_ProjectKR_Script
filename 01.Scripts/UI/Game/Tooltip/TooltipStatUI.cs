using JMT.System.SkillSystem;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.UISystem.Tooltip
{
    // public class StatData
    // {
    //     public StatSO Stat;
    //     public float StatValue;
    // }

    public class TooltipStatUI : MonoBehaviour
    {
        [SerializeField] private Image statPanel;
        [SerializeField] private StatObject firstStat;
        [SerializeField] private StatObject secondStat;

        public void SetStatTooltip(Color panelColor, SkillStatData firstStat, SkillStatData secondStat = null)
        {
            statPanel.color = panelColor;
            this.firstStat.SetStat(firstStat);

            if(secondStat != null)
                this.secondStat.SetStat(secondStat);
        }
    }
}
