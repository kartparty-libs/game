using System;
using System.Collections.Generic;
using Framework;
using static EnumDefine;

public class TreasureChestSystem : BaseSystem<TreasureChestSystem>
{
    public List<TreasureChestData> Datas { get; set; } = new List<TreasureChestData>();
    public override void OnAwake()
    {
        base.OnAwake();
    }
    public override void OnStart()
    {
        base.OnStart();

    }
    public void Clear()
    {
        Datas.Clear();
    }
    public void AddTreasure(TreasureChestData info)
    {
        Datas.Add(info);
    }
}