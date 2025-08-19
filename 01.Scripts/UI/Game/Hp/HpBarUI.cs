using DG.Tweening;
using JMT.System.AgentSystem;
using JMT.System.SkillSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JMT.UISystem.HpBar
{
    public class HpBarUI : MonoBehaviour
    {
        [SerializeField] private AgentHealth health;
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI hpText;

        private void Awake()
        {
            health.OnHealthChange += HandleHealthChange;
        }

        private void HandleHealthChange(float current, float max, SkillType skillType)
        {
            hpText.text = $"{Mathf.Ceil(current)}/{Mathf.Ceil(max)}";
            fillImage.DOFillAmount(current / max, 0.3f);
        }
    }
}
