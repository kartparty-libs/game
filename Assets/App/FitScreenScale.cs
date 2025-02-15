using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RectTransform))]
public class FitScreenScale : MonoBehaviour
{
    // Start is called before the first frame update
    private float _width;
    private float _height;
    public RectTransform Window;
    private RectTransform _rectTransform;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _width = _rectTransform.sizeDelta.x;
        _height = _rectTransform.sizeDelta.y;
    }
    void Start()
    {
        check();
    }
    private void check()
    {
        if (Window == null)
        {
            return;
        }
        var w = Window.rect.width;
        var scale = w / _width;
        var h = _height * scale;
        _rectTransform.sizeDelta = new Vector2(w, h);
    }
}
