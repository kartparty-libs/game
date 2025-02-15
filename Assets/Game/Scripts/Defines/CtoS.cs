
/// <summary>
/// 客户端发送至服务器的消息需要在此注册
/// </summary>
public static class CtoS
{
    // ------------------------------------------------------------------------------------------------------
    // 登录系统

    // 进入KS请求
    public const string K_EnterKSReqMsg = "K_EnterKSReqMsg";

    // 请求玩家创建
    public const string K_PlayerCreate = "K_PlayerCreate";

    // 请求创建新角色消息
    public const string K_CreateCharReqMsg = "K_CreateCharReqMsg";

    // 离开KS请求
    public const string K_LeaveKSReqMsg = "K_LeaveKSReqMsg";

    // 心跳
    public const string K_HeartBeat = "K_HeartBeat";

    // ------------------------------------------------------------------------------------------------------
    // 角色系统

    // 修改头像
    public const string K_ChangeHeadReq = "K_ChangeHeadReq";

    // 修改玩家外显Id
    public const string K_ChangePlayerCfgIdReq = "K_ChangePlayerCfgIdReq";

    // 修改赛车Id
    public const string K_ChangeCarCfgIdReq = "K_ChangeCarCfgIdReq";

    // ------------------------------------------------------------------------------------------------------
    // 匹配系统

    // 开始匹配比赛
    public const string K_StartMateCompe = "K_StartMateCompe";

    // 退出匹配比赛
    public const string K_EndMateCompe = "K_EndMateCompe";

    // 测试添加机器人
    public const string K_TestAddRobot = "K_TestAddRobot";

    // 玩家准备通知
    public const string K_PlayerReadyMateCompe = "K_PlayerReadyMateCompe";

    //房主开始游戏通知
    public const string K_RoomOwnerStartMateCompe = "K_RoomOwnerStartMateCompe";

    // ------------------------------------------------------------------------------------------------------
    // 比赛地图

    // 加载地图完成
    public const string K_PlayerLoadMapCompleteReq = "K_PlayerLoadMapCompleteReq";

    // 玩家状态同步
    public const string K_PlayerStateInfoReq = "K_PlayerStateInfoReq";

    // 玩家完成比赛同步
    public const string K_PlayerCompleteGame = "K_PlayerCompleteGame";

    // 玩家离开地图
    public const string K_PlayerLeaveMapReq = "K_PlayerLeaveMapReq";

    // 玩添加buff
    public const string K_PlayerAddBuffReq = "K_PlayerAddBuffReq";

    // ------------------------------------------------------------------------------------------------------
    // 任务系统

    // 请求领取任务奖励
    public const string K_ReceiveTaskAwardReq = "K_ReceiveTaskAwardReq";

    // ------------------------------------------------------------------------------------------------------
    // 排行榜系统

    // 请求玩家自己的排名数据
    public const string K_ReqSelfRankData = "K_ReqSelfRankData";

    // 请求本服排行数据
    public const string K_RankDataReq = "K_RankDataReq";

    // ------------------------------------------------------------------------------------------------------
    // 邀请系统
    
    // 请求邀请码数据列表
    public const string K_GetBoundInviteCodeReq = "K_GetBoundInviteCodeReq";

    // 请求绑定邀请码
    public const string K_BindInviteCodeReq = "K_BindInviteCodeReq";

    // ------------------------------------------------------------------------------------------------------
    // 测试

    // 请求测试时间段排行榜快照
    public const string K_ReqBufferRankTest = "K_ReqBufferRankTest";

    // 请求测试时间段执行机器人
    public const string K_ReqTastExecuteRobot = "K_ReqTastExecuteRobot";

}