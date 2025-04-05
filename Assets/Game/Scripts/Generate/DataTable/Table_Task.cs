
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class Task_Table : DataTableLoader<Task_Data>
{
    public override string GetTableName() { return "Task"; }
    protected override Task_Data CreateItem()
    {
        return new Task_Data();
    }
    public Task_Table()
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
    public Task_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class Task_Data : IDataTable
{
	public int Id { get; protected set; }
	public int TitleType { get; protected set; }
	public int TaskType { get; protected set; }
	public int TaskEvent { get; protected set; }
	public int JumpUI { get; protected set; }
	public int[] TaskConditionParams { get; protected set; }
	public int[] TaskConditionTypes { get; protected set; }
	public int TaskValueParam { get; protected set; }
	public int Day { get; protected set; }
	public int TaskScoreAward { get; protected set; }
	public int IconType { get; protected set; }
	public int[] TaskAward { get; protected set; }
	public int[] TaskTreasureChestAward { get; protected set; }
	public string TaskDesc { get; protected set; }
	public string Title { get; protected set; }
	public int Quality { get; protected set; }
	public string Icon { get; protected set; }
	public string Url { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		TitleType = Utils.Parse.ParseInt(values[1]);
		TaskType = Utils.Parse.ParseInt(values[2]);
		TaskEvent = Utils.Parse.ParseInt(values[3]);
		JumpUI = Utils.Parse.ParseInt(values[5]);
		if (Utils.Parse.ParseList(values[6]) is string[]  s_TaskConditionParams) { var f_TaskConditionParams = new List<int>(); for (int i = 0; i < s_TaskConditionParams.Length; i++) { f_TaskConditionParams.Add(Utils.Parse.ParseInt(s_TaskConditionParams[i])); } TaskConditionParams = f_TaskConditionParams.ToArray(); }
		if (Utils.Parse.ParseList(values[7]) is string[]  s_TaskConditionTypes) { var f_TaskConditionTypes = new List<int>(); for (int i = 0; i < s_TaskConditionTypes.Length; i++) { f_TaskConditionTypes.Add(Utils.Parse.ParseInt(s_TaskConditionTypes[i])); } TaskConditionTypes = f_TaskConditionTypes.ToArray(); }
		TaskValueParam = Utils.Parse.ParseInt(values[8]);
		Day = Utils.Parse.ParseInt(values[9]);
		TaskScoreAward = Utils.Parse.ParseInt(values[10]);
		IconType = Utils.Parse.ParseInt(values[11]);
		if (Utils.Parse.ParseList(values[12]) is string[]  s_TaskAward) { var f_TaskAward = new List<int>(); for (int i = 0; i < s_TaskAward.Length; i++) { f_TaskAward.Add(Utils.Parse.ParseInt(s_TaskAward[i])); } TaskAward = f_TaskAward.ToArray(); }
		if (Utils.Parse.ParseList(values[13]) is string[]  s_TaskTreasureChestAward) { var f_TaskTreasureChestAward = new List<int>(); for (int i = 0; i < s_TaskTreasureChestAward.Length; i++) { f_TaskTreasureChestAward.Add(Utils.Parse.ParseInt(s_TaskTreasureChestAward[i])); } TaskTreasureChestAward = f_TaskTreasureChestAward.ToArray(); }
		TaskDesc = values[14];
		Title = values[15];
		Quality = Utils.Parse.ParseInt(values[16]);
		Icon = values[17];
		Url = values[18];
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(TitleType);
		buffer.Write(TaskType);
		buffer.Write(TaskEvent);
		buffer.Write(JumpUI);
		if(TaskConditionParams!=null) { ushort TaskConditionParamsLen = (ushort)TaskConditionParams.Length;buffer.Write(TaskConditionParamsLen); for (int i = 0; i < TaskConditionParamsLen; i++) { buffer.Write(TaskConditionParams[i]); } }else { buffer.Write((ushort)0); }
		if(TaskConditionTypes!=null) { ushort TaskConditionTypesLen = (ushort)TaskConditionTypes.Length;buffer.Write(TaskConditionTypesLen); for (int i = 0; i < TaskConditionTypesLen; i++) { buffer.Write(TaskConditionTypes[i]); } }else { buffer.Write((ushort)0); }
		buffer.Write(TaskValueParam);
		buffer.Write(Day);
		buffer.Write(TaskScoreAward);
		buffer.Write(IconType);
		if(TaskAward!=null) { ushort TaskAwardLen = (ushort)TaskAward.Length;buffer.Write(TaskAwardLen); for (int i = 0; i < TaskAwardLen; i++) { buffer.Write(TaskAward[i]); } }else { buffer.Write((ushort)0); }
		if(TaskTreasureChestAward!=null) { ushort TaskTreasureChestAwardLen = (ushort)TaskTreasureChestAward.Length;buffer.Write(TaskTreasureChestAwardLen); for (int i = 0; i < TaskTreasureChestAwardLen; i++) { buffer.Write(TaskTreasureChestAward[i]); } }else { buffer.Write((ushort)0); }
		buffer.Write(TaskDesc);
		buffer.Write(Title);
		buffer.Write(Quality);
		buffer.Write(Icon);
		buffer.Write(Url);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		TitleType = buffer.ReadInt();
		TaskType = buffer.ReadInt();
		TaskEvent = buffer.ReadInt();
		JumpUI = buffer.ReadInt();
		var TaskConditionParamsLen = buffer.ReadUshort(); TaskConditionParams = new int[TaskConditionParamsLen]; for (int i = 0; i < TaskConditionParamsLen; i++) { TaskConditionParams[i] = buffer.ReadInt(); }
		var TaskConditionTypesLen = buffer.ReadUshort(); TaskConditionTypes = new int[TaskConditionTypesLen]; for (int i = 0; i < TaskConditionTypesLen; i++) { TaskConditionTypes[i] = buffer.ReadInt(); }
		TaskValueParam = buffer.ReadInt();
		Day = buffer.ReadInt();
		TaskScoreAward = buffer.ReadInt();
		IconType = buffer.ReadInt();
		var TaskAwardLen = buffer.ReadUshort(); TaskAward = new int[TaskAwardLen]; for (int i = 0; i < TaskAwardLen; i++) { TaskAward[i] = buffer.ReadInt(); }
		var TaskTreasureChestAwardLen = buffer.ReadUshort(); TaskTreasureChestAward = new int[TaskTreasureChestAwardLen]; for (int i = 0; i < TaskTreasureChestAwardLen; i++) { TaskTreasureChestAward[i] = buffer.ReadInt(); }
		TaskDesc = buffer.ReadString();
		Title = buffer.ReadString();
		Quality = buffer.ReadInt();
		Icon = buffer.ReadString();
		Url = buffer.ReadString();
    }
}
