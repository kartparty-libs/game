using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchData
{
    public bool ShowFinishTime { get; private set; }
    private float _finishBeginTime;
    private float _finishTotalTime;
    public int MatchCharacterCount { get; private set; }
    private readonly List<MatchCharacterData> MatchCharacterDatas = new List<MatchCharacterData>();
    private int rankId;
    public void Start(int mapId)
    {
        ShowFinishTime = false;
        var data = GameEntry.Table.Map.Get(mapId);
        float mEndTime = data.EndTime / 1000f;
        if (mEndTime > 0)
        {
            _finishTotalTime = mEndTime;
        }
        MatchCharacterDatas.Clear();
        rankId = 1;
    }
    public void SetWaitTime(float time)
    {
        if (ShowFinishTime) return;
        _finishTotalTime = time / 1000f;
        ShowFinishTime = true;
        _finishBeginTime = Time.time;
    }
    public void PlayerAllFinishRace()
    {
        _finishTotalTime = 0;
    }
    public void CharacterFinish(MatchCharacterData data)
    {
        data.Goal=true;
        MatchCharacterDatas.Add(data);
        MatchCharacterCount = MatchCharacterDatas.Count;
        var info = CharacterManager.Instance.GetCharacter(data.RoleID);
        if (info != null)
        {
            data.Head = info.GetCharacterInfo().HeadIconId;
        }
    }
    public MatchCharacterData GetItem(int index)
    {
        return MatchCharacterDatas[index];
    }
    public MatchCharacterData GetByRoleId(long value)
    {
        foreach (var item in MatchCharacterDatas)
        {
            if (value == item.RoleID)
            {
                return item;
            }
        }
        return null;
    }
    public MatchCharacterData GetCharacterData(int index)
    {
        if (MatchCharacterDatas.Count > index)
        {
            return MatchCharacterDatas[index];
        }
        return null;
    }
    public float GetFinishWaitTime()
    {
        if (_finishTotalTime > 0f)
        {
            var time = _finishTotalTime - (Time.time - _finishBeginTime);
            if (time < 0f)
            {
                time = 0f;
            }
            return time;
        }
        return 0f;
    }

    public void Clear()
    {
        ShowFinishTime = false;
        _finishTotalTime = 0;
        MatchCharacterDatas.Clear();
    }
}
