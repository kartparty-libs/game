
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class KartKey_Table : DataTableLoader<KartKey_Data>
{
    public override string GetTableName() { return "KartKey"; }
    protected override KartKey_Data CreateItem()
    {
        return new KartKey_Data();
    }
    public KartKey_Table()
    {
        _branchs = new List<string>() { "" };
    }
#if true
    private Dictionary<string, int> _keyToIndex;
    protected override bool hasIndexKey()
    {
        return true;
    }
    protected override void onPostBefore()
    {
        base.onPostBefore();
        if (_keyToIndex == null)
        {
            _keyToIndex = new Dictionary<string, int>(ItemCount);
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
    public KartKey_Data Get(string key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class KartKey_Data : IDataTable
{
	public string Id { get; private set; }
	public int Pledge { get; private set; }
	public int IsRobot { get; private set; }
	public int RobotType { get; private set; }
	public void Parse(string[] values)
	{
		Id = values[0];
		Pledge = Utils.Parse.ParseInt(values[1]);
		IsRobot = Utils.Parse.ParseInt(values[2]);
		RobotType = Utils.Parse.ParseInt(values[3]);
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(Pledge);
		buffer.Write(IsRobot);
		buffer.Write(RobotType);
    }
    public void Decode(ByteArray buffer)
    {
		Id = buffer.ReadString();
		Pledge = buffer.ReadInt();
		IsRobot = buffer.ReadInt();
		RobotType = buffer.ReadInt();
    }
}
