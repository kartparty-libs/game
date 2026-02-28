using System;
using System.Collections.Generic;
using Framework;
using Framework.Core;
using Google.Protobuf;
using Proto;
using UnityEngine;
public class ProtoManagerClient : ProtoManager
{
    public const int Version = 9;
    private List<IMessage> _messages;
    private MsgServerHeader Header;
    private string Account;
    private long UID;
    //网关服
    private string gateServer;
    //排行服
    private string rankServer;
    //游戏服
    private string gameServer;
    //比赛服
    private string matchServer;

    public ProtoManagerClient()
    {
        _messages = new List<IMessage>();
        Header = new MsgServerHeader();
        Header.Version = Version;
        Header.Hash = "";
        AddMapping<Proto.MsgServerHeader>();
        AddMapping<Proto.ReqMsgBodyBuyTreasureChest>();
        AddMapping<Proto.ReqMsgBodyLoginPlayer>();
        AddMapping<Proto.ReqMsgBodyCompleteCompetitionMap>();
        AddMapping<Proto.ReqMsgBodyCreateNewPlayer>();
        AddMapping<Proto.ReqMsgBodyEnterCompetitionMap>();
        AddMapping<Proto.ReqMsgBodyFusionTreasureChest>();
        AddMapping<Proto.ReqMsgBodyGetSystemData>();
        AddMapping<Proto.ReqMsgBodyGetChangeDataSystemData>();
        AddMapping<Proto.ReqMsgBodyOnlineTime>();
        AddMapping<Proto.ReqMsgBodyOpenTreasureChest>();
        AddMapping<Proto.ReqMsgBodyTaskReceiveAward>();

        AddMapping<Proto.ResMsgBodyCode>(Proto.ResMsgBodyCode.Parser.ParseFrom, OnMsgCodeInfo);
        AddMapping<Proto.ResMsgBodyPlayerLoginData>(Proto.ResMsgBodyPlayerLoginData.Parser.ParseFrom, OnServerInfo);
        AddMapping<Proto.MsgClientHeader>(Proto.MsgClientHeader.Parser.ParseFrom, OnHeader);
        AddMapping<Proto.ResMsgBodyPlayerData>(Proto.ResMsgBodyPlayerData.Parser.ParseFrom, OnPlayerData);
        AddMapping<Proto.ResMsgBodyTaskSystem>(Proto.ResMsgBodyTaskSystem.Parser.ParseFrom, OnTaskData);
        AddMapping<Proto.ResMsgBodyTreasureChestSystem>(Proto.ResMsgBodyTreasureChestSystem.Parser.ParseFrom, OnTreasureChestData);
        AddMapping<Proto.ResMsgBodyBaseInfo>(Proto.ResMsgBodyBaseInfo.Parser.ParseFrom, OnBaseInfo);
        AddMapping<Proto.ResMsgBodyCarCultivateSystem>(Proto.ResMsgBodyCarCultivateSystem.Parser.ParseFrom, OnCarCultivateSystem);
        AddMapping<Proto.ResMsgBodyLuckyBoxSystem>(Proto.ResMsgBodyLuckyBoxSystem.Parser.ParseFrom, OnLuckyBoxSystem);
        AddMapping<Proto.ResMsgBodyMiningSystem>(Proto.ResMsgBodyMiningSystem.Parser.ParseFrom, OnMiningSystem);
        AddMapping<Proto.ResMsgBodyIsBuyShowItem>(Proto.ResMsgBodyIsBuyShowItem.Parser.ParseFrom, OnCheckShopCanBuy);
        AddMapping<Proto.ResMsgBodyBuyShowItem>(Proto.ResMsgBodyBuyShowItem.Parser.ParseFrom, OnBuyShowItem);
        AddMapping<Proto.ResMsgBodyBuyOrderId>(Proto.ResMsgBodyBuyOrderId.Parser.ParseFrom, OnBuyOrderId);
        AddMapping<Proto.ResMsgBodyShopSystem>(Proto.ResMsgBodyShopSystem.Parser.ParseFrom, OnShopSystem);
        AddMapping<Proto.ResMsgBodyCommonRankManager>(Proto.ResMsgBodyCommonRankManager.Parser.ParseFrom, OnRankData);
        AddMapping<Proto.ResMsgBodyMapSystem>(Proto.ResMsgBodyMapSystem.Parser.ParseFrom, OnMapSystemData);
        AddMapping<Proto.ResMsgBodyBindWallet>(Proto.ResMsgBodyBindWallet.Parser.ParseFrom, OnBindWallet);
        AddMapping<Proto.ResMsgBodyClientUseGiftCode>(Proto.ResMsgBodyClientUseGiftCode.Parser.ParseFrom, OnChangeCodeSuccess);
        AddMapping<Proto.ResMsgBodyGiftCodeSystem>(Proto.ResMsgBodyGiftCodeSystem.Parser.ParseFrom, OnChangeCode);
        AddMapping<Proto.ResMsgBodyEnergySystem>(Proto.ResMsgBodyEnergySystem.Parser.ParseFrom, OnEnergySystem);
        AddMapping<Proto.ResMsgBodyBuyEnergy>(Proto.ResMsgBodyBuyEnergy.Parser.ParseFrom, OnBuyEnergy);
        AddMapping<Proto.ResMsgBodyExtendSystem>(Proto.ResMsgBodyExtendSystem.Parser.ParseFrom, OnExtendSystem);
        AddMapping<Proto.ResMsgBodyTwitterFollow>(Proto.ResMsgBodyTwitterFollow.Parser.ParseFrom, OnTwitterFollow);
        AddMapping<Proto.ResMsgBodyGiftSystem>(Proto.ResMsgBodyGiftSystem.Parser.ParseFrom, OnGiftSystem);
        AddMapping<Proto.ResMsgBodyLuckyTurntableSystem>(Proto.ResMsgBodyLuckyTurntableSystem.Parser.ParseFrom, OnLuckyTurntable);
        AddMapping<Proto.ResMsgBodyReadyEnterRoom>(Proto.ResMsgBodyReadyEnterRoom.Parser.ParseFrom, OnGetServerInfo);
        AddMapping<Proto.ResMsgBodyPlayerEnterRoom>(Proto.ResMsgBodyPlayerEnterRoom.Parser.ParseFrom, OnEnterRoom);
        AddMapping<Proto.ResMsgBodyRoomDataChange>(Proto.ResMsgBodyRoomDataChange.Parser.ParseFrom, OnRoomData);
        AddMapping<Proto.ResMsgBodyPlayerLeaveRoom>(Proto.ResMsgBodyPlayerLeaveRoom.Parser.ParseFrom, OnLeaveRoom);
        AddMapping<Proto.ResMsgBodyPlayerStateSync>(Proto.ResMsgBodyPlayerStateSync.Parser.ParseFrom, OnPlayerStateSync);
        AddMapping<Proto.ResMsgBodyPlayerCompleteGame>(Proto.ResMsgBodyPlayerCompleteGame.Parser.ParseFrom, OnPlayerCompleteGame);
        AddMapping<Proto.ResMsgBodyItemSystem>(Proto.ResMsgBodyItemSystem.Parser.ParseFrom, OnItemSystem);
        AddMapping<Proto.ResMsgBodySeasonSystem>(Proto.ResMsgBodySeasonSystem.Parser.ParseFrom, OnSeasonSystem);
        AddMapping<Proto.ResMsgBodyClientReceiveToken>(Proto.ResMsgBodyClientReceiveToken.Parser.ParseFrom, OnReceiveToken);
        AddMapping<Proto.ResMsgBodyInviteSystem>(Proto.ResMsgBodyInviteSystem.Parser.ParseFrom, OnInviteSystem);
        AddMapping<Proto.ResMsgBodyPing>(Proto.ResMsgBodyPing.Parser.ParseFrom, OnPing);
    }

    
    private void SendMessage(IMessage value, string server = null)
    {
        _messages.Clear();
        _messages.Add(Header);
        _messages.Add(value);
        Debug.Log("send:" + value.GetType().Name + "  " + value.ToString());
        try
        {
            var b = SendMessages(_messages);
            GameEntry.Http.Send(b, server);
        }
        catch (System.Exception)
        {
            Debug.LogError("send serror:" + value);
        }
    }
    public byte[] GetMessageBytes(IMessage value)
    {
        _messages.Clear();
        _messages.Add(Header);
        if (value != null)
        {
            _messages.Add(value);
        }
        try
        {
            return SendMessages(_messages);
        }
        catch (System.Exception)
        {
            Debug.LogError("send serror:" + value);
        }
        return null;
    }
    public void SendSocket(IMessage value)
    {
        _messages.Clear();
        _messages.Add(Header);
        _messages.Add(value);
        try
        {
            var b = SendMessages(_messages);
            GameEntry.Match.Send(b);
        }
        catch (System.Exception)
        {
            Debug.LogError("send serror:" + value);
        }
    }

