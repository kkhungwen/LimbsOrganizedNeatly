using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InputHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action onPointerRightClick;
    public event Action onPointerLeftClick;
    public event Action onLeftBeginDrag;
    public event Action onRightBeginDrag;
    public event Action<Vector2> onLeftDrag;
    public event Action<Vector2> onRightDrag;
    public event Action onLeftEndDrag;
    public event Action onRightEndDrag;
    public event Action onPointerEnter;
    public event Action onPointerExit;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            onRightBeginDrag?.Invoke();

        if (eventData.button == PointerEventData.InputButton.Left)
            onLeftBeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            onRightDrag?.Invoke(HelperUtils.GetMouseWorldPosition());

        if (eventData.button == PointerEventData.InputButton.Left)
            onLeftDrag?.Invoke(HelperUtils.GetMouseWorldPosition());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            onRightEndDrag?.Invoke();

        if (eventData.button == PointerEventData.InputButton.Left)
            onLeftEndDrag?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            onPointerRightClick?.Invoke();

        if (eventData.button == PointerEventData.InputButton.Left)

            onPointerLeftClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke();
    }
}
