using System;
using System.Collections.Generic;
using Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Proto;
public partial class HttpHandler
{
    private ProtoManagerClient proto;
    public HttpHandler()
    {
        proto = new ProtoManagerClient();
    }
    public ProtoManagerClient GetProtoClient()
    {
        return proto;
    }
    public void GMAddMoney(int type, long value)
    {
        proto.GMAddMoney(type, value);
    }
    public void GMAddItem(int id, long count)
    {
        proto.GMAddItem(id, count);
    }
    public void Login(string name, string password)
    {
        //登陆
        proto.Login(name, password); return;
        /* var param = new ReqMsgBodyLoginPlayer()
        {
            account = name,
        };
        GameEntry.Http.Send("LoginPlayer", param); */
    }
    public void Register(string account, string mail, string code, string nickName, string inviteAccount)
    {
        proto.Register(account, mail, code, nickName, inviteAccount);
    }
    public void SetNickName(string nickName)
    {
        proto.SetNickName(nickName);
    }
    public void SetHeadId(int headId)
    {
        proto.SetHeadId(headId);
    }
    public void TaskReceiveAward(int id)
    {
        //任务领奖
        proto.TaskReceiveAward(id); return;

        /* var param = new ReqMsgBodyTaskReceiveAward()
        {
            taskCfgId = id
        };
        GameEntry.Http.Send("TaskReceiveAward", param); */
    }
    public void BuyTreasureChest(int id, int count)
    {
        //买宝箱
        proto.BuyTreasureChest(id, count); return;
        /* var req = new ReqMsgBodyBuyTreasureChest();
        req.treasureChestCfgId = id;
        req.buyNum = count;
        req.orderHash = "";
        GameEntry.Http.Send("BuyTreasureChest", req); */
    }
    public void MergeTreasureChest(List<int> ids)
    {
        //合并宝箱
        proto.MergeTreasureChest(ids); return;
        /* var req = new ReqMsgBodyFusionTreasureChest();
        if (ids.Count < 3)
        {
            return;
        }
        req.fusionTreasureChests = ids;
        GameEntry.Http.Send("FusionTreasureChest", req); */
    }
    public void UseTreasureChest(int id)
    {
        //开宝箱
        proto.UseTreasureChest(id); return;
        /* var req = new ReqMsgBodyOpenTreasureChest();
        req.instId = id;
        GameEntry.Http.Send("OpenTreasureChest", req); */
    }
    public void EnterMap(GameplayBase gameplay)
    {
        //进入地图
        proto.EnterMap(gameplay); return;
        /* var req = new ReqMsgBodyEnterCompetitionMap();
        req.mapCfgId = gameplay.TableData.Id;
        GameEntry.Http.Send("EnterCompetitionMap", req); */
    }
    public void FinishMap(GameplayBase gameplay, int rank, int time)
    {
        //完成地址
        proto.FinishMap(gameplay, rank, time); return;
        /* var req = new ReqMsgBodyCompleteCompetitionMap();
        req.mapCfgId = gameplay.TableData.Id;
        req.rank = rank;
        req.time = time;
        GameEntry.Http.Send("CompleteCompetitionMap", req); */
    }
    public void UploadOnlineTime(int minutes)
    {
        return;
        //更新在线时长
        proto.UploadOnlineTime(minutes); return;
        /* var req = new ReqMsgBodyOnlineTime();
        req.onlineTimeMinute = minutes;
        GameEntry.Http.Send("OnlineTime", req); */
    }
    public void GetSystemData(Proto.SystemEnum type)
    {
        proto.GetSystemData(type);
    }
    public void GetChangeDataSystemData()
    {
        proto.GetChangeDataSystemData();
    }
    public void UpgradeCar(int id)
    {
        proto.UpgradeCar(id);
    }
    public void UpgradeAllCarPart()
    {
        proto.UpgradeAllCarPart();
    }
    public void BuyAutoUpgrade()
    {
        proto.BuyAutoUpgrade();
    }
    public void BuyGoldSpeedup(int slot)
    {
        proto.BuyGoldSpeedup(slot);
    }
    public void OpenLuckyBox(int count)
    {
        proto.OpenLuckyBox(count);
    }
    public void StakeCar()
    {
        proto.StakeCar();
    }
    public void StakeCanCancel()
    {
        proto.StakeCanCancel();
    }
    public void StakeCarClaim()
    {
        proto.StakeCarClaim();
    }
    public void StakeDiamond(int addCount)
    {
        proto.StakeDiamond(addCount);
    }
    public void StakeDiamondCancel()
    {
        proto.StakeDiamondCancel();
    }
    public void StakeDiamondClaim()
    {
        proto.StakeDiamondClaim();
    }
    public void GetShopItemEnable(int tplId)
    {
        proto.GetShopItemEnable(tplId);
    }
    public void GetShopOrder(int tplid, int count, long amount, string buyType, int sid, string wallet)
    {
        proto.GetShopOrder(tplid, count, amount, buyType, sid, wallet);
    }
    public void CreateAeonOrder(int tplid, int count, string buyType)
    {
        proto.CreateAeonOrder(tplid, count, buyType);
    }
    public void ShopBuy(int tplId, string order, TransactionPlatformEnum transactionPlatformEnum, string cryptoEnum)
    {
        proto.ShopBuy(tplId, order, transactionPlatformEnum, cryptoEnum);
    }
    public void GetRank(RankTypeEnum type, int offset = 0, int count = 10)
    {
        proto.GetRank(type, offset, count);
    }
    public void BindWallet(string address, string walletName)
    {
        proto.BindWallet(address, walletName);
    }
    public void ChangeCode(string code)
    {
        proto.ChangeCode(code);
    }
    public void FinishShare()
    {
        proto.FinishShare();
    }
    public void FinishFollow()
    {
        proto.FinishFollow();
    }
    public void FinishFollowCEO()
    {
        proto.FinishFollowCEO();
    }
    public void BuyEnergy()
    {
        proto.BuyEnergy();
    }
    public void StartLuckyTurntable()
    {
        proto.StartLuckyTurntable();
    }
    public void EnterRoom(string RoomCode = "")
    {
        proto.EnterRoom(RoomCode);
    }
    public void SetRoleId(int value)
    {
        proto.SetRoleId(value);
    }
    public void SetCarId(int value)
    {
        proto.SetCarId(value);
    }
    public void TaskCompleteEvent(int id)
    {
        proto.TaskCompleteEvent(id);
    }
    public void CarUpgrade()
    {
        proto.UpgradeAllCarPart();
    }
    public void BuyPveCount()
    {
        proto.BuyPveCount();
    }
    public void ChangeToken(int type, string code, string email)
    {
        proto.ChangeToken(type, code, email);
    }
    public void ReceiveSeasonJourneyAward(int id)
    {
        proto.ReceiveSeasonJourneyAward(id);
    }
    public void SetImmutablePassportAddress(string value)
    {
        proto.SetImmutablePassportAddress(value);
    }
    public void GMAddGift(int id)
    {
        proto.GMAddGift(id);
    }
    public void GMAddRandomGift(int id)
    {
        proto.GMAddRandomGift(id);
    }
}

public partial class HttpHandler
{
    public void ResponseData(byte[] data)
    {
        if (data == null)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", 11);
            return;
        }
        proto.FromBytes(data);
    }
}