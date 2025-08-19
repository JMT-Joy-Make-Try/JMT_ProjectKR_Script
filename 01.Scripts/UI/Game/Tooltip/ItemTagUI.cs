using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTagUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Image content;
    [SerializeField] private TextMeshProUGUI tagText;

    public void SetTag(bool isOpen, string tag = "", Color color = default)
    {
        group.alpha = isOpen ? 1 : 0;
        tagText.text = tag;

        if(color != default)
            tagText.color = color;
    }
}
