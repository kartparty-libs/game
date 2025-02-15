using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class BuffNode
{
    public IBuffContext context { get; private set; }
    private List<BaseBuff> _buffs = new List<BaseBuff>();
    private int _frame;
    public BuffNode(IBuffContext context)
    {
        this.context = context;
    }
    public void Update(float deltaTime)
    {
        if (UnityEngine.Time.frameCount <= _frame)
        {
            return;
        }
        if (context != null)
        {
            context.Clear();
        }
        _frame = UnityEngine.Time.frameCount;
        var len = _buffs.Count;
        while (len-- > 0)
        {
            var buff = _buffs[len];
            buff.Update(deltaTime);
            if (buff.Finished)
            {
                _buffs.RemoveAt(len);
                continue;
            }
        }
    }
    public bool Add(BaseBuff value)
    {
        if (value == null)
        {
            return false;
        }
        value.Setup(context);
        var len = _buffs.Count;
        while (len-- > 0)
        {
            var buff = _buffs[len];
            if (buff.InstanceId == value.InstanceId)
            {
                return false;
            }
            if (buff.ClassType == value.ClassType)
            {
                if (buff.Replace(value))
                {
                    return false;
                }
            }
        }
        _buffs.Add(value);
        return true;
    }
    public void Remove(BaseBuff value)
    {
        var len = _buffs.Count;
        while (len-- > 0)
        {
            var buff = _buffs[len];
            if (buff == value)
            {
                _buffs.RemoveAt(len);
            }
        }
    }
    public void Clear()
    {
        _buffs.Clear();
    }
}

public class BaseBuff
{
    private static int _instanceId = 1;
    public int InstanceId
    {
        get
        {
            if (_id < 1)
            {
                _id = _instanceId++;
            }
            return _id;
        }
    }
    public int BuffType;
    public Type ClassType { get; private set; }
    public bool Finished { get; protected set; }
    public float Duration;
    protected float _time;
    private int _id;
    internal bool _added;
    public virtual void Setup(IBuffContext context)
    {
        _time = 0f;
        ClassType = this.GetType();
    }
    public virtual void Update(float deltaTime)
    {
        _time += deltaTime;
        Finished = _time >= Duration;
    }
    public virtual bool Replace(BaseBuff newData)
    {
        return false;
    }
    public virtual void Decode(JArray data)
    {

    }
    public virtual void Encode(JArray data)
    {

    }
}