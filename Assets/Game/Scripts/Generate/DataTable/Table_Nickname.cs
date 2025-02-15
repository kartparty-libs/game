
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class Nickname_Table : DataTableLoader<Nickname_Data>
{
    public override string GetTableName() { return "Nickname"; }
    protected override Nickname_Data CreateItem()
    {
        return new Nickname_Data();
    }
    public Nickname_Table()
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
    public Nickname_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class Nickname_Data : IDataTable
{
	public int Id { get; protected set; }
	public string FirstName { get; protected set; }
	public string MiddleName { get; protected set; }
	public string LastName { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		FirstName = values[1];
		MiddleName = values[2];
		LastName = values[3];
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(FirstName);
		buffer.Write(MiddleName);
		buffer.Write(LastName);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		FirstName = buffer.ReadString();
		MiddleName = buffer.ReadString();
		LastName = buffer.ReadString();
    }
}
