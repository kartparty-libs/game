
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class CarModules_Table : DataTableLoader<CarModules_Data>
{
    public override string GetTableName() { return "CarModules"; }
    protected override CarModules_Data CreateItem()
    {
        return new CarModules_Data();
    }
    public CarModules_Table()
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
    public CarModules_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class CarModules_Data : IDataTable
{
	public int Id { get; protected set; }
	public string Name { get; protected set; }
	public int[] QualityLevel { get; protected set; }
	public CarMoudleProperty[] Propertys { get; protected set; }
	public string Asset { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		Name = values[1];
		if (Utils.Parse.ParseList(values[2]) is string[]  s_QualityLevel) { var f_QualityLevel = new List<int>(); for (int i = 0; i < s_QualityLevel.Length; i++) { f_QualityLevel.Add(Utils.Parse.ParseInt(s_QualityLevel[i])); } QualityLevel = f_QualityLevel.ToArray(); }
		if (Utils.Parse.ParseList(values[3]) is string[]  s_Propertys) { var f_Propertys = new List<CarMoudleProperty>(); for (int i = 0; i < s_Propertys.Length; i++) { f_Propertys.Add(CarMoudleProperty.Parse(s_Propertys[i])); } Propertys = f_Propertys.ToArray(); }
		Asset = values[4];
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(Name);
		if(QualityLevel!=null) { ushort QualityLevelLen = (ushort)QualityLevel.Length;buffer.Write(QualityLevelLen); for (int i = 0; i < QualityLevelLen; i++) { buffer.Write(QualityLevel[i]); } }else { buffer.Write((ushort)0); }
		if(Propertys!=null) { ushort PropertysLen = (ushort)Propertys.Length;buffer.Write(PropertysLen); for (int i = 0; i < PropertysLen; i++) { CarMoudleProperty.Encode(Propertys[i],buffer); } }else { buffer.Write((ushort)0); }
		buffer.Write(Asset);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		Name = buffer.ReadString();
		var QualityLevelLen = buffer.ReadUshort(); QualityLevel = new int[QualityLevelLen]; for (int i = 0; i < QualityLevelLen; i++) { QualityLevel[i] = buffer.ReadInt(); }
		var PropertysLen = buffer.ReadUshort(); Propertys = new CarMoudleProperty[PropertysLen]; for (int i = 0; i < PropertysLen; i++) { Propertys[i] = CarMoudleProperty.Decode(buffer); }
		Asset = buffer.ReadString();
    }
}