    public void GMAddMoney(int type, long value)
    {
#if UNITY_EDITOR
        var req = new Proto.ReqMsgBodyGMAddMoney();
        req.Type = type;
        req.Value = value;
        SendMessage(req);
#endif
    }
    public void GMAddItem(int id, long count)
    {
#if UNITY_EDITOR
        var req = new ReqMsgBodyGMAddItem();
        req.Id = id;
        req.Value = count;
        SendMessage(req);
#endif
    }

    public void Login(string name, string password)
    {
        if (string.IsNullOrEmpty(gameServer))
        {
            var gate = new ReqMsgBodyGetPlayerLoginData();
            gate.Account = name;
            SendMessage(gate);
            return;
        }
        //登陆
        var req = new Proto.ReqMsgBodyLoginPlayer();
        req.Account = name;
        req.PlatformEnum = GameEntry.Context.Platform;
        req.PlatformInfo = GameEntry.Context.PlatformInfo;
        req.InviteAccount = GameEntry.Context.InviteAccount;
        SendMessage(req);
    }
    public void Register(string account, string mail, string code, string nickName, string inviteAccount)
    {
        var req = new ReqMsgBodyCreateNewPlayer();
        req.Account = account;
        req.Name = nickName;
        req.Email = mail;
        req.KartKey = code;
        req.PlatformEnum = GameEntry.Context.Platform;
        req.PlatformInfo = GameEntry.Context.PlatformInfo;
        req.InviteAccount = inviteAccount;
        SendMessage(req);
    }

