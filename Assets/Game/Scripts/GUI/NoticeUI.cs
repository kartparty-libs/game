
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class NoticeUI : UIWindowBase
{
    private Queue<string> normalMsg = new Queue<string>();
    private Queue<string> impatientMsg = new Queue<string>();
    private bool _playing;
    private float _posx = 0;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.size_RectTransform.gameObject.SetActive(false);
        // normalMsg.Enqueue("0111231231dddd");

    }
    public override void SetParam(string name, object value)
    {
        base.SetParam(name, value);
        if (value is string msg)
        {
            normalMsg.Enqueue(msg);
        }
    }
    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (!_playing)
        {
            if (impatientMsg.Count > 0)
            {
                play(impatientMsg.Dequeue());
            }
            else if (normalMsg.Count > 0)
            {
                play(normalMsg.Dequeue());
            }
            return;
        }
        _posx -= deltaTime * 100f;
        this.msg_TextMeshProUGUI.rectTransform.anchoredPosition = new Vector2(_posx, 0);
        if (_posx < -this.size_RectTransform.rect.width - this.msg_TextMeshProUGUI.rectTransform.rect.width)
        {
            _playing = false;
            this.size_RectTransform.gameObject.SetActive(false);

        }
    }
    private void play(string msg)
    {
        this.size_RectTransform.gameObject.SetActive(true);
        _posx = 10f;
        this.msg_TextMeshProUGUI.text = msg;
        this.msg_TextMeshProUGUI.rectTransform.anchoredPosition = new Vector2(10, 0);
        _playing = true;
    }
}