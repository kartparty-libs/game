
namespace Framework
{
    /// <summary>
    /// 事件定义
    /// </summary>
    public class EventDefine
    {
        /// <summary>
        /// 全局事件
        /// </summary>
        public static class Global
        {
            /// <summary>
            /// 数量
            /// </summary>
            public static int Count = 0;

            // ----------------------------------------------------------------------------------------
            // 框架端事件定义

            /// <summary>
            /// 应用退出通知
            /// </summary>
            public readonly static int OnApplicationQuit = Count++;

            /// <summary>
            /// 应用切换后台暂停通知
            /// </summary>
            public readonly static int OnApplicationPause = Count++;

            /// <summary>
            /// 配置表加载完成通知
            /// </summary>
            public readonly static int OnConfigLoaded = Count++;

            /// <summary>
            /// 玩家登录通知
            /// </summary>
            public readonly static int OnPlayerLogin = Count++;

            /// <summary>
            /// 玩家信息同步通知
            /// </summary>
            public readonly static int OnPlayerDataSyn = Count++;

            /// <summary>
            /// 玩家下线通知
            /// </summary>
            public readonly static int OnPlayerLogoff = Count++;

            // ----------------------------------------------------------------------------------------
            // 内容端事件定义

            /// <summary>
            /// 连接成功通知
            /// </summary>
            public readonly static int OnConnectSuc = Count++;

            /// <summary>
            /// 连接失败通知
            /// </summary>
            public readonly static int OnConnectFail = Count++;

            /// <summary>
            /// 连接关闭通知
            /// </summary>
            public readonly static int OnConnectClose = Count++;

            /// <summary>
            /// 连接断开通知
            /// </summary>
            public readonly static int OnDisconnect = Count++;

            // ----------------------------------------------------------------------------------------
            // 比赛地图事件定义

            /// <summary>
            /// 比赛地图同步地图状态通知
            /// </summary>
            public readonly static int OnCompetitionGameState = Count++;

            /// <summary>
            /// 同步玩家完成比赛信息通知
            /// </summary>
            public readonly static int OnCompleteGamePlayerInfo = Count++;

            // ----------------------------------------------------------------------------------------
            // 玩家系统事件定义

            /// <summary>
            /// 改变头像通知
            /// </summary>
            public readonly static int OnChangeHead = Count++;

            /// <summary>
            /// 改变玩家外显Id通知
            /// </summary>
            public readonly static int OnChangePlayerCfgId = Count++;

            /// <summary>
            /// 改变赛车Id通知
            /// </summary>
            public readonly static int OnChangeCarCfgId = Count++;

            // ----------------------------------------------------------------------------------------
            // 任务系统事件定义

            /// <summary>
            /// 任务数据改变通知
            /// </summary>
            public readonly static int OnChangeTaskData = Count++;

            /// <summary>
            /// 积分改变
            /// </summary>
            public readonly static int OnChangeScore = Count++;

            // ----------------------------------------------------------------------------------------
            // 排行榜系统事件定义

            /// <summary>
            /// 排行榜改变
            /// </summary>
            public readonly static int OnRankChange = Count++;
            /// <summary>
            /// 道具信息改变
            /// </summary> <summary>
            /// 
            /// </summary>
            public readonly static int OnTreasureChestChanged = Count++;
            /// <summary>
            /// 玩家金币改变
            /// </summary>

            public readonly static int OnGoldChanged = Count++;
            //车升级
            public readonly static int OnCarCultivateChanged = Count++;
            //挖矿
            public readonly static int OnMiningChanged = Count++;
            //商店
            public readonly static int OnShopChanged = Count++;
            //体力
            public readonly static int OnEnergyChanged = Count++;
            //转盘
            public readonly static int OnLuckyTurntableSystem = Count++;
            //转盘结果
            public readonly static int OnLuckyTurntableShowSystem = Count++;
            public readonly static int OnMatchServerConnected = Count++;
            public readonly static int OnMatchServerDisConnected = Count++;
            public readonly static int OnItemCountChanged = Count++;
            public readonly static int OnSeasonSystemChanged = Count++;
        }
    }
}