    public void SetNickName(string nickName)
    {
        var req = new Proto.ReqMsgBodySetName();
        req.Name = nickName;
        SendMessage(req);
    }
    public void SetHeadId(int headId)
    {
        var req = new Proto.ReqMsgBodySetHeadId();
        req.HeadId = headId;
        SendMessage(req);
    }
    public void TaskReceiveAward(int id)
    {
        var req = new Proto.ReqMsgBodyTaskReceiveAward();
        req.TaskCfgId = id;
        SendMessage(req);
    }
    public void BuyTreasureChest(int id, int count)
    {
        var req = new Proto.ReqMsgBodyBuyTreasureChest();
        req.OrderHash = "";
        req.BuyNum = count;
        req.TreasureChestCfgId = id;
        SendMessage(req);

    }
    public void MergeTreasureChest(List<int> ids)
    {
        var req = new Proto.ReqMsgBodyFusionTreasureChest();
        req.FusionTreasureChests.AddRange(ids);
        SendMessage(req);
    }
    public void UseTreasureChest(int id)
    {
        var req = new Proto.ReqMsgBodyOpenTreasureChest();
        req.InstId = id;
        SendMessage(req);
    }
    public void EnterMap(GameplayBase gameplay)
    {
        var req = new Proto.ReqMsgBodyEnterCompetitionMap();
        req.MapCfgId = gameplay.TableData.Id;
        SendMessage(req);
    }
    public void FinishMap(GameplayBase gameplay, int rank, int time)
    {
        //完成地址
        var req = new Proto.ReqMsgBodyCompleteCompetitionMap();
        req.MapCfgId = gameplay.TableData.Id;
        req.Rank = rank;
        req.Time = time;
        SendMessage(req);
    }
    public void UploadOnlineTime(int minutes)
    {
        //更新在线时长
        // GameEntry.Http.Handler.TriggerTaskEventAddValue(new List<TaskEventEnum>() { TaskEventEnum.eOnlineTime }, new List<int>() { minutes });
        var req = new Proto.ReqMsgBodyOnlineTime();
        req.OnlineTimeMinute = minutes;
        SendMessage(req);
    }
    public void GetSystemData(Proto.SystemEnum type)
    {
        var req = new Proto.ReqMsgBodyGetSystemData();
        req.SystemEnum = type;
        SendMessage(req);
    }
    public void GetChangeDataSystemData()
    {
        var req = new Proto.ReqMsgBodyGetChangeDataSystemData();
        SendMessage(req);
    }
    public void UpgradeCar(int id)
    {
        var req = new ReqMsgBodyUpgradeCar();
        req.CarModuleTypeEnum = (CarModuleTypeEnum)id;
        SendMessage(req);
    }
    public void UpgradeAllCarPart()
    {
        var req = new ReqMsgBodyUpgradeCars();
        SendMessage(req);
    }
    public void BuyAutoUpgrade()
    {
        var req = new ReqMsgBodyBuyAutoUpgrade();
        SendMessage(req);
    }
    public void BuyGoldSpeedup(int slot)
    {
        var req = new ReqMsgBodyProductGoldSpeedup();
        req.Type = slot;
        SendMessage(req);
    }
    public void OpenLuckyBox(int count)
    {
        var req = new ReqMsgBodyOpenLuckyBox();
        req.OpenCount = count;
        SendMessage(req);
    }
    public void StakeCar()
    {
        SendMessage(new ReqMsgBodyChangeCarPledge());
    }
    public void StakeCanCancel()
    {
        SendMessage(new ReqMsgBodyCancelCarPledge());
    }
    public void StakeCarClaim()
    {
        SendMessage(new ReqMsgBodyReceiveCarTokenScore());
    }
    public void StakeDiamond(int addCount)
    {
        addCount = Mathf.Max(addCount, 0);
        if (addCount < 1)
        {
            return;
        }
        var req = new ReqMsgBodyAddDiamondPledge();
        req.AddCount = addCount;
        SendMessage(req);
    }
    public void StakeDiamondCancel()
    {
        SendMessage(new ReqMsgBodyCancelDiamondPledge());
    }
    public void StakeDiamondClaim()
    {
        SendMessage(new ReqMsgBodyReceiveDiamondTokenScore());
    }
    public void GetShopItemEnable(int tplId)
    {
        var req = new ReqMsgBodyIsBuyShowItem();
        req.ShopCfgId = tplId;
        SendMessage(req);
    }
    public void GetShopOrder(int tplid, int count, long amount, string buyType, int sid, string wallet)
    {
        var req = new ReqMsgBodyBuyOrderId();
        req.ShopCfgId = tplid;
        req.Count = count;
        req.Amount = amount;
        req.CryptoCoinType = buyType;
        req.SId = sid;
        SendMessage(req);
    }
    public void CreateAeonOrder(int tplid, int count, string buyType)
    {
        var req = new ReqMsgBodyCreateAeonOrder();
        req.ShopCfgId = tplid;
        req.Count = count;
        req.CryptoCoinType = buyType;
        SendMessage(req);
    }
    public void ShopBuy(int tplId, string orderId, TransactionPlatformEnum transactionPlatformEnum, string cryptoEnum)
    {
        var req = new ReqMsgBodyBuyShowItem();
        req.ShopCfgId = tplId;
        req.OrderHash = orderId;
        req.TransactionPlatformEnum = transactionPlatformEnum;
        req.CryptoCoinType = cryptoEnum;
        SendMessage(req);
    }
    public void GetRank(RankTypeEnum type, int offset = 0, int count = 10)
    {
        if (UID < 1)
        {
            return;
        }
        var req = new ReqMsgBodyGetRankData();
        req.IsBackup = false;
        req.RankTypeEnum = type;
        offset = Mathf.Max(0, offset);
        req.RankStartIndex = offset;
        req.RankEndIndex = offset + count - 1;
        req.RankEndIndex = Mathf.Max(req.RankStartIndex, req.RankEndIndex);
        req.SelfKey = UID;
        SendMessage(req, rankServer);
    }
    public void BindWallet(string address, string walletName)
    {
        var req = new ReqMsgBodyBindWallet();
        req.Account = Account;
        req.WalletHash = address;
        req.WalletName = walletName;
        SendMessage(req, gateServer);


        if (walletName == "OKX Wallet")
        {
            SendMessage(new ReqMsgBodyBindWalletTask());
        }
    }
    public void ChangeCode(string code)
    {
        var req = new ReqMsgBodyClientUseGiftCode();
        req.Code = code;
        req.PlatformEnum = GameEntry.Context.Platform;
        SendMessage(req);
    }
    public void FinishShare()
    {
        SendMessage(new ReqMsgBodyShare());
    }
    public void FinishFollow()
    {
        SendMessage(new ReqMsgBodyTwitterFollowKP());
    }
    public void FinishFollowCEO()
    {
        SendMessage(new ReqMsgBodyTwitterFollowCEO());
    }
    public void BuyEnergy()
    {
        SendMessage(new ReqMsgBodyBuyEnergy());
    }
    public void StartLuckyTurntable()
    {
        SendMessage(new ReqMsgBodyExecuteLuckyTurntable());
    }
    public void SetRoleId(int value)
    {
        var req = new ReqMsgBodySetRoleCfgId();
        req.RoleCfgId = value;
        SendMessage(req);
    }
    public void SetCarId(int value)
    {
        var req = new ReqMsgBodySetCarCfgId();
        req.CarCfgId = value;
        SendMessage(req);
    }
    public void EnterRoom(string RoomCode = "")
    {
        var req = new ReqMsgBodyReadyEnterRoom();
        if (RoomCode == null)
        {
            RoomCode = "";
        }
        req.RoomCode = RoomCode;
        SendMessage(req);
    }
    public void TaskCompleteEvent(int id)
    {
        var req = new ReqMsgBodyTaskEvent();
        req.TaskCfgId = id;
        SendMessage(req);
    }
    public void BuyPveCount()
    {
        var req = new ReqMsgBodyBuyPveItem();
        SendMessage(req);
    }
    public void ChangeToken(int type, string code, string email)
    {
        var req = new ReqMsgBodyClientReceiveToken();
        req.Type = type;
        req.Account = code;
        req.EMail = email;
        SendMessage(req);
    }
    public void ReceiveSeasonJourneyAward(int id)
    {
        var req = new ReqMsgBodyReceiveSeasonJourneyAward();
        req.SeasonJourneyCfgId = id;
        SendMessage(req);
    }
    public void SetImmutablePassportAddress(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }
        var req = new ReqMsgBodySetNFTWallet();
        req.NFTWallet = value;
        SendMessage(req);
    }
    public void GMAddGift(int id)
    {
#if UNITY_EDITOR
        var req = new ReqMsgBodyGMAddGift();
        req.Id = id;
        SendMessage(req);
#endif
    }
    public void GMAddRandomGift(int id)
    {
#if UNITY_EDITOR
        var req = new ReqMsgBodyGMAddRandomGift();
        req.Id = id;
        SendMessage(req);
#endif
    }
