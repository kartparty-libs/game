
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class Mining_Table : DataTableLoader<Mining_Data>
{
    public override string GetTableName() { return "Mining"; }
    protected override Mining_Data CreateItem()
    {
        return new Mining_Data();
    }
    public Mining_Table()
    {
        _branchs = new List<string>() { "" };
    }
#if true
    private Dictionary<int, int> _keyToIndex;
    protected override bool hasIndexKey()
    {
        return true;
    }
    protected override void onPostBefore()
    {
        base.onPostBefore();
        if (_keyToIndex == null)
        {
            _keyToIndex = new Dictionary<int, int>(ItemCount);
        }
        else
        {
            _keyToIndex.Clear();
        }
        if (DecodeInstant())
        {
            var len = DataList.Count;
            for (int i = 0; i < len; i++)
            {
                var item = DataList[i];
                if (_keyToIndex.ContainsKey(item.Id))
                {
                    Debug.LogErrorFormat("{0} 存在相同的索引 {1}", GetFileName(), item.Id);
                    continue;
                }
                _keyToIndex.Add(item.Id, i);
            }
        }
    }
    public Mining_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class Mining_Data : IDataTable
{
	public int Id { get; protected set; }
	public int Condition { get; protected set; }
	public int Cooldown { get; protected set; }
	public int ScoreCoefficient { get; protected set; }
	public int ScorePool { get; protected set; }
	public int TotalScorePool { get; protected set; }
	public int PreSettlementTime { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		Condition = Utils.Parse.ParseInt(values[1]);
		Cooldown = Utils.Parse.ParseInt(values[2]);
		ScoreCoefficient = Utils.Parse.ParseInt(values[3]);
		ScorePool = Utils.Parse.ParseInt(values[4]);
		TotalScorePool = Utils.Parse.ParseInt(values[5]);
		PreSettlementTime = Utils.Parse.ParseInt(values[6]);
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(Condition);
		buffer.Write(Cooldown);
		buffer.Write(ScoreCoefficient);
		buffer.Write(ScorePool);
		buffer.Write(TotalScorePool);
		buffer.Write(PreSettlementTime);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		Condition = buffer.ReadInt();
		Cooldown = buffer.ReadInt();
		ScoreCoefficient = buffer.ReadInt();
		ScorePool = buffer.ReadInt();
		TotalScorePool = buffer.ReadInt();
		PreSettlementTime = buffer.ReadInt();
    }
}
