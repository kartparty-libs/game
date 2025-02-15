using System;
using Framework;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// 线性速度Buff
/// </summary>
public class VelocityBuff : BaseBuff
{
    public string From;
    public int Id;
    public float VelocityValue;
    public Vector3 VelocityDirection;
    private BuffContext _context;
    public VelocityBuff()
    {
        this.BuffType = EnumDefine.BuffType.VelocityBuff;
    }
    public override void Encode(JArray data)
    {
        base.Encode(data);
        data.Add((int)(this.Duration * 1000f));
        data.Add((int)(this.VelocityValue * 1000f));
        data.Add((int)(this.VelocityDirection.x * 1000f));
        data.Add((int)(this.VelocityDirection.y * 1000f));
        data.Add((int)(this.VelocityDirection.z * 1000f));

    }
    public override void Decode(JArray data)
    {
        base.Decode(data);
        this.Duration = Convert.ToInt32(data[0]) * 0.001f;
        VelocityValue = Convert.ToInt32(data[1]) * 0.001f;
        VelocityDirection = new Vector3(Convert.ToInt32(data[2]), Convert.ToInt32(data[3]), Convert.ToInt32(data[4])) * 0.001f;
    }
    public override void Setup(IBuffContext context)
    {
        base.Setup(context);
        if (context is BuffContext cxt)
        {
            this._context = cxt;
        }
        if (string.IsNullOrEmpty(From))
        {
            if (Id == 0)
            {
                Id = UnityEngine.Random.Range(100, 500);
            }
            From = Id.ToString();
        }

    }
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (_context == null) return;
        float VelocityValue = this.VelocityValue * (1f - _time / Duration);
        _context.Velocity += VelocityDirection * VelocityValue;
    }
    public override bool Replace(BaseBuff newData)
    {
        if (newData is VelocityBuff buff)
        {
            if (buff.From == this.From)
            {
                this._time = 0f;
                return true;
            }
        }
        return base.Replace(newData);
    }
}
