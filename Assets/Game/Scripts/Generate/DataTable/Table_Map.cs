
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class Map_Table : DataTableLoader<Map_Data>
{
    public override string GetTableName() { return "Map"; }
    protected override Map_Data CreateItem()
    {
        return new Map_Data();
    }
    public Map_Table()
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
    public Map_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class Map_Data : IDataTable
{
	public int Id { get; protected set; }
	public string Name { get; protected set; }
	public int Loop { get; protected set; }
	public string Scene { get; protected set; }
	public string Icon { get; protected set; }
	public int LineMaxPlayerNum { get; protected set; }
	public int SceneType { get; protected set; }
	public int Time { get; protected set; }
	public int EndTime { get; protected set; }
	public int CarLevelLimit { get; protected set; }
	public CfgItemData[] AwardItem { get; protected set; }
	public float AISpeedMin { get; protected set; }
	public float AISpeedMax { get; protected set; }
	public int CostEnergy { get; protected set; }
	public CfgItemData[] CostItems { get; protected set; }
	public int IsOpen { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		Name = values[1];
		Loop = Utils.Parse.ParseInt(values[2]);
		Scene = values[3];
		Icon = values[4];
		LineMaxPlayerNum = Utils.Parse.ParseInt(values[5]);
		SceneType = Utils.Parse.ParseInt(values[6]);
		Time = Utils.Parse.ParseInt(values[7]);
		EndTime = Utils.Parse.ParseInt(values[8]);
		CarLevelLimit = Utils.Parse.ParseInt(values[10]);
		if (Utils.Parse.ParseList(values[11]) is string[]  s_AwardItem) { var f_AwardItem = new List<CfgItemData>(); for (int i = 0; i < s_AwardItem.Length; i++) { f_AwardItem.Add(CfgItemData.Parse(s_AwardItem[i])); } AwardItem = f_AwardItem.ToArray(); }
		AISpeedMin = Utils.Parse.ParseFloat(values[12]);
		AISpeedMax = Utils.Parse.ParseFloat(values[13]);
		CostEnergy = Utils.Parse.ParseInt(values[14]);
		if (Utils.Parse.ParseList(values[15]) is string[]  s_CostItems) { var f_CostItems = new List<CfgItemData>(); for (int i = 0; i < s_CostItems.Length; i++) { f_CostItems.Add(CfgItemData.Parse(s_CostItems[i])); } CostItems = f_CostItems.ToArray(); }
		IsOpen = Utils.Parse.ParseInt(values[16]);
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(Name);
		buffer.Write(Loop);
		buffer.Write(Scene);
		buffer.Write(Icon);
		buffer.Write(LineMaxPlayerNum);
		buffer.Write(SceneType);
		buffer.Write(Time);
		buffer.Write(EndTime);
		buffer.Write(CarLevelLimit);
		if(AwardItem!=null) { ushort AwardItemLen = (ushort)AwardItem.Length;buffer.Write(AwardItemLen); for (int i = 0; i < AwardItemLen; i++) { CfgItemData.Encode(AwardItem[i],buffer); } }else { buffer.Write((ushort)0); }
		buffer.Write(AISpeedMin);
		buffer.Write(AISpeedMax);
		buffer.Write(CostEnergy);
		if(CostItems!=null) { ushort CostItemsLen = (ushort)CostItems.Length;buffer.Write(CostItemsLen); for (int i = 0; i < CostItemsLen; i++) { CfgItemData.Encode(CostItems[i],buffer); } }else { buffer.Write((ushort)0); }
		buffer.Write(IsOpen);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		Name = buffer.ReadString();
		Loop = buffer.ReadInt();
		Scene = buffer.ReadString();
		Icon = buffer.ReadString();
		LineMaxPlayerNum = buffer.ReadInt();
		SceneType = buffer.ReadInt();
		Time = buffer.ReadInt();
		EndTime = buffer.ReadInt();
		CarLevelLimit = buffer.ReadInt();
		var AwardItemLen = buffer.ReadUshort(); AwardItem = new CfgItemData[AwardItemLen]; for (int i = 0; i < AwardItemLen; i++) { AwardItem[i] = CfgItemData.Decode(buffer); }
		AISpeedMin = buffer.ReadFloat();
		AISpeedMax = buffer.ReadFloat();
		CostEnergy = buffer.ReadInt();
		var CostItemsLen = buffer.ReadUshort(); CostItems = new CfgItemData[CostItemsLen]; for (int i = 0; i < CostItemsLen; i++) { CostItems[i] = CfgItemData.Decode(buffer); }
		IsOpen = buffer.ReadInt();
    }
}