#if UNITY_EDITOR
    public void TestInvite(int serverId,string account)
    {
        var req = new ReqMsgBodyGMAddInvitePlayer();
        req.InviteAccountServerId = serverId;
        req.OwnerAccount = this.Account;
        req.InviteAccount = account;
        SendMessage(req);
    }
#endif

    private void OnServerInfo(IMessage message)
    {
        if (message is ResMsgBodyPlayerLoginData res)
        {
            if (string.IsNullOrEmpty(res.ServerUrl))
            {
                return;
            }
            gameServer = res.ServerUrl;
            gateServer = GameEntry.Http.serverUrl;
            if (GameEntry.Context.DebugMode)
            {
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", "2.OnServerInfo " + gameServer);
            }
            GameEntry.Http.SetGameServer(res.ServerId, gameServer);
            Login(res.Account, "");
        }
    }

    private void OnBaseInfo(IMessage message)
    {
        if (message is ResMsgBodyBaseInfo res)
        {
            PlayerSystem.Instance.OnChangeHead(res.HeadId);
            PlayerSystem.Instance.OnChangeGold(res.Gold);
            PlayerSystem.Instance.OnChangeDiamond(res.Diamond);
            PlayerSystem.Instance.OnChangeTokenScore(res.TokenScore);
            PlayerSystem.Instance.SetChangedToken(res.TotalReceiveToken);
            PlayerSystem.Instance.Monetary = res.Monetary;
            ItemSystem.Instance.Set203ItemCount(res.NFTCount);

        }
    }
    private void OnHeader(IMessage message)
    {
        if (message is Proto.MsgClientHeader res)
        {
            GameEntry.OfflineManager.SetServerTime(res.Date);
            // UnityEngine.Debug.Log("OnHeader " + res.Code);
            if (res.Code == ResponseCodeEnum.AccountNotRegistered)
            {
                //创建账号
                GameEntry.Context.NeedRegister = true;
                // GameEntry.Context.EnterMain = true;

                var e = ReferencePool.Get<GUIEvent>();
                e.IntValue = (int)res.Code;
                e.EventType = GUIEvent.NeedRegister;
                GameEntry.GUIEvent.DispatchEvent(e);

            }
            else if (res.Code == ResponseCodeEnum.NotOpenServer)
            {

            }
            else if (res.Code == ResponseCodeEnum.NewVersion)
            {
                var vData = new ConfirmData();
                vData.Title = GameEntry.Table.Lang.GetText(1539);
                vData.Content = GameEntry.Table.Lang.GetText(1540);
                vData.OkText = GameEntry.Table.Lang.GetText(1524);
                vData.Ok = () =>
                {
                    JumpUtils.Reload();

                };
                GameEntry.GUI.Open(GameEntry.GUIPath.ConfirmUI.Path, "data", vData);
            }
            else
            {
                if (res.Code != Proto.ResponseCodeEnum.Succeed)
                {
                    UnityEngine.Debug.LogError("error code " + res.Code);
                    GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", (int)res.Code);
                    var e = ReferencePool.Get<GUIEvent>();
                    e.IntValue = (int)res.Code;
                    e.EventType = GUIEvent.OnErrorCode;
                    GameEntry.GUIEvent.DispatchEvent(e);
                }
            }
        }
    }
    private void OnPlayerData(IMessage message)
    {
        if (message is Proto.ResMsgBodyPlayerData res)
        {
            Account = res.Account;
            UID = res.RoleId;
            rankServer = res.RankPostUrl;
            Header.RoleId = res.RoleId;
            GameEntry.Context.EnterMain = true;
            GameEntry.Context.NeedRegister = false;
            GameEntry.Context.Account = res.Account;
            GameEntry.Http.SetAccountToken(res.Account, res.Hash);
            PlayerSystem.Instance.SetNickName(res.Name);
            PlayerSystem.Instance.SetUID(res.RoleId);
            Debug.LogError(res.RoleId);
            PlayerSystem.Instance.LoginNum = res.LoginNum;
            PlayerSystem.Instance.InviteCount = res.InviteCount;
            bool isFirst = false;
            if (string.IsNullOrEmpty(Header.Hash))
            {
                isFirst = true;
                Header.Hash = res.Hash;
            }
            if (isFirst)
            {
                GameEntry.OfflineManager.LoginSuccess(res.Account);
                JavascriptBridge.Call(JavascriptBridge.UnityPlayerAccount, res.Account);
                if (GameEntry.Context.DebugMode)
                {
                    GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", "3.OnPlayerData");
                }
                GameEntry.Http.Handler.SetImmutablePassportAddress(GameEntry.Context.ImmutablePassportAddress);
            }
            else
            {
                GameEntry.GUIEvent.DispatchEvent(GUIEvent.OnPlayerInfoChanged);
            }

        }
    }
    private void OnTaskData(IMessage message)
    {
        if (message is Proto.ResMsgBodyTaskSystem res)
        {
            TaskSystem.Instance.Clear();
            foreach (var item in res.TaskDatas)
            {
                var data = new TaskData();
                data.taskCfgId = item.TaskCfgId;
                data.taskValue = item.TaskValue;
                data.isReceiveAward = item.IsReceiveAward;
                TaskSystem.Instance.UpdateTaskData(data);
            }
            TaskSystem.Instance.Update();

        }
    }
    private void OnTreasureChestData(IMessage message)
    {
        if (message is Proto.ResMsgBodyTreasureChestSystem res)
        {
            TreasureChestSystem.Instance.Clear();
            foreach (var item in res.TreasureChests)
            {
                TreasureChestData d = new TreasureChestData();
                d.orderHash = item.OrderHash;
                d.cfgId = item.CfgId;
                d.isVerified = item.IsVerified;
                d.instId = item.InstId;
                d.treasureChestSourceEnum = (int)item.TreasureChestSource;
                TreasureChestSystem.Instance.AddTreasure(d);
            }
            EventManager.Instance.Dispatch(EventDefine.Global.OnTreasureChestChanged);
        }
    }
    private void OnCarCultivateSystem(IMessage message)
    {
        if (message is ResMsgBodyCarCultivateSystem res)
        {
            CarCultivateSystem.Instance.SetData(res);
        }
    }
    private void OnLuckyBoxSystem(IMessage message)
    {
        if (message is ResMsgBodyLuckyBoxSystem res)
        {
            LuckBoxSystem.Instance.SetData(res);

        }
    }
    private void OnMiningSystem(IMessage message)
    {
        if (message is ResMsgBodyMiningSystem res)
        {
            // Debug.LogError(res.ToString());
            MiningSystem.Instance.SetData(res);
        }
    }
    private void OnShopSystem(IMessage message)
    {
        if (message is ResMsgBodyShopSystem res)
        {
            ShopSystem.Instance.SetData(res);
        }
    }
    private void OnCheckShopCanBuy(IMessage message)
    {
        if (message is ResMsgBodyIsBuyShowItem res)
        {
            var e = ReferencePool.Get<GUIEvent>();
            e.UserData = res;
            e.IntValue = res.ShopCfgId;
            e.Count = res.IsCanBuy ? 100 : 0;
            e.EventType = GUIEvent.OnShopBuyItem;
            GameEntry.GUIEvent.DispatchEvent(e);
            /* if (res.IsCanBuy)
            {
                //测试购买
                ShopBuy(res.ShopCfgId, "GMKart");
            } */
        }
    }
    private void OnBuyShowItem(IMessage message)
    {
        if (message is ResMsgBodyBuyShowItem res)
        {
            ShopSystem.Instance.BuySuccess(res);

        }
    }
    private void OnBuyOrderId(IMessage message)
    {
        if (message is ResMsgBodyBuyOrderId res)
        {
            ShopSystem.Instance.SetOrderId(res);
        }
    }
    private void OnRankData(IMessage message)
    {
        if (message is ResMsgBodyCommonRankManager res)
        {
            RankSystem.Instance.OnRankData(res);
        }
    }
    private void OnMapSystemData(IMessage message)
    {
        if (message is ResMsgBodyMapSystem res)
        {
            MapSystem.Instance.SetData(res);
        }
        // ResMsgBodyMapSystem;
    }
    private void OnMsgCodeInfo(IMessage message)
    {
        if (message is ResMsgBodyCode res)
        {
            var now = GameEntry.OfflineManager.GetServerDateTime();
            var openTime = TimeUtils.GetDateTimeByMilliseconds(res.OpenServerTime);
            var offset = TimeUtils.GetDiff(now, openTime);
            if (res.OpenServerTime > 0 && offset.TotalSeconds > 0)
            {
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", GameEntry.Table.Lang.Get(21, TimeUtils.GetRemainder(offset)));
            }
            else
            {
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", GameEntry.Table.Lang.GetText(22));
            }
        }
    }
    private void OnBindWallet(IMessage message)
    {
        if (message is ResMsgBodyBindWallet res)
        {
            if (res.IsBindSucceed)
            {

            }
        }
    }
    private void OnChangeCodeSuccess(IMessage message)
    {
        GameEntry.GUI.Close(GameEntry.GUIPath.ChangeCodeUI.Path);
        GameEntry.Timer.Start(3f, () =>
        {
            GetChangeDataSystemData();
        }, 1);
    }

    private void OnChangeCode(IMessage message)
    {
        if (message is ResMsgBodyGiftCodeSystem res)
        {
            if (res.UseGiftCodes.Count > 0)
            {
                bool hasError = false;
                foreach (var item in res.UseGiftCodes)
                {
                    // if(item.GiftType)
                    if (!item.IsSucceed)
                    {
                        hasError = true;
                        // 1：没有该兑换码  2：兑换码已被兑换
                        if (item.Code == 1)
                        {
                            GameEntry.GUI.Open(GameEntry.GUIPath.MsgBoxTipUI.Path, "error", 1529);
                        }
                        else if (item.Code == 2)
                        {
                            GameEntry.GUI.Open(GameEntry.GUIPath.MsgBoxTipUI.Path, "error", 1530);
                        }
                    }
                    else
                    {
                        ShopSystem.Instance.AddItemFromChangeCode(item.GiftId);
                    }
                }
                var e = ReferencePool.Get<GUIEvent>();
                e.StringValues.Clear();
                e.EventType = GUIEvent.OnChangeCodeResult;
                e.IntValue = hasError ? 1 : 0;
                GameEntry.GUIEvent.DispatchEvent(e);
            }
        }
    }
    private void OnEnergySystem(IMessage message)
    {
        if (message is ResMsgBodyEnergySystem res)
        {
            EnergySystem.Instance.SetData(res);
        }
    }
    private void OnBuyEnergy(IMessage message)
    {
        if (message is ResMsgBodyBuyEnergy res)
        {
            EnergySystem.Instance.BuyResult(res);
        }
    }
    private void OnExtendSystem(IMessage message)
    {
        if (message is ResMsgBodyExtendSystem res)
        {
            ExtendSystem.Instance.SetData(res);
        }
    }
    private void OnTwitterFollow(IMessage message)
    {
        if (message is ResMsgBodyTwitterFollow res)
        {
            if (!res.IsBind)
            {
                if (!string.IsNullOrEmpty(res.BindTwitterUrl))
                {
#if UNITY_EDITOR
                    Application.OpenURL(res.BindTwitterUrl);
#else
                    JavascriptBridge.Call(JavascriptBridge.UnityOpenLink, res.BindTwitterUrl);
#endif
                }
            }
            else
            {
                // if (res.FollowType == 1)
                // {
                //     TaskSystem.Instance.RequestJump7();
                // }
                // else if (res.FollowType == 2)
                // {
                //     TaskSystem.Instance.RequestJump8();
                // }
            }
        }
    }
    private void OnGiftSystem(IMessage message)
    {
        if (message is ResMsgBodyGiftSystem res)
        {
            GiftSystem.Instance.SetData(res);
        }
    }
    private void OnLuckyTurntable(IMessage message)
    {
        if (message is ResMsgBodyLuckyTurntableSystem res)
        {
            LuckyTurntableSystem.Instance.SetData(res);
        }
    }
    private void OnGetServerInfo(IMessage message)
    {
        if (message is ResMsgBodyReadyEnterRoom res)
        {
            GameEntry.Match.SetAddress(res.WsUrl);
        }
    }
    private void OnEnterRoom(IMessage message)
    {
        if (message is ResMsgBodyPlayerEnterRoom res)
        {
            MatchSystem.Instance.EnterRoom(res);
        }
    }
    private void OnRoomData(IMessage message)
    {
        if (message is ResMsgBodyRoomDataChange res)
        {
            MatchSystem.Instance.UpdateRoomInfo(res);
        }
    }
    private void OnLeaveRoom(IMessage message)
    {
        if (message is ResMsgBodyPlayerLeaveRoom res)
        {
            GameEntry.GUI.Close(GameEntry.GUIPath.MatchUI.Path);
        }
    }
    private void OnPing(IMessage message)
    {
        if (message is ResMsgBodyPing)
        {
            GameEntry.Match.Handler.Pang();
        }
    }

    private void OnPlayerStateSync(IMessage message)
    {
        if (message is ResMsgBodyPlayerStateSync res)
        {
            CharacterManager.Instance.OnPlayerStateSync(res);
        }
    }
    private void OnPlayerCompleteGame(IMessage message)
    {
        if (message is ResMsgBodyPlayerCompleteGame res)
        {
            MatchSystem.Instance.OnPlayerFinishSync(res);
        }
    }
    private void OnItemSystem(IMessage message)
    {
        if (message is ResMsgBodyItemSystem res)
        {
            ItemSystem.Instance.SetData(res);
        }
    }
    private void OnSeasonSystem(IMessage message)
    {
        if (message is ResMsgBodySeasonSystem res)
        {
            SeasonSystem.Instance.SetData(res);
        }
    }

    private void OnReceiveToken(IMessage message)
    {
        if (message is ResMsgBodyClientReceiveToken res)
        {
            ChangeKSystem.Instance.SetData(res);
        }
    }

    private void OnInviteSystem(IMessage message)
    {
        if(message is ResMsgBodyInviteSystem res){
            InviteSystem.Instance.SetData(res);
        }
    }



}