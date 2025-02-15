using UnityEngine;

public class ForceBuff : BaseBuff
{
    public int Id;
    public float Time;
    public ForceMode Mode;
    public float ForceValue;
    public Vector3 Force;
    public ForceBuff()
    {
        this.BuffType = EnumDefine.BuffType.ForceBuff;
    }
}
