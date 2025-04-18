using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldableButton : Button
{
    private bool _isHolding;
    public event Action onHold;

    private void Update()
    {
        if (_isHolding) onHold?.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        _isHolding = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        _isHolding = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _isHolding = false;
    }
}