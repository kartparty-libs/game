using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIConfig : MonoBehaviour, IUGUIManagerConfig
{
    public Canvas Canvas;
    public RectTransform InstanceRoot;
    public RectTransform Root;
    public RectTransform[] Layers;
    private bool _debugMode;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Debug"))
        {
            _debugMode = true;
        }
        else
        {
            _debugMode = false;
        }
    }
    public Canvas GetCanvas()
    {
        return Canvas;
    }

    public Vector2 GetDesignSize()
    {
        return new Vector2(2532, 1170);
    }
    public RectTransform GetInstanceRoot()
    {
        return InstanceRoot;
    }

    public Transform GetLayerContainer(int layer)
    {
        if (layer < Layers.Length)
        {
            return Layers[layer];
        }
        return Canvas.transform;

    }

    public RectTransform GetRootContainer()
    {
        return Root;
    }
    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.normal.textColor = Color.white; // 设置文本颜色为红色  
        myStyle.fontSize = 30; // 设置字体大小为20  
        GUI.color = Color.white;

        var safeArea = Screen.safeArea;
        var txt = BuildTime.Time;
        GUI.Label(new Rect(safeArea.x + 20, safeArea.y + safeArea.height - 60, 100, 50), txt, myStyle);

    }
}