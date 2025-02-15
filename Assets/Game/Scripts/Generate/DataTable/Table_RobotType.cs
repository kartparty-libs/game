
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class RobotType_Table : DataTableLoader<RobotType_Data>
{
    public override string GetTableName() { return "RobotType"; }
    protected override RobotType_Data CreateItem()
    {
        return new RobotType_Data();
    }
    public RobotType_Table()
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
    public RobotType_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class RobotType_Data : IDataTable
{
	public int Id { get; private set; }
	public string Name { get; private set; }
	public bool Open { get; private set; }
	public float Time { get; private set; }
	public Vector3 Position { get; private set; }
	public int[] PList { get; private set; }
	public Vector3[] PosList { get; private set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		Name = values[1];
		Open = Utils.Parse.ParseBool(values[2]);
		Time = Utils.Parse.ParseFloat(values[3]);
		Position = Vector3Ext.Parse(values[4]);
		if (Utils.Parse.ParseList(values[5]) is string[]  s_PList) { var f_PList = new List<int>(); for (int i = 0; i < s_PList.Length; i++) { f_PList.Add(Utils.Parse.ParseInt(s_PList[i])); } PList = f_PList.ToArray(); }
		if (Utils.Parse.ParseList(values[6]) is string[]  s_PosList) { var f_PosList = new List<Vector3>(); for (int i = 0; i < s_PosList.Length; i++) { f_PosList.Add(Vector3Ext.Parse(s_PosList[i])); } PosList = f_PosList.ToArray(); }
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(Name);
		buffer.Write(Open);
		buffer.Write(Time);
		Vector3Ext.Encode(Position,buffer);
		if(PList!=null) { ushort PListLen = (ushort)PList.Length;buffer.Write(PListLen); for (int i = 0; i < PListLen; i++) { buffer.Write(PList[i]); } }else { buffer.Write((ushort)0); }
		if(PosList!=null) { ushort PosListLen = (ushort)PosList.Length;buffer.Write(PosListLen); for (int i = 0; i < PosListLen; i++) { Vector3Ext.Encode(PosList[i],buffer); } }else { buffer.Write((ushort)0); }
    }
    public void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		Name = buffer.ReadString();
		Open = buffer.ReadBool();
		Time = buffer.ReadFloat();
		Position = Vector3Ext.Decode(buffer);
		var PListLen = buffer.ReadUshort(); PList = new int[PListLen]; for (int i = 0; i < PListLen; i++) { PList[i] = buffer.ReadInt(); }
		var PosListLen = buffer.ReadUshort(); PosList = new Vector3[PosListLen]; for (int i = 0; i < PosListLen; i++) { PosList[i] = Vector3Ext.Decode(buffer); }
    }
}
