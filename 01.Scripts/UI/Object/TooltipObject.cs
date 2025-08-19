using JMT.UISystem;
using JMT.UISystem.Tooltip;
using UnityEngine;
public class TooltipObject : UIObject
{
    [SerializeField] private bool _isSkillTooltip;
    private ITooltipable tooltip;

    private void Awake()
    {
        tooltip = GetComponent<ITooltipable>();
        OnActiveEvent += HandleActiveEvent;
    }

    private void HandleActiveEvent(bool isActive)
    {
        // @TODO : ���߿� ������ ��������. ����� falseó��
        UIManager.Instance.TooltipCompo.ShowTooltip(isActive, _isSkillTooltip, tooltip);
    }
}
