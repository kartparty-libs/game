using UnityEngine;

/// <summary>
/// 加速Buff
/// </summary>
public class SpeedUpBuff : BaseBuff
{
    public int Id;
    public float Time;
    public float Timer;
    public float SpeedUpValue;
    public SpeedUpBuff()
    {
        this.BuffType = EnumDefine.BuffType.SpeedUpBuff;
    }
}
