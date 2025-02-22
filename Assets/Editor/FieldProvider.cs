using Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldProvider : GameObjectFieldItemNameProvider
{
    public FieldProvider()
    {
        // Debug.LogError(typeof(Button).FullName);
    }
    public override void GetShowColor(string type, ShowColor color)
    {
        if (type == typeof(Button).FullName)
        {
            color.Color = new Color32(255, 0, 0, 128);
            color.Enabled = true;
            return;
        }
        base.GetShowColor(type, color);
    }
}