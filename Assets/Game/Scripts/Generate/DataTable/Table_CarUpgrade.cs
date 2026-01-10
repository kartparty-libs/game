
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class CarUpgrade_Table : DataTableLoader<CarUpgrade_Data>
{
    public override string GetTableName() { return "CarUpgrade"; }
    protected override CarUpgrade_Data CreateItem()
    {
        return new CarUpgrade_Data();
    }
    public CarUpgrade_Table()
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
    public CarUpgrade_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class CarUpgrade_Data : IDataTable
{
	public int Id { get; protected set; }
	public long Cost1 { get; protected set; }
	public int[] CostItem1 { get; protected set; }
	public int Property1 { get; protected set; }
	public int Scoring1 { get; protected set; }
	public long Cost2 { get; protected set; }
	public int[] CostItem2 { get; protected set; }
	public int Property2 { get; protected set; }
	public int Scoring2 { get; protected set; }
	public long Cost3 { get; protected set; }
	public int[] CostItem3 { get; protected set; }
	public int Property3 { get; protected set; }
	public int Scoring3 { get; protected set; }
	public long Cost4 { get; protected set; }
	public int[] CostItem4 { get; protected set; }
	public int Property4 { get; protected set; }
	public int Scoring4 { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		Cost1 = Utils.Parse.ParseLong(values[1]);
		if (Utils.Parse.ParseList(values[2]) is string[]  s_CostItem1) { var f_CostItem1 = new List<int>(); for (int i = 0; i < s_CostItem1.Length; i++) { f_CostItem1.Add(Utils.Parse.ParseInt(s_CostItem1[i])); } CostItem1 = f_CostItem1.ToArray(); }
		Property1 = Utils.Parse.ParseInt(values[3]);
		Scoring1 = Utils.Parse.ParseInt(values[4]);
		Cost2 = Utils.Parse.ParseLong(values[5]);
		if (Utils.Parse.ParseList(values[6]) is string[]  s_CostItem2) { var f_CostItem2 = new List<int>(); for (int i = 0; i < s_CostItem2.Length; i++) { f_CostItem2.Add(Utils.Parse.ParseInt(s_CostItem2[i])); } CostItem2 = f_CostItem2.ToArray(); }
		Property2 = Utils.Parse.ParseInt(values[7]);
		Scoring2 = Utils.Parse.ParseInt(values[8]);
		Cost3 = Utils.Parse.ParseLong(values[9]);
		if (Utils.Parse.ParseList(values[10]) is string[]  s_CostItem3) { var f_CostItem3 = new List<int>(); for (int i = 0; i < s_CostItem3.Length; i++) { f_CostItem3.Add(Utils.Parse.ParseInt(s_CostItem3[i])); } CostItem3 = f_CostItem3.ToArray(); }
		Property3 = Utils.Parse.ParseInt(values[11]);
		Scoring3 = Utils.Parse.ParseInt(values[12]);
		Cost4 = Utils.Parse.ParseLong(values[13]);
		if (Utils.Parse.ParseList(values[14]) is string[]  s_CostItem4) { var f_CostItem4 = new List<int>(); for (int i = 0; i < s_CostItem4.Length; i++) { f_CostItem4.Add(Utils.Parse.ParseInt(s_CostItem4[i])); } CostItem4 = f_CostItem4.ToArray(); }
		Property4 = Utils.Parse.ParseInt(values[15]);
		Scoring4 = Utils.Parse.ParseInt(values[16]);
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(Cost1);
		if(CostItem1!=null) { ushort CostItem1Len = (ushort)CostItem1.Length;buffer.Write(CostItem1Len); for (int i = 0; i < CostItem1Len; i++) { buffer.Write(CostItem1[i]); } }else { buffer.Write((ushort)0); }
		buffer.Write(Property1);
		buffer.Write(Scoring1);
		buffer.Write(Cost2);
		if(CostItem2!=null) { ushort CostItem2Len = (ushort)CostItem2.Length;buffer.Write(CostItem2Len); for (int i = 0; i < CostItem2Len; i++) { buffer.Write(CostItem2[i]); } }else { buffer.Write((ushort)0); }
		buffer.Write(Property2);
		buffer.Write(Scoring2);
		buffer.Write(Cost3);
		if(CostItem3!=null) { ushort CostItem3Len = (ushort)CostItem3.Length;buffer.Write(CostItem3Len); for (int i = 0; i < CostItem3Len; i++) { buffer.Write(CostItem3[i]); } }else { buffer.Write((ushort)0); }
		buffer.Write(Property3);
		buffer.Write(Scoring3);
		buffer.Write(Cost4);
		if(CostItem4!=null) { ushort CostItem4Len = (ushort)CostItem4.Length;buffer.Write(CostItem4Len); for (int i = 0; i < CostItem4Len; i++) { buffer.Write(CostItem4[i]); } }else { buffer.Write((ushort)0); }
		buffer.Write(Property4);
		buffer.Write(Scoring4);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		Cost1 = buffer.ReadLong();
		var CostItem1Len = buffer.ReadUshort(); CostItem1 = new int[CostItem1Len]; for (int i = 0; i < CostItem1Len; i++) { CostItem1[i] = buffer.ReadInt(); }
		Property1 = buffer.ReadInt();
		Scoring1 = buffer.ReadInt();
		Cost2 = buffer.ReadLong();
		var CostItem2Len = buffer.ReadUshort(); CostItem2 = new int[CostItem2Len]; for (int i = 0; i < CostItem2Len; i++) { CostItem2[i] = buffer.ReadInt(); }
		Property2 = buffer.ReadInt();
		Scoring2 = buffer.ReadInt();
		Cost3 = buffer.ReadLong();
		var CostItem3Len = buffer.ReadUshort(); CostItem3 = new int[CostItem3Len]; for (int i = 0; i < CostItem3Len; i++) { CostItem3[i] = buffer.ReadInt(); }
		Property3 = buffer.ReadInt();
		Scoring3 = buffer.ReadInt();
		Cost4 = buffer.ReadLong();
		var CostItem4Len = buffer.ReadUshort(); CostItem4 = new int[CostItem4Len]; for (int i = 0; i < CostItem4Len; i++) { CostItem4[i] = buffer.ReadInt(); }
		Property4 = buffer.ReadInt();
		Scoring4 = buffer.ReadInt();
    }
}
