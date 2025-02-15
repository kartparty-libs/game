using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragBehaviour : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Vector2 Down;
    public Vector2 Position;
    public Vector2 MoveValue;
    public Vector2 LastValue;
    public Action<Vector2> OnDragAction;
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Down = eventData.position;
        LastValue = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Position = eventData.position;
        MoveValue = Position - LastValue;
        LastValue = Position;
        OnDragAction?.Invoke(MoveValue);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}