using System;
using System.Collections.Generic;
using Proto;
using UnityEngine;

public static class GameDefine
{
#if UNITY_EDITOR
    //编译器
    // public const string LoginServer = "http://127.0.0.1:9885/kp_game/imx_loginserver/proto";
    public const string LoginServer = "https://www.kartparty.net/kp_game/imx_loginserver/proto";
#else
#if true
    //正式地址
    public const string LoginServer = "https://www.kartparty.net/kp_game/imx_loginserver/proto";
#else
    //测试地址
    public const string LoginServer = "https://www.kartparty.net/kp_game_test/imx_loginserver/proto";
#endif
#endif

    /// <summary>
    /// 游戏重力加速度
    /// </summary>
    public const float Gravity = -9.81f;

    /// <summary>
    /// 相机模式
    /// </summary>
    public const EnumDefine.CameraModleEnum CameraModle = EnumDefine.CameraModleEnum.Normal;

    /// <summary>
    /// 同步数值倍率
    /// </summary>
    public const float SyncValueMultiple = 1000f;

    public const string PreRelease = nameof(PreRelease);
    public static bool IsPreRelease()
    {
        if (PlayerPrefs.HasKey(GameDefine.PreRelease))
        {
            if (PlayerPrefs.GetString(GameDefine.PreRelease) == "1")
            {
                return true;
            }
        }
        return false;
    }
}
/// <summary>
/// 任务数据
/// </summary>
public class TaskData
{
    /// <summary>
    /// 任务配置Id
    /// </summary>
    public int taskCfgId = 0;
    /// <summary>
    /// 任务数值
    /// </summary>
    public long taskValue = 0;
    /// <summary>
    /// 是否已领取奖励
    /// </summary>
    public bool isReceiveAward = false;

    public bool Running = false;
}
public class ShopBuyItemData
{
    public int TplId;
    public int Rate;
}
public class OpenBoxResultData
{
    public int CfgId;
    public long Value;
    public long Score;
}
public enum RedDotName
{
    //免费加速
    FreeSpeedUp,
    //免费购买
    FreeShopBuy,
    //任务
    TaskAll,
    Task,
    Achievement,
    Challenge,
    NewMap,
    //前置任务
    OKXTask,
    OKXTaskSuccess,
    TurntableEntry,
    TurntableTask,
    SeasonSystemTask,
    Achievement_InviteFriend,
    Action_Check_Invite,
    IconInvateFriendInfo,

}
public class RankConfigData
{
    public RankTypeEnum Type;
    public bool HasData
    {
        get
        {
            return NextUpdateTime > 0;
        }
    }
    public int NeedCount;
    public long NextUpdateTime;
    public ValueRankData SelfData;
    public bool FullData;

    public List<ValueRankData> All = new List<ValueRankData>();
    private Dictionary<long, ValueRankData> map = new Dictionary<long, ValueRankData>();

    public List<DateTime> StartTimeList;
    public List<DateTime> SettlementTimeList;
    public List<DateTime> EndTimeList;
    public int TimeId;
    public DateTime TimeStart;
    public DateTime TimeSettlement;
    public void AddItem(ValueRankData value)
    {
        if (map.TryGetValue(value.Key, out var valueRankData))
        {
            valueRankData.From(value);
            return;
        }
        map.Add(value.Key, value);
        All.Add(value);
    }
    public void Clear()
    {
        SelfData = null;
        FullData = false;
        NextUpdateTime = 0;
        All.Clear();
        map.Clear();
        StartTimeList = null;
        SettlementTimeList = null;
        EndTimeList = null;
    }
    public void CheckTimePhase()
    {
        TimeId = -1;
        if (StartTimeList == null)
        {
            return;
        }
        var now = GameEntry.OfflineManager.GetServerDateTime();
        var len = StartTimeList.Count;
        while (len-- > 0)
        {
            TimeStart = StartTimeList[len];
            TimeSettlement = SettlementTimeList[len];
            if (now > TimeStart && now < TimeSettlement)
            {
                TimeId = len;
                break;
            }
        }
    }
}
public class ValueRankData
{
    public long Key;
    public RankTypeEnum Type;
    public int Rank;
    public long Value;
    public long[] Values;
    public long Time;
    public RankOtherInfo_PlayerInfo Info;
    public void From(ValueRankData from)
    {
        Rank = from.Rank;
        Value = from.Value;
        Time = from.Time;
        Info = from.Info;
        Values = from.Values;
    }
    public long GetValue(int index)
    {
        if (index < 0 || index >= Values.Length)
        {
            return 0;
        }
        return Values[index];
    }

}
/// <summary>
/// 排行榜其他信息_玩家简要信息
/// </summary>

public class RankOtherInfo_PlayerInfo
{
    public int serverId;
    public long roleId;
    public string name;
    public int headId;
    public string email;
}
public class WalletInfo
{
    public string Address;
    public string Name;
    public string ImageUrl;
    public string Chain;
}
public class CryptoCoinType
{
    public const string TON = nameof(TON);
    public const string TONCOIN = nameof(TONCOIN);
    public const string USDT = nameof(USDT);
}
public class WalletName
{
    public const string TON = nameof(TON);
    public const string OKX = nameof(OKX);
    public const string AEON = nameof(AEON);
}
public class ConfirmData
{
    public string Title;
    public string Content;
    public string CancelText;
    public string OkText;
    public Action Cancel;
    public Action Ok;
    public ConfirmData()
    {
        Title = "";
        Content = "";
        CancelText = "Cancel";
        OkText = "OK";
    }
}
public enum ExecuteActionType
{
    Nothing,
    AddEnergy,//加体力
    AddMoneyGold,
    AddMoneyK,
    AddMoneyDiamond,
    //pvp界面
    Map_Pvp,
    Map_Pve,
    GarageUI,
    Rank,
    Shop,
    Task_Achievement,
    Action_InviteFriends,
    UI_InviteFriend,
}
public enum BindValueType
{
    Nothing,
    Gold = 1,
    Diamond = 2,
    KartMoney = 3,
    Energy,
    ChangedToken,
    InviteFriendCount,
    InviteFriendKCount,
}
public class PropertyInfo
{
    public int Level;
    public float Progress;
    public long Cost;
    public int CostItemId;
    public long CostItemCount;
    public float Min;
    public float Max;
    public float Value;
    public float NextValue;
    public int Score;
    public int MaxScore;
}
public class PropertyUpgradeInfo
{
    public List<int> PropertyLvs = new List<int>();
    public void Grab()
    {
        PropertyLvs.Clear();
        for (var i = 0; i < 4; i++)
        {
            var info = CarCultivateSystem.Instance.GetModuleLevel((CarModuleType)(i + 1));
            PropertyLvs.Add(info);
        }
    }

}