using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.UI;
public class FlyConfig
{
    private RectTransform container;
    private GameObject Tpl;
    private Queue<FlyMoney> Pool;
    private int Count;
    public FlyConfig(GameObject tpl, RectTransform container)
    {
        Pool = new Queue<FlyMoney>(20);
        Tpl = tpl;
        this.container = container;
        for (var i = 0; i < 20; i++)
        {
            var go = GameObject.Instantiate(Tpl, container);
            go.name = "fly" + Count++;
            go.SetActive(false);
            var fly = go.AddComponent<FlyMoney>();
            fly.Cfg = this;
            Pool.Enqueue(fly);
        }
    }
    public FlyMoney Get()
    {
        FlyMoney ins = null;
        if (Pool.Count > 0)
        {
            ins = Pool.Dequeue();
        }
        else
        {
            var go = GameObject.Instantiate(Tpl, container);
            go.SetActive(false);
            ins = go.AddComponent<FlyMoney>();
            go.name = "fly" + Count++;
            ins.Cfg = this;
        }
        return ins;
    }
    public void Hide(FlyMoney value)
    {
        Pool.Enqueue(value);
        value.gameObject.SetActive(false);
    }
}
public class FlyMoney : MonoBehaviour
{
    public FlyConfig Cfg;
    public string UserData;
    public float LocalScale = 1f;
    public float Alpha = 1f;
    public RectTransform Self;
    public Vector3 TargetAnchoredPosition;
    public Vector3 TargetPosition;
    public Vector3 Control;
    private bool _playing;
    private Action<FlyMoney> _callback;
    private float _time;
    private float _progress;
    private Vector3 From;
    public Image Image;
    private bool _invoke;
    private void Awake()
    {
        Self = this.GetComponent<RectTransform>();
        Image = this.GetComponent<Image>();
    }
    public void SetTarget(Vector3 from, Vector3 target, float time, Action<FlyMoney> callback = null)
    {
        From = from;
        Self.anchoredPosition = from;
        TargetAnchoredPosition = target;
        _callback = callback;
        _time = time;
        _invoke = false;
        Control = From + new Vector3(From.x > 0f ? 500f : -500f, UnityEngine.Random.Range(100, 250f), 0f);
    }
    public void Run()
    {
        _playing = true;
        _progress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playing)
        {
            _progress += Time.deltaTime;
            var p = _progress / _time;
            var bpos = Bezier.BezierCurve(From, Control, TargetAnchoredPosition, p);
            // var pos = Vector3.Lerp(Self.anchoredPosition, Target, p);
            var scale = Vector3.Lerp(Self.transform.localScale, Vector3.one * LocalScale, p);
            var color = Color.Lerp(Image.color, new Color(1, 1, 1, Alpha), p);
            Self.anchoredPosition = bpos;
            Self.localScale = scale;
            Image.color = color;
            if (_progress >= _time)
            {
                LocalScale = 1f;
                this.Cfg.Hide(this);
                _playing = false;
                _invoke = true;
                _callback.Invoke(this);
            }
            // if (!_invoke && p > 0.8f)
            // {
            //     _invoke = true;
            //     _callback.Invoke(this);
            // }
        }
    }
}
