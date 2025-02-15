
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class LoadingUI : UIWindowBase
{
    public const string SetTotalTime = nameof(SetTotalTime);
    public const string SetLoadComplete = nameof(SetLoadComplete);
    private float _time;
    private float _totalTime;
    private bool _canHide;
    private bool _willHide;
    public override void SetParam(string name, object value)
    {
        base.SetParam(name, value);
        if (name == SetTotalTime)
        {
            if (value is float totalTime)
            {
                if (totalTime > 0f)
                {
                    _totalTime = (float)value;
                    _time = 0f;
                    _canHide = false;
                    _willHide = false;
                }
                if (totalTime < 0f)
                {
                    _canHide = true;
                }
            }
        }
        else if (name == SetLoadComplete)
        {
            _canHide = true;
        }
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        this.progress_Image.fillAmount = 0f;
        var rect = this.slider_Slider.GetComponent<RectTransform>();
        var canvas = rect.parent.GetComponent<RectTransform>();
        rect.offsetMin = new Vector2(0f, 0f);
        rect.offsetMax = new Vector2(1f, 0f);

        rect.sizeDelta = new Vector2(-GameEntry.GUI.ScreenInfo.CanvasSize.x % 38, 64);
        rect.anchoredPosition = new Vector2(0, 53);
    }
    protected override void OnClose()
    {
        base.OnClose();
        _canHide = false;
        GameEntry.GUIEvent.DispatchEvent(GUIEvent.OnLoadingHide);
        GameEntry.Context.LoadingShow = false;
    }
    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (_totalTime <= 0f)
        {
            return;
        }
        if (_willHide)
        {
            return;
        }
        if (_canHide)
        {
            _time += deltaTime * 6f;
        }
        else
        {
            _time += deltaTime * 1f;
            if (_time > _totalTime * 0.9f)
            {
                _time = _totalTime * 0.9f;
            }
        }
        this.slider_Slider.value = _time / _totalTime;
        int v = Mathf.FloorToInt(slider_Slider.value * 100);
        msg_Text.text = "LOADING " + v + "%";
        this.progress_Image.fillAmount = _time / _totalTime;
        if (_time > _totalTime)
        {
            _willHide = true;
            this.Close();
        }
    }
}