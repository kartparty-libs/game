using System;
using System.Collections.Generic;
using Framework;

public static class EnumDefine
{

    /// <summary>
    /// 配置布局类型
    /// </summary>
    public static class ConfigLayout
    {
        /// <summary>
        /// 字符串布局
        /// </summary>
        public const int StringLayout = 0;

        /// <summary>
        /// 二进制布局
        /// </summary>
        public const int BinaryLayout = 1;
    }

    /// <summary>
    /// 玩家被踢原因
    /// </summary>
    public static class PlayerBeKickReasonEnum
    {
        public const int eBanPlay = 1; // 封号踢人
        public const int eRepeatLogin = 2; // 重复登录踢人
        public const int eGMKick = 3; // GM踢人
        public const int eServerShutdown = 4; // 服务器关闭
        public const int eLoginFailed = 5; // 登陆失败
        public const int eLoginServerError = 6; // 登录服务器错误
        public const int eServerHeavyLoad = 7; // 服务器高负载
        public const int eRename = 8; // 改名成功后踢人
        public const int eLockMac = 9; // 禁止mac
        public const int eGSError = 50; // GS报错踢人
        public const int eKSHandleCLMsgError = 51; // KS处理客户端消息错误
        public const int eKSDayRefreshError = 52; // KS每日刷新错误
        public const int eKSUpdateError = 53; // KSupdate错误
        public const int eToBridgeServer = 54; // 离开本服去跨服
        public const int eLeaveBridgeServer = 55; // 离开跨服回本服
        public const int eNoPlayerData = 56; // 没有player数据
        public const int eNoToken = 57; // 没有跨服token
        public const int eInsertPlayerError = 58; // 数据库新建player错误
        public const int eTooManyPlayer = 59; // 创建角色过多
        public const int eSelectPlayerError = 60; // 数据库查询player错误
        public const int ePlayerInBridge = 61; // 角色在跨服服务器里
        public const int eKSHandleGSMsgError = 62; // KS处理GS消息错误
        public const int eNormalServerKick = 63; // 普通服申请把在跨服的账号踢掉
        public const int eLoadDBDataError = 64;// 在DB线程查询playsystem错误
    }

    /// <summary>
    /// 玩家被踢原因msg映射
    /// </summary>
    public static readonly Dictionary<int, int> PlayerBeKickReasonMsg = new Dictionary<int, int>
    {
        {PlayerBeKickReasonEnum.eRepeatLogin, 9},
        {PlayerBeKickReasonEnum.eServerShutdown, 21},
    };


    /// <summary>
    /// 游戏流程
    /// </summary>
    public enum GameProcedureName
    {
        /// <summary>
        /// 预加载
        /// </summary>
        GamePreload,
        /// <summary>
        /// 登录
        /// </summary>
        GameLogin,
        /// <summary>
        /// 主场景
        /// </summary>
        GameMain,

        /// <summary>
        /// 比赛场景
        /// </summary>
        GameWorld,
    }

    /// <summary>
    /// 本地存储枚举
    /// </summary>
    public enum LocalStorage
    {
        /// <summary>
        /// 登录_账号
        /// </summary>
        Login_Account,
    }

    /// <summary>
    /// 相机模式枚举
    /// </summary>
    public enum CameraModleEnum
    {
        /// <summary>
        /// 常规模式
        /// </summary>
        Normal,
        /// <summary>
        /// 自动跟随模式
        /// </summary>
        Auto,
    }

    /// <summary>
    /// 地图类型
    /// </summary>
    public enum MapTypeEnum
    {
        Fake=0,
        /// <summary>
        /// 公共场景/主地图
        /// </summary>
        Common = 1,
        /// <summary>
        /// 比赛地图
        /// </summary>
        Competition = 2,
        /// <summary>
        /// pvp
        /// </summary>
        PVP = 3,
    }

    /// <summary>
    /// 比赛地图状态
    /// </summary>
    public enum CompetitionMapState
    {
        None = 0,
        /// <summary>
        /// 准备游戏
        /// </summary>
        ReadyGame = 1,
        /// <summary>
        /// 开始游戏
        /// </summary>
        StartGame = 2,
        /// <summary>
        /// 结束游戏
        /// </summary>
        EndGame = 3,
    }

    /// <summary>
    /// 角色类型枚举
    /// </summary>
    public enum CharacterTypeEnum
    {
        /// <summary>
        /// 主玩家角色
        /// </summary>
        MainCharacter,
        /// <summary>
        /// 网络玩家角色
        /// </summary>
        NetWorkCharacter,
        /// <summary>
        /// AI角色
        /// </summary>
        VehicleAI,
    }

    /// <summary>
    /// 动画参数类型枚举
    /// </summary>
    public enum AnimatorValueTypeEnum
    {
        Trigger,
        Float,
        Int,
        Bool,
    }

