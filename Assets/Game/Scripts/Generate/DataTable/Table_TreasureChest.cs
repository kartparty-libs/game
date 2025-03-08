
using UnityEngine;
using System.Collections.Generic;
using Framework.DataTable;
using Framework;
public partial class TreasureChest_Table : DataTableLoader<TreasureChest_Data>
{
    public override string GetTableName() { return "TreasureChest"; }
    protected override TreasureChest_Data CreateItem()
    {
        return new TreasureChest_Data();
    }
    public TreasureChest_Table()
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
    public TreasureChest_Data Get(int key)
    {
        if (_keyToIndex!=null && _keyToIndex.TryGetValue(key, out int index))
        {
            return GetItem(index);
        }
        return null;
    }
#endif
}

public partial class TreasureChest_Data : IDataTable
{
	public int Id { get; protected set; }
	public string Name { get; protected set; }
	public string Desc { get; protected set; }
	public int Type { get; protected set; }
	public int[] RandomScore { get; protected set; }
	public int[] FusionParam { get; protected set; }
	public string ImagePath { get; protected set; }
	public string BgPath { get; protected set; }
	public void Parse(string[] values)
	{
		Id = Utils.Parse.ParseInt(values[0]);
		Name = values[1];
		Desc = values[2];
		Type = Utils.Parse.ParseInt(values[3]);
		if (Utils.Parse.ParseList(values[4]) is string[]  s_RandomScore) { var f_RandomScore = new List<int>(); for (int i = 0; i < s_RandomScore.Length; i++) { f_RandomScore.Add(Utils.Parse.ParseInt(s_RandomScore[i])); } RandomScore = f_RandomScore.ToArray(); }
		if (Utils.Parse.ParseList(values[5]) is string[]  s_FusionParam) { var f_FusionParam = new List<int>(); for (int i = 0; i < s_FusionParam.Length; i++) { f_FusionParam.Add(Utils.Parse.ParseInt(s_FusionParam[i])); } FusionParam = f_FusionParam.ToArray(); }
		ImagePath = values[6];
		BgPath = values[7];
	}
    public void Encode(ByteArray buffer)
    {
		buffer.Write(Id);
		buffer.Write(Name);
		buffer.Write(Desc);
		buffer.Write(Type);
		if(RandomScore!=null) { ushort RandomScoreLen = (ushort)RandomScore.Length;buffer.Write(RandomScoreLen); for (int i = 0; i < RandomScoreLen; i++) { buffer.Write(RandomScore[i]); } }else { buffer.Write((ushort)0); }
		if(FusionParam!=null) { ushort FusionParamLen = (ushort)FusionParam.Length;buffer.Write(FusionParamLen); for (int i = 0; i < FusionParamLen; i++) { buffer.Write(FusionParam[i]); } }else { buffer.Write((ushort)0); }
		buffer.Write(ImagePath);
		buffer.Write(BgPath);
    }
    public virtual void Decode(ByteArray buffer)
    {
		Id = buffer.ReadInt();
		Name = buffer.ReadString();
		Desc = buffer.ReadString();
		Type = buffer.ReadInt();
		var RandomScoreLen = buffer.ReadUshort(); RandomScore = new int[RandomScoreLen]; for (int i = 0; i < RandomScoreLen; i++) { RandomScore[i] = buffer.ReadInt(); }
		var FusionParamLen = buffer.ReadUshort(); FusionParam = new int[FusionParamLen]; for (int i = 0; i < FusionParamLen; i++) { FusionParam[i] = buffer.ReadInt(); }
		ImagePath = buffer.ReadString();
		BgPath = buffer.ReadString();
    }
}
