using UnityEngine;
using UnityEngine.UI;

public class OutlineObject : UIObject
{
    [SerializeField] private Image outline;
    private void Awake()
    {
        OnActiveEvent += HandleActiveEvent;
    }

    private void HandleActiveEvent(bool isActive)
    {
        outline.enabled = isActive;
    }
}
