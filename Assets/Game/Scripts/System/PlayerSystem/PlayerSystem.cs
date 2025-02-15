using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Framework
{
    public class PlayerSystem : BaseSystem<PlayerSystem>
    {
        private string m_sRoleId = "";
        private string m_sName = "";
        private int m_nCreateTime = 0;

        private int m_nLevel = 1;
        private double m_nExp = 0;
        private int m_nVipLv = 0;
        private long m_nGold = 0;
        private long m_nDiamond = 0;
        private int m_nEnergy = 0;
        private int m_nHead = 1;
        private int m_nPlayerCfgId = 1;
        private int m_nCarCfgId = 1;
        private int m_nScore = 0;
        public int CarLevel { get; private set; }
        private long m_uid;
        public long TokenScore;
        public string TokenScoreStr;
        public long ChangedToken;
        public int LoginNum;
        public int InviteCount;
        public long Monetary;
        public void SetUID(long value)
        {
            this.m_uid = value;
        }
        public long GetUID()
        {
            return this.m_uid;
        }
        public override void OnStart()
        {

        }
        /// <summary>
        /// 是否逻辑刷新
        /// </summary>
        /// <returns></returns>
        public override bool IsUpdate() { return true; }

        /// <summary>
        /// 逻辑刷新通知
        /// </summary>
        /// <param name="i_nDelay"></param>
        public override void OnUpdate(float i_nDelay)
        {

        }

        public override void OnRefresh_EveryDay()
        {
            base.OnRefresh_EveryDay();
        }


        public void SetRoleId(string value)
        {
            m_sRoleId = value;
        }
        public void SetNickName(string value)
        {
            m_sName = value;
        }
        /// <summary>
        /// 服务器同步玩家基本信息消息
        /// </summary>
        public void OnSyncBasicInfo(JArray i_tBasicInfo)
        {
            m_nLevel = (int)i_tBasicInfo[0];
            m_nExp = (double)i_tBasicInfo[1];
            m_nVipLv = (int)i_tBasicInfo[2];
            m_nGold = (long)i_tBasicInfo[3];
            m_nDiamond = (long)i_tBasicInfo[4];
            m_nEnergy = (int)i_tBasicInfo[5];
            m_nHead = (int)i_tBasicInfo[6];
            m_nPlayerCfgId = (int)i_tBasicInfo[7];
            m_nCarCfgId = (int)i_tBasicInfo[8];
            m_nScore = (int)i_tBasicInfo[9];
        }

        // ------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取角色Id
        /// </summary>
        public string GetRoleID() { return m_sRoleId; }

        /// <summary>
        /// 获取名字
        /// </summary>
        public string GetName() { return m_sName; }

        /// <summary>
        /// 获取玩家创建时间
        /// </summary>
        /// <returns></returns>
        public int GetCreateTime() { return m_nCreateTime; }

        // ------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取等级
        /// </summary>
        public int GetLevel() { return m_nLevel; }

        /// <summary>
        /// 服务器改变等级消息
        /// </summary>
        public void OnChangeLevel(int i_nNewLevel)
        {
            m_nLevel = i_nNewLevel;
        }

        // ------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取经验
        /// </summary>
        public double GetExp() { return m_nExp; }

        /// <summary>
        /// 服务器改变经验消息
        /// </summary>
        public void OnChangeExp(double i_nNewExp)
        {
            m_nExp = i_nNewExp;
        }

        // ------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取Vip等级
        /// </summary>
        public int GetVipLv() { return m_nVipLv; }

        /// <summary>
        /// 服务器改变Vip等级消息
        /// </summary>
        public void OnChangeVipLv(int i_nNewVipLv)
        {
            m_nVipLv = i_nNewVipLv;
        }

        // ------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取金币
        /// </summary>
        public long GetGold()
        {
            return m_nGold;
        }

        /// <summary>
        /// 服务器改变金币消息
        /// </summary>
        public void OnChangeGold(long i_nNewGold)
        {
            long nOld = m_nGold;
            m_nGold = i_nNewGold - GoldPoolValue;
            if (m_nGold < 1)
            {
                m_nGold = 0;
            }
            TextBindValue.SetValue(BindValueType.Gold, m_nGold);
            EventManager.Instance.Dispatch(EventDefine.Global.OnGoldChanged);
        }
        public int GoldPoolValue { get; private set; }
        public void AddGoldPool(int value)
        {
            GoldPoolValue += value;
        }
        public void MovePoolGoldToPlayer(int value)
        {
            if (value > 0 && GoldPoolValue > 0)
            {
                if (value > GoldPoolValue)
                {
                    value = GoldPoolValue;
                }
                m_nGold += value;
                GoldPoolValue -= value;
                TextBindValue.SetValue(BindValueType.Gold, m_nGold);
                EventManager.Instance.Dispatch(EventDefine.Global.OnGoldChanged);
            }
        }
        // ------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取钻石
        /// </summary>
        public long GetDiamond() { return m_nDiamond; }

        /// <summary>
        /// 服务器改变钻石消息
        /// </summary>
        public void OnChangeDiamond(long i_nDiamond)
        {
            long nOld = m_nDiamond;
            m_nDiamond = i_nDiamond;
            TextBindValue.SetValue(BindValueType.Diamond, m_nDiamond);
        }
        public void OnChangeTokenScore(long value)
        {
            TokenScore = value;
            TokenScoreStr = (value * 0.001f).ToString("#.###");
            TextBindValue.SetValue(BindValueType.KartMoney, TokenScore);
            EventManager.Instance.Dispatch(EventDefine.Global.OnGoldChanged);
        }
        public void SetChangedToken(long value)
        {
            ChangedToken = value;
            TextBindValue.SetValue(BindValueType.ChangedToken, ChangedToken);
        }
        // ------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取体力
        /// </summary>
        public int GetEnergy() { return m_nEnergy; }

        /// <summary>
        /// 服务器改变体力消息
        /// </summary>
        public void OnChangeEnergy(int i_nNewEnergy)
        {
            m_nEnergy = i_nNewEnergy;
            TextBindValue.SetValue(BindValueType.Energy, m_nEnergy);
        }

        // ------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取头像
        /// </summary>
        public int GetHead() { return m_nHead; }

        /// <summary>
        /// 设置头像
        /// </summary>
        public void SetHead(int i_nNewHead)
        {
            GameEntry.Net.Send(CtoS.K_ChangeHeadReq, i_nNewHead);
        }

        /// <summary>
        /// 服务器改变头像消息
        /// </summary>
        public void OnChangeHead(int i_nNewHead)
        {
            m_nHead = i_nNewHead;
            GameEntry.GUIEvent.DispatchEvent(GUIEvent.OnHeadChange);
        }

        // ------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取玩家外显Id
        /// </summary>
        public int GetPlayerCfgId() { return m_nPlayerCfgId; }

        /// <summary>
        /// 设置玩家外显Id
        /// </summary>
        public void SetPlayerCfgId(int i_nNewPlayerCfgId)
        {
            m_nPlayerCfgId = i_nNewPlayerCfgId;
            GameEntry.Net.Send(CtoS.K_ChangePlayerCfgIdReq, i_nNewPlayerCfgId);
        }

        /// <summary>
        /// 服务器改变玩家外显Id消息
        /// </summary>
        public void OnChangePlayerCfgId(int i_nNewPlayerCfgId)
        {
            m_nPlayerCfgId = i_nNewPlayerCfgId;
        }

        // ------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取赛车Id
        /// </summary>
        public int GetCarCfgId() { return m_nCarCfgId; }

        /// <summary>
        /// 设置赛车Id
        /// </summary>
        public void SetCarCfgId(int i_nNewCarCfgId)
        {
            m_nCarCfgId = i_nNewCarCfgId;
            GameEntry.Net.Send(CtoS.K_ChangeCarCfgIdReq, i_nNewCarCfgId);
        }

        /// <summary>
        /// 服务器改变赛车Id消息
        /// </summary>
        public void OnChangeCarCfgId(int i_nNewCarCfgId)
        {
            m_nCarCfgId = i_nNewCarCfgId;
        }

        // ------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取积分
        /// </summary>
        public int GetScore() { return m_nScore; }

        /// <summary>
        /// 服务器改变积分消息
        /// </summary>
        public void OnChangeScore(int i_nScore)
        {
            int nOld = m_nScore;
            m_nScore = i_nScore;
            EventManager.Instance.Dispatch(EventDefine.Global.OnChangeScore);

        }
    }
}