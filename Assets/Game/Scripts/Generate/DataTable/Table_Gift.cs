
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class Gift_Table : DataTableLoader<Gift_Data>
{
    public override string GetTableName() { return "Gift"; }
    protected override Gift_Data CreateItem()
    {
        return new Gift_Data();
    }
    public Gift_Table()
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
    public Gift_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class Gift_Data : IDataTable
{
	public int Id { get; protected set; }
	public int Source { get; protected set; }
	public int[] ItemIds { get; protected set; }
	public int[] MaxCounts { get; protected set; }
	public int Quality { get; protected set; }
	public string Icon { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		Source = Utils.Parse.ParseInt(values[1]);
		if (Utils.Parse.ParseList(values[2]) is string[]  s_ItemIds) { var f_ItemIds = new List<int>(); for (int i = 0; i < s_ItemIds.Length; i++) { f_ItemIds.Add(Utils.Parse.ParseInt(s_ItemIds[i])); } ItemIds = f_ItemIds.ToArray(); }
		if (Utils.Parse.ParseList(values[4]) is string[]  s_MaxCounts) { var f_MaxCounts = new List<int>(); for (int i = 0; i < s_MaxCounts.Length; i++) { f_MaxCounts.Add(Utils.Parse.ParseInt(s_MaxCounts[i])); } MaxCounts = f_MaxCounts.ToArray(); }
		Quality = Utils.Parse.ParseInt(values[5]);
		Icon = values[6];
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(Source);
		if(ItemIds!=null) { ushort ItemIdsLen = (ushort)ItemIds.Length;buffer.Write(ItemIdsLen); for (int i = 0; i < ItemIdsLen; i++) { buffer.Write(ItemIds[i]); } }else { buffer.Write((ushort)0); }
		if(MaxCounts!=null) { ushort MaxCountsLen = (ushort)MaxCounts.Length;buffer.Write(MaxCountsLen); for (int i = 0; i < MaxCountsLen; i++) { buffer.Write(MaxCounts[i]); } }else { buffer.Write((ushort)0); }
		buffer.Write(Quality);
		buffer.Write(Icon);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		Source = buffer.ReadInt();
		var ItemIdsLen = buffer.ReadUshort(); ItemIds = new int[ItemIdsLen]; for (int i = 0; i < ItemIdsLen; i++) { ItemIds[i] = buffer.ReadInt(); }
		var MaxCountsLen = buffer.ReadUshort(); MaxCounts = new int[MaxCountsLen]; for (int i = 0; i < MaxCountsLen; i++) { MaxCounts[i] = buffer.ReadInt(); }
		Quality = buffer.ReadInt();
		Icon = buffer.ReadString();
    }
}
