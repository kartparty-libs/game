
using System.Collections.Generic;
using Framework;
using Framework.Core;
using UnityEngine;
public partial class EffectUI : UIWindowBase
{
    private Dictionary<int, FlyConfig> FlyMap = new Dictionary<int, FlyConfig>();
    protected override void OnAwake()
    {
        base.OnAwake();
        var len = collectMoney_RectTransform.childCount;
        for (var type = 0; type < len; type++)
        {
            var tpl = this.collectMoney_RectTransform.GetChild(type).gameObject;
            var cfg = new FlyConfig(tpl, collectMoney_RectTransform);
            FlyMap.Add(type, cfg);
            tpl.SetActive(false);
        }
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        GameEntry.GUIEvent.AddEventListener(GUIEvent.FlyEffecct, onFlyEffecct);
        GameEntry.GUIEvent.AddEventListener(GUIEvent.FlyEffecctOne, onFlyOneEffecct);
    }



    private void onFlyEffecct(IEventData eventData)
    {
        if (eventData is GUIEvent e)
        {
            var start = GameEntry.Context.FlyStart;
            var target = GameEntry.Context.FlyTarget;
            GameEntry.Context.FlyStart = null;
            GameEntry.Context.FlyTarget = null;
            if (target == null)
            {
                return;
            }
            if (!FlyMap.TryGetValue(e.IntValue, out var cfg))
            {
                return;
            }
            var startPos = new Vector3(Random.Range(-100, 100f), Random.Range(-100, 100f));
            if (start != null)
            {
                startPos = Utils.Unity.ScreenPointToUIPoint(this.Widget.RectTransform, start.position);
            }
            var targetpos = Utils.Unity.ScreenPointToUIPoint(this.Widget.RectTransform, target.position);
            var len = 10;
            while (len-- > 0)
            {
                var fly = cfg.Get();
                fly.gameObject.SetActive(true);
                fly.transform.localScale = Vector3.one * 0.6f;
                fly.LocalScale = 0.8f;
                fly.Alpha = 1f;
                fly.Image.color = new Color(1, 1, 1, 0f);
                fly.SetTarget(startPos, targetpos, Random.Range(0.5f, 0.9f), fly =>
                {
                    fly.LocalScale = 1f;

                });
                fly.Run();
            }
        }
    }
    private void onFlyOneEffecct(IEventData eventData)
    {
        if (eventData is GUIEvent e)
        {
            var start = e.Object1;
            var target = e.Object2;
            if (target == null)
            {
                return;
            }
            if (!FlyMap.TryGetValue(e.IntValue, out var cfg))
            {
                return;
            }
            var startPos = new Vector3(Random.Range(-100, 100f), Random.Range(-100, 100f));
            if (start != null)
            {
                startPos = Utils.Unity.ScreenPointToUIPoint(this.Widget.RectTransform, start.position);
            }
            var targetpos = Utils.Unity.ScreenPointToUIPoint(this.Widget.RectTransform, target.position);
            var fly = cfg.Get();
            fly.gameObject.SetActive(true);
            fly.transform.localScale = Vector3.one * 0.6f;
            fly.UserData = e.StringValue;
            fly.Alpha = 1f;
            fly.TargetPosition = target.position;
            var time = Random.Range(0.5f, 0.9f);
            if (e.FloatValue > 0.01f)
            {
                time = e.FloatValue;
            }
            time += Random.Range(0, e.RandomValue);
            if (time < 0.01f)
            {
                time = 0.01f;
            }
            fly.SetTarget(startPos, targetpos, time, fly =>
            {
                fly.LocalScale = 1f;
                var finish = ReferencePool.Get<GUIEvent>();
                finish.Vector3Value = fly.TargetPosition;
                finish.EventType = GUIEvent.FlyEffecctOneFinish;
                finish.StringValue = fly.UserData;
                GameEntry.GUIEvent.DispatchEvent(finish);
            });
            if (e.StringValue == "show")
            {
                fly.Control = startPos + Vector3.up * 200f + Vector3.right * Random.Range(-100f, 100f);
            }
            else if (e.StringValue == "get")
            {
                fly.Control = targetpos - Vector3.up * 200f + Vector3.right * Random.Range(-400f, 400f);
            }
            fly.Run();

        }
    }
}