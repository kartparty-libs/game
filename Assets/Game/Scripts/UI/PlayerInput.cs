using System;
using System.Collections.Generic;
using UnityEngine;
using Framework;
public class PlayerInput
{
    public const string Steer = "Steer";
    public const string Jump = "Jump";
    public bool Enabled { get; private set; }

    public GameInputDevice InputDevice { get; private set; }
    public PlayerInput()
    {
        InputDevice = new GameInputDevice();
        var list = new List<string>() { Steer,Jump };
        var inputControls = new GameInputControls();
        inputControls.Enable();
        var len = list.Count;
        for (var i = 0; i < list.Count; i++)
        {
            var name = list[i];
            var action = inputControls.FindAction(name, true);
            action.started += onActionStarted;
            action.performed += onActionPerformed;
            action.canceled += onActionCanceled;
        }
        InputDevice.GetAction("Accel").PerformEveryFrame = true;
    }
    public void Start(string name)
    {
        InputDevice.ActionStart(name);
    }
    public GameDeviceAction Perform(string name)
    {
        return InputDevice.ActionPerform(name);
    }
    public void Cancel(string name)
    {
        InputDevice.ActionCancel(name);
    }
    private void onActionStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        InputDevice.ActionStart(obj.action.name);
    }
    private void onActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        var action = InputDevice.ActionPerform(obj.action.name);
        if (obj.valueType == typeof(Vector2))
        {
            action.DataValue2 = obj.ReadValue<Vector2>();
        }
    }
    private void onActionCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        InputDevice.ActionCancel(obj.action.name);
    }
    public void Update(float deltaTime)
    {
        if (!Enabled) return;
        InputDevice.Update(deltaTime);
    }
    public void SetEnabled(bool value)
    {
        Enabled = value;
    }
    public GameDeviceAction Read()
    {
        if (!Enabled) return null;
        return InputDevice.Read();
    }
}