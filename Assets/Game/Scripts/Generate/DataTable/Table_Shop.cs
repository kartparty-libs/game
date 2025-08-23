
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class Shop_Table : DataTableLoader<Shop_Data>
{
    public override string GetTableName() { return "Shop"; }
    protected override Shop_Data CreateItem()
    {
        return new Shop_Data();
    }
    public Shop_Table()
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
    public Shop_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class Shop_Data : IDataTable
{
	public int Id { get; protected set; }
	public int Type { get; protected set; }
	public int Cost { get; protected set; }
	public string TonCost { get; protected set; }
	public CfgItemData[] ShopItems { get; protected set; }
	public CfgItemData[] GiveShopItems { get; protected set; }
	public string DoubleShow { get; protected set; }
	public int[] ItemId { get; protected set; }
	public int[] Count { get; protected set; }
	public int IconType { get; protected set; }
	public int TodayBuyCount { get; protected set; }
	public int TotalBuyCount { get; protected set; }
	public string Desc { get; protected set; }
	public bool IsOpen { get; protected set; }
	public string Icon { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		Type = Utils.Parse.ParseInt(values[1]);
		Cost = Utils.Parse.ParseInt(values[2]);
		TonCost = values[3];
		if (Utils.Parse.ParseList(values[4]) is string[]  s_ShopItems) { var f_ShopItems = new List<CfgItemData>(); for (int i = 0; i < s_ShopItems.Length; i++) { f_ShopItems.Add(CfgItemData.Parse(s_ShopItems[i])); } ShopItems = f_ShopItems.ToArray(); }
		if (Utils.Parse.ParseList(values[5]) is string[]  s_GiveShopItems) { var f_GiveShopItems = new List<CfgItemData>(); for (int i = 0; i < s_GiveShopItems.Length; i++) { f_GiveShopItems.Add(CfgItemData.Parse(s_GiveShopItems[i])); } GiveShopItems = f_GiveShopItems.ToArray(); }
		DoubleShow = values[6];
		if (Utils.Parse.ParseList(values[7]) is string[]  s_ItemId) { var f_ItemId = new List<int>(); for (int i = 0; i < s_ItemId.Length; i++) { f_ItemId.Add(Utils.Parse.ParseInt(s_ItemId[i])); } ItemId = f_ItemId.ToArray(); }
		if (Utils.Parse.ParseList(values[8]) is string[]  s_Count) { var f_Count = new List<int>(); for (int i = 0; i < s_Count.Length; i++) { f_Count.Add(Utils.Parse.ParseInt(s_Count[i])); } Count = f_Count.ToArray(); }
		IconType = Utils.Parse.ParseInt(values[9]);
		TodayBuyCount = Utils.Parse.ParseInt(values[10]);
		TotalBuyCount = Utils.Parse.ParseInt(values[11]);
		Desc = values[12];
		IsOpen = Utils.Parse.ParseBool(values[13]);
		Icon = values[14];
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(Type);
		buffer.Write(Cost);
		buffer.Write(TonCost);
		if(ShopItems!=null) { ushort ShopItemsLen = (ushort)ShopItems.Length;buffer.Write(ShopItemsLen); for (int i = 0; i < ShopItemsLen; i++) { CfgItemData.Encode(ShopItems[i],buffer); } }else { buffer.Write((ushort)0); }
		if(GiveShopItems!=null) { ushort GiveShopItemsLen = (ushort)GiveShopItems.Length;buffer.Write(GiveShopItemsLen); for (int i = 0; i < GiveShopItemsLen; i++) { CfgItemData.Encode(GiveShopItems[i],buffer); } }else { buffer.Write((ushort)0); }
		buffer.Write(DoubleShow);
		if(ItemId!=null) { ushort ItemIdLen = (ushort)ItemId.Length;buffer.Write(ItemIdLen); for (int i = 0; i < ItemIdLen; i++) { buffer.Write(ItemId[i]); } }else { buffer.Write((ushort)0); }
		if(Count!=null) { ushort CountLen = (ushort)Count.Length;buffer.Write(CountLen); for (int i = 0; i < CountLen; i++) { buffer.Write(Count[i]); } }else { buffer.Write((ushort)0); }
		buffer.Write(IconType);
		buffer.Write(TodayBuyCount);
		buffer.Write(TotalBuyCount);
		buffer.Write(Desc);
		buffer.Write(IsOpen);
		buffer.Write(Icon);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		Type = buffer.ReadInt();
		Cost = buffer.ReadInt();
		TonCost = buffer.ReadString();
		var ShopItemsLen = buffer.ReadUshort(); ShopItems = new CfgItemData[ShopItemsLen]; for (int i = 0; i < ShopItemsLen; i++) { ShopItems[i] = CfgItemData.Decode(buffer); }
		var GiveShopItemsLen = buffer.ReadUshort(); GiveShopItems = new CfgItemData[GiveShopItemsLen]; for (int i = 0; i < GiveShopItemsLen; i++) { GiveShopItems[i] = CfgItemData.Decode(buffer); }
		DoubleShow = buffer.ReadString();
		var ItemIdLen = buffer.ReadUshort(); ItemId = new int[ItemIdLen]; for (int i = 0; i < ItemIdLen; i++) { ItemId[i] = buffer.ReadInt(); }
		var CountLen = buffer.ReadUshort(); Count = new int[CountLen]; for (int i = 0; i < CountLen; i++) { Count[i] = buffer.ReadInt(); }
		IconType = buffer.ReadInt();
		TodayBuyCount = buffer.ReadInt();
		TotalBuyCount = buffer.ReadInt();
		Desc = buffer.ReadString();
		IsOpen = buffer.ReadBool();
		Icon = buffer.ReadString();
    }
}
