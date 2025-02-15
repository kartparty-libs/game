using System.Collections.Generic;
using System.Linq;
using Framework;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using Proto;

/// <summary>
/// 排行榜系统
/// </summary>
public class RankSystem : BaseSystem<RankSystem>
{
    public int RefreshRemainSeconds { get; private set; }
    private const int maxCount = 500;
    public const int pageCount = 50;
    private Dictionary<RankTypeEnum, RankConfigData> rankData = new Dictionary<RankTypeEnum, RankConfigData>();

    public void Clear()
    {
        rankData.Clear();
    }
    public void NewDay()
    {
        foreach (var item in rankData)
        {
            item.Value.Clear();
        }
    }
    public void CheckRefreshTime(Proto.RankTypeEnum type)
    {
        var info = GetRankInfo(type);
        if (info.NextUpdateTime < 1)
        {
            return;
        }
        var endTime = TimeUtils.GetDateTimeByMilliseconds(info.NextUpdateTime);
        var diff = TimeUtils.GetDiff(GameEntry.OfflineManager.GetServerDateTime(), endTime);
        if (diff.TotalSeconds > 0)
        {
            return;
        }
        info.Clear();
    }
    public void Request(Proto.RankTypeEnum type, int needCount = -1)
    {
        if (type == RankTypeEnum.SeasonMedalLast)
        {
            return;
        }

        var info = GetRankInfo(type);
        if (info.FullData)
        {
            return;
        }
        var count = pageCount;
        if (needCount > 0)
        {
            count = needCount;
        }
        int offset = info.All.Count;
        if (offset + count > maxCount)
        {
            count = maxCount - offset;
        }
        if (count < 1)
        {
            return;
        }
        info.NeedCount = count;
        GameEntry.Http.Handler.GetRank(type, offset, count);
    }
    public void RequestSelf(Proto.RankTypeEnum type)
    {
        var info = GetRankInfo(type);
        if (info.HasData)
        {
            return;
        }
        info.NeedCount = 0;
        GameEntry.Http.Handler.GetRank(type, 0, 1);
    }
    public void OnRankData(ResMsgBodyCommonRankManager res)
    {
        var ranktype = res.RankTypeEnum;
        if (res.IsBackup)
        {

        }
        if (!rankData.TryGetValue(ranktype, out var data))
        {
            data = new RankConfigData();
            data.Type = ranktype;
            rankData.Add(ranktype, data);
        }
        data.NextUpdateTime = res.NextUpdateTime;
        if (res.SelfRankData != null && res.SelfRankData.Rank <= 3000)
        {
            data.SelfData = GetRankData(ranktype, res.SelfRankData);
        }
        else
        {
            data.SelfData = null;
        }
        data.FullData = data.NeedCount > 0 && res.RankDataList.Count < data.NeedCount;
        foreach (var item in res.RankDataList)
        {
            var d = GetRankData(ranktype, item);
            if (d == null)
            {
                continue;
            }
            data.AddItem(d);
        }
        EventManager.Instance.Dispatch(EventDefine.Global.OnRankChange);
    }
    public RankConfigData GetRankInfo(RankTypeEnum type)
    {
        if (!rankData.TryGetValue(type, out var info))
        {
            info = new RankConfigData();
            info.Type = type;
            rankData.Add(type, info);
        }
        if (type == Proto.RankTypeEnum.SeasonMedal)
        {
            if (info.StartTimeList == null)
            {
                info.StartTimeList = new List<System.DateTime>();
                info.SettlementTimeList = new List<System.DateTime>();
                info.EndTimeList = new List<System.DateTime>();
                var list = GameEntry.Table.Param.Get("SeasonMedalStartTime").TextParam.Split(",");
                foreach (var item in list)
                {
                    info.StartTimeList.Add(TimeUtils.GetDateTimeByMilliseconds(TimeUtils.ConvertToUtcTimestampMilliseconds(item)));
                }
                list = GameEntry.Table.Param.Get("SeasonMedalEndTime").TextParam.Split(",");
                foreach (var item in list)
                {
                    info.SettlementTimeList.Add(TimeUtils.GetDateTimeByMilliseconds(TimeUtils.ConvertToUtcTimestampMilliseconds(item)));
                }
            }
        }
        else if (type == Proto.RankTypeEnum.SeasonLeagueXp)
        {
            if (info.StartTimeList == null)
            {
                info.StartTimeList = new List<System.DateTime>();
                info.SettlementTimeList = new List<System.DateTime>();
                info.EndTimeList = new List<System.DateTime>();
                var list = GameEntry.Table.Param.Get("SeasonStartTime").TextParam.Split(",");
                foreach (var item in list)
                {
                    info.StartTimeList.Add(TimeUtils.GetDateTimeByMilliseconds(TimeUtils.ConvertToUtcTimestampMilliseconds(item)));
                }
                list = GameEntry.Table.Param.Get("SeasonSettlementTime").TextParam.Split(",");
                foreach (var item in list)
                {
                    info.SettlementTimeList.Add(TimeUtils.GetDateTimeByMilliseconds(TimeUtils.ConvertToUtcTimestampMilliseconds(item)));
                }
                list = GameEntry.Table.Param.Get("SeasonEndTime").TextParam.Split(",");
                foreach (var item in list)
                {
                    info.EndTimeList.Add(TimeUtils.GetDateTimeByMilliseconds(TimeUtils.ConvertToUtcTimestampMilliseconds(item)));
                }
            }
        }
        return info;
    }
    private ValueRankData GetRankData(RankTypeEnum type, ResMsgBodyRankData data)
    {
        if (data == null)
        {
            return null;
        }
        var other = Utils.Json.ToObject<RankOtherInfo_PlayerInfo>(data.OtherInfo);
        ValueRankData ins = null;
        ins = new ValueRankData();
        ins.Key = data.Key;
        ins.Rank = (int)data.Rank;
        ins.Info = other;
        ins.Type = type;
        ins.Value = data.Values[0];
        ins.Values = data.Values.ToArray<long>();
        ins.Time = data.Time;
        return ins;
    }
}

