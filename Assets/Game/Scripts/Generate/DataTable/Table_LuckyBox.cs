
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class LuckyBox_Table : DataTableLoader<LuckyBox_Data>
{
    public override string GetTableName() { return "LuckyBox"; }
    protected override LuckyBox_Data CreateItem()
    {
        return new LuckyBox_Data();
    }
    public LuckyBox_Table()
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
    public LuckyBox_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class LuckyBox_Data : IDataTable
{
	public int Id { get; protected set; }
	public int ItemId { get; protected set; }
	public int ShowType { get; protected set; }
	public string Skin { get; protected set; }
	public int Quality { get; protected set; }
	public int Name { get; protected set; }
	public int Icon { get; protected set; }
	public int LuckyRate { get; protected set; }
	public string IconPath { get; protected set; }
	public string IconPathS { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		ItemId = Utils.Parse.ParseInt(values[1]);
		ShowType = Utils.Parse.ParseInt(values[2]);
		Skin = values[3];
		Quality = Utils.Parse.ParseInt(values[4]);
		Name = Utils.Parse.ParseInt(values[5]);
		Icon = Utils.Parse.ParseInt(values[6]);
		LuckyRate = Utils.Parse.ParseInt(values[8]);
		IconPath = values[10];
		IconPathS = values[11];
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(ItemId);
		buffer.Write(ShowType);
		buffer.Write(Skin);
		buffer.Write(Quality);
		buffer.Write(Name);
		buffer.Write(Icon);
		buffer.Write(LuckyRate);
		buffer.Write(IconPath);
		buffer.Write(IconPathS);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		ItemId = buffer.ReadInt();
		ShowType = buffer.ReadInt();
		Skin = buffer.ReadString();
		Quality = buffer.ReadInt();
		Name = buffer.ReadInt();
		Icon = buffer.ReadInt();
		LuckyRate = buffer.ReadInt();
		IconPath = buffer.ReadString();
		IconPathS = buffer.ReadString();
    }
}
