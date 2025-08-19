using JMT.System.SkillSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.UISystem.Tooltip
{
    public class StatObject : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI statTypeText;
        [SerializeField] private TextMeshProUGUI statValueText;

        public void SetStat(SkillStatData statData)
        {
            icon.sprite = statData.Icon;
            statTypeText.text = statData.StatTypeToString;
            statValueText.text = statData.GetValue().ToString();
        }
    }
}
