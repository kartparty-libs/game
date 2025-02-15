using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VariableJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private JoystickType joystickType = JoystickType.Fixed;
    public Action OnStart;
    public Action<Vector2> OnHold;
    public Action OnEnd;
    private Vector2 fixedPosition = Vector2.zero;

    public void SetMode(JoystickType joystickType)
    {
        this.joystickType = joystickType;
        if (joystickType == JoystickType.Fixed)
        {
            background.anchoredPosition = fixedPosition;
            background.gameObject.SetActive(true);
        }
        else
            background.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        fixedPosition = background.anchoredPosition;
        SetMode(joystickType);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (_down) return;
        _down = true;
        _touchId = eventData.pointerId;
        var px = eventData.position.x / (float)Screen.width;
        var py = eventData.position.y / (float)Screen.height;

        var rect = this.baseRect.rect;
        var cw = rect.width;
        var ch = rect.height;
        px = cw * px - cw * 0.5f;
        py = ch * py - ch * 0.5f;
        // background.anchoredPosition = new Vector2(px, py);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
        if (OnStart != null)
        {
            OnStart.Invoke();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (_down)
        {
            if (_touchId != eventData.pointerId) return;
        }
        _down = false;
        if (joystickType != JoystickType.Fixed)
            background.gameObject.SetActive(false);

        base.OnPointerUp(eventData);
        if (OnEnd != null)
        {
            OnEnd.Invoke();
        }
    }
    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            // background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
    private void Update()
    {
        if (_down)
        {
            if (OnHold != null)
            {
                OnHold.Invoke(Direction);
            }
        }
    }
}

public enum JoystickType { Fixed, Floating, Dynamic }