    /// <summary>
    /// 动画HashID映射Idx
    /// </summary>
    public static readonly Dictionary<int, int> AnimatorHashIdMappingIdx = new Dictionary<int, int>
    {
        {AnimatorHashIDs.SpeedFloat, 0},
        {AnimatorHashIDs.SteerFloat, 1},
        {AnimatorHashIDs.JumpTrigger, 2},
        {AnimatorHashIDs.VictoryTrigger, 3},
        {AnimatorHashIDs.DizzyBool, 4},
        {AnimatorHashIDs.SprintBool, 5},
        {AnimatorHashIDs.GroundBool, 6},
        {AnimatorHashIDs.HitTrigger, 7},
    };

    /// 动画Idx映射HashID
    /// </summary>
    public static readonly int[] AnimatorIdxMappingHashId = new int[]
    {
        AnimatorHashIDs.SpeedFloat,
        AnimatorHashIDs.SteerFloat,
        AnimatorHashIDs.JumpTrigger,
        AnimatorHashIDs.VictoryTrigger,
        AnimatorHashIDs.DizzyBool,
        AnimatorHashIDs.SprintBool,
        AnimatorHashIDs.GroundBool,
        AnimatorHashIDs.HitTrigger,
    };

    /// <summary>
    /// 服务器角色同步数据Idx
    /// </summary>
    public static class CharacterStateDataIdx
    {
        public const int RoleId = 0;
        public const int Transform = 1;
        public const int Velocity = 2;
        public const int Animator = 3;

        // 空间信息
        public const int TransformPosX = 0;
        public const int TransformPosY = 1;
        public const int TransformPosZ = 2;
        public const int TransformRotX = 3;
        public const int TransformRotY = 4;
        public const int TransformRotZ = 5;
        public const int TransformRotW = 6;

        // 速度信息
        public const int VelocityX = 0;
        public const int VelocityY = 1;
        public const int VelocityZ = 2;
    }

    /// <summary>
    /// 任务类型
    /// </summary>
    public enum TaskTypeEnum
    {
        /// <summary>
        /// 每日任务
        /// </summary>
        eDailyTask = 1,
        /// <summary>
        /// 一次性任务
        /// </summary>
        eOneTask = 2,
    }

    /// <summary>
    /// 任务事件
    /// </summary>
    public enum TaskEventEnum
    {
        /// <summary>
        /// 完成比赛
        /// </summary>
        eAccomplishGame = 1,
        /// <summary>
        /// 获得冠军
        /// </summary>
        eChampionship = 2,
        /// <summary>
        /// 在线时长
        /// </summary>
        eOnlineTime = 3,
        /// <summary>
        /// 累计登陆
        /// </summary>
        eLogin = 4,
        /// <summary>
        /// 累计邀请
        /// </summary>
        eInvite = 5,
        /// <summary>
        /// 体验赛场
        /// </summary>
        ePlayMap = 6,
        /// <summary>
        /// 完成指定类型任务
        /// </summary>
        eAccomplishTaskByType = 7,
        /// <summary>
        /// 完成指定事件任务
        /// </summary>
        eAccomplishTaskByEvent = 8,


        eTaskEvent1 = 101,
        eTaskEvent2 = 102,
        eTaskEvent3 = 103,
        eTaskEvent4 = 104,
        eTaskEvent5 = 105,
        eTaskEvent6 = 106,
        eTaskEvent7 = 107,
        eTaskEvent8 = 108,
        eTaskEvent9 = 109,
        eTaskEvent10 = 110,
        eTaskEvent11 = 111,
        eTaskEvent12 = 112,
        eTaskEvent13 = 113,
        eTaskEvent14 = 114,
    }

    /// <summary>
    /// 排行榜类型
    /// </summary>
    public static class ScoreRankType
    {
        public const int eScoreRank = 1;  // 积分榜
    }

    /// <summary>
    /// Buff类型
    /// </summary>
    public static class BuffType
    {
        public const int ForceBuff = 1;
        public const int SpeedUpBuff = 2;
        public const int VelocityBuff = 3;
    }

    /// <summary>
    /// Buff创建映射
    /// </summary>
    public static readonly Dictionary<int, Func<BaseBuff>> BuffTypeMappingIdx = new Dictionary<int, Func<BaseBuff>>
    {
        {BuffType.ForceBuff, delegate(){return new ForceBuff();}},
        {BuffType.SpeedUpBuff, delegate(){return new SpeedUpBuff();}},
        {BuffType.VelocityBuff, delegate(){return new VelocityBuff();}},
    };
    public static class PlayerSystemMapping
    {
        public const string BaseInfoSystem = nameof(BaseInfoSystem);
        public const string TaskSystem = nameof(TaskSystem);
        public const string TreasureChestSystem = nameof(TreasureChestSystem);
    }
    /// <summary>
    /// 宝箱来源枚举
    /// </summary>
    public enum TreasureChestSourceEnum
    {
        /// <summary>
        /// 任务
        /// </summary>
        eTask = 0,
        /// <summary>
        /// 购买
        /// </summary>
        eBuy = 1,
        /// <summary>
        /// 融合
        /// </summary>
        eFusion = 2,
    }


    /// <summary>
    /// 订单类型枚举
    /// </summary>
    public enum OrderTypeEnum
    {
        /// <summary>
        /// 宝箱订单
        /// </summary>
        eTreasure = 0,
    }
}
