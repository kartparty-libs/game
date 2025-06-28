
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class Param_Table : DataTableLoader<Param_Data>
{
    public override string GetTableName() { return "Param"; }
    protected override Param_Data CreateItem()
    {
        return new Param_Data();
    }
    public Param_Table()
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
    public Param_Data Get(string key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class Param_Data : IDataTable
{
	public string Id { get; protected set; }
	public int IntParam { get; protected set; }
	public int[] IntParams { get; protected set; }
	public string TextParam { get; protected set; }
	public void Parse(string[] values)
	{
		Id = values[0];
		IntParam = Utils.Parse.ParseInt(values[1]);
		if (Utils.Parse.ParseList(values[2]) is string[]  s_IntParams) { var f_IntParams = new List<int>(); for (int i = 0; i < s_IntParams.Length; i++) { f_IntParams.Add(Utils.Parse.ParseInt(s_IntParams[i])); } IntParams = f_IntParams.ToArray(); }
		TextParam = values[3];
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(IntParam);
		if(IntParams!=null) { ushort IntParamsLen = (ushort)IntParams.Length;buffer.Write(IntParamsLen); for (int i = 0; i < IntParamsLen; i++) { buffer.Write(IntParams[i]); } }else { buffer.Write((ushort)0); }
		buffer.Write(TextParam);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadString();
		IntParam = buffer.ReadInt();
		var IntParamsLen = buffer.ReadUshort(); IntParams = new int[IntParamsLen]; for (int i = 0; i < IntParamsLen; i++) { IntParams[i] = buffer.ReadInt(); }
		TextParam = buffer.ReadString();
    }
}
