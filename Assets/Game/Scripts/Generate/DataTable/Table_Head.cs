
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class Head_Table : DataTableLoader<Head_Data>
{
    public override string GetTableName() { return "Head"; }
    protected override Head_Data CreateItem()
    {
        return new Head_Data();
    }
    public Head_Table()
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
    public Head_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class Head_Data : IDataTable
{
	public int Id { get; protected set; }
	public bool Sex { get; protected set; }
	public string Name { get; protected set; }
	public string BigIcon { get; protected set; }
	public string SmallIcon { get; protected set; }
	public string TinyIcon { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		Sex = Utils.Parse.ParseBool(values[1]);
		Name = values[2];
		BigIcon = values[3];
		SmallIcon = values[4];
		TinyIcon = values[5];
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(Sex);
		buffer.Write(Name);
		buffer.Write(BigIcon);
		buffer.Write(SmallIcon);
		buffer.Write(TinyIcon);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		Sex = buffer.ReadBool();
		Name = buffer.ReadString();
		BigIcon = buffer.ReadString();
		SmallIcon = buffer.ReadString();
		TinyIcon = buffer.ReadString();
    }
}
