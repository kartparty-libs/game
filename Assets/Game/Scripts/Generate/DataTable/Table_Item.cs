
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class Item_Table : DataTableLoader<Item_Data>
{
    public override string GetTableName() { return "Item"; }
    protected override Item_Data CreateItem()
    {
        return new Item_Data();
    }
    public Item_Table()
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
    public Item_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class Item_Data : IDataTable
{
	public int Id { get; protected set; }
	public int Type { get; protected set; }
	public int[] Params { get; protected set; }
	public int MaxCount { get; protected set; }
	public int IsSeasonClear { get; protected set; }
	public string Name { get; protected set; }
	public string SIcon { get; protected set; }
	public string BIcon { get; protected set; }
	public int ShowRate { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		Type = Utils.Parse.ParseInt(values[1]);
		if (Utils.Parse.ParseList(values[2]) is string[]  s_Params) { var f_Params = new List<int>(); for (int i = 0; i < s_Params.Length; i++) { f_Params.Add(Utils.Parse.ParseInt(s_Params[i])); } Params = f_Params.ToArray(); }
		MaxCount = Utils.Parse.ParseInt(values[3]);
		IsSeasonClear = Utils.Parse.ParseInt(values[4]);
		Name = values[5];
		SIcon = values[6];
		BIcon = values[7];
		ShowRate = Utils.Parse.ParseInt(values[8]);
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(Type);
		if(Params!=null) { ushort ParamsLen = (ushort)Params.Length;buffer.Write(ParamsLen); for (int i = 0; i < ParamsLen; i++) { buffer.Write(Params[i]); } }else { buffer.Write((ushort)0); }
		buffer.Write(MaxCount);
		buffer.Write(IsSeasonClear);
		buffer.Write(Name);
		buffer.Write(SIcon);
		buffer.Write(BIcon);
		buffer.Write(ShowRate);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		Type = buffer.ReadInt();
		var ParamsLen = buffer.ReadUshort(); Params = new int[ParamsLen]; for (int i = 0; i < ParamsLen; i++) { Params[i] = buffer.ReadInt(); }
		MaxCount = buffer.ReadInt();
		IsSeasonClear = buffer.ReadInt();
		Name = buffer.ReadString();
		SIcon = buffer.ReadString();
		BIcon = buffer.ReadString();
		ShowRate = buffer.ReadInt();
    }
}
