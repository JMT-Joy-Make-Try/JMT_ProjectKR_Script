using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JMT.System.DeckSettingSystem.Filter
{
    public class FilterItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _outlineImage;
        protected bool _isSelected = false;

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            _isSelected = !_isSelected;
            _outlineImage.color = _isSelected ? Color.white : Color.gray;
        }
        
        public void ResetItem()
        {
            _isSelected = false;
            _outlineImage.color = Color.gray;
        }
    }
}