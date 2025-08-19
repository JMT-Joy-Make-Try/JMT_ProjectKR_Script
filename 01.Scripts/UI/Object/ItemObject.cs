using System.Collections.Generic;
using JMT.UISystem.Tooltip;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : UIObject, ITooltipable
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;

    public string Name { get; private set; }

    public List<ItemTag> Tags { get; private set; }

    public string Desc { get; private set; }

    public void SetItemUI(Sprite icon, string name = null)
    {
        itemIcon.sprite = icon;
        if(itemNameText != null && name != null)
            itemNameText.text = name;
    }

    public void SetItemUI(Sprite icon, string name, string coin)
    {
        itemIcon.sprite = icon;
        if (itemNameText != null && name != null)
            itemNameText.text = name;

        if (priceText != null)
            priceText.text = coin;
    }
    
    public void SetTooltip(string name, List<ItemTag> tags, string desc)
    {
        Name = name;
        Tags = tags;
        Desc = desc;
    }
}
