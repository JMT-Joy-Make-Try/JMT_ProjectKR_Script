using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action OnClickEvent;
    public event Action<bool> OnActiveEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnActiveEvent?.Invoke(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnActiveEvent?.Invoke(false);
    }
}
