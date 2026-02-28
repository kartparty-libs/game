
using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class StakeUI : UIWindowBase
{
    private int levelLimit;
    private int minMoney;
    private int addMoneyValue;
    private bool hasFinish;
    protected override void OnAwake()
    {
        base.OnAwake();
        levelLimit = 800;
        /*
        var table = GameEntry.Table.CarUpgrade;
        var len = table.ItemCount;
        var miningValue = int.MaxValue;
        for (var i = 0; i < len; i++)
        {
            var d = table.GetItem(i);
            if (d.MiningValue < 1)
            {
                continue;
            }
            if (d.MiningValue < miningValue)
            {
                miningValue = d.MiningValue;
                levelLimit = d.Id;
            }
        }
        */
        this.money_Slider.onValueChanged.AddListener(this.OnSliderChanged);
    }

    private void OnSliderChanged(float arg0)
    {
        var money = PlayerSystem.Instance.GetDiamond();
        addMoneyValue = Mathf.FloorToInt(money_Slider.value);
        if (money < addMoneyValue)
        {
            this.showmoney_TextMeshProUGUI.text = addMoneyValue + "/<color=#ff0000>" + FormatUtils.FormatMoney(money) + "</color>";
        }
        else
        {
            this.showmoney_TextMeshProUGUI.text = addMoneyValue + "/" + FormatUtils.FormatMoney(money);
        }

    }

    protected override void OnOpen()
    {
        base.OnOpen();
        this.stakeEndTime_TextMeshProUGUI.text = "Ended";
        this.needlv_TextMeshProUGUI.text = GameEntry.Table.Lang.GetText(1120);//levelLimit;
        var lv = CarCultivateSystem.Instance.GetCarLevel();
        Utils.Unity.SetActive(this.needlv_TextMeshProUGUI, lv < levelLimit);
        Utils.Unity.SetActive(this.carStake_Button, lv >= levelLimit);
        Utils.Unity.SetActive(this.carnode_GameObject, false);
        Utils.Unity.SetActive(this.moneynode_GameObject, false);
        Utils.Unity.SetActive(this.redeemnode_GameObject, false);

        OnMiningChanged();
        EventManager.Instance.Regist(EventDefine.Global.OnMiningChanged, OnMiningChanged);
        EventManager.Instance.Regist(EventDefine.Global.OnRankChange, OnRankChange);
        EventManager.Instance.Regist(EventDefine.Global.OnGoldChanged, updateGold);
        OnRankChange();
        var Info = RankSystem.Instance.GetRankInfo(Proto.RankTypeEnum.MiningTokenScore);
        if (!Info.HasData)
        {
            RankSystem.Instance.RequestSelf(Proto.RankTypeEnum.MiningTokenScore);
        }
        updateGold();
        GameEntry.Http.Handler.GetChangeDataSystemData();
    }

    private void updateGold()
    {
        this.allValue_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1101, PlayerSystem.Instance.TokenScore);
    }

    private void OnRankChange()
    {
        int rankValue = 3;
        var Info = RankSystem.Instance.GetRankInfo(Proto.RankTypeEnum.MiningTokenScore);
        if (Info.SelfData != null && Info.SelfData.Rank > 0)
        {
            rankValue = Info.SelfData.Rank - 1;
            if (rankValue > 2)
            {
                rankValue = 3;
            }
            this.ranktxt_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1126, Info.SelfData.Rank);
        }
        else
        {
            this.ranktxt_TextMeshProUGUI.text = GameEntry.Table.Lang.GetText(1127);
        }
        var len = this.rankIcon_RectTransform.childCount;
        while (len-- > 0)
        {
            var child = this.rankIcon_RectTransform.GetChild(len);
            Utils.Unity.SetActive(child, len == rankValue);
        }
    }

    protected override void OnClose()
    {
        base.OnClose();
        EventManager.Instance.Remove(EventDefine.Global.OnMiningChanged, OnMiningChanged);
        EventManager.Instance.Remove(EventDefine.Global.OnRankChange, OnRankChange);
        EventManager.Instance.Remove(EventDefine.Global.OnGoldChanged, updateGold);
    }
    private List<float> _rateValue = new List<float>();
    private List<float> GetRate()
    {
        var date = GameEntry.OfflineManager.GetServerDateTime();

        float[] rates;
        int addCount = 0;
        int addScore = 0;
        if (date.Day == 21)
        {
            rates = new float[] { 2f, 2.5f, 3f, 2.2f };
        }
        else if (date.Day == 22)
        {
            rates = new float[] { 2f, 2.3f, 2.3f, 2.1f };
            addCount = 800;
            addScore = addCount * 7;
        }
        else if (date.Day == 23)
        {
            rates = new float[] { 2.2f, 2.4f, 2.6f, 2.4f };
            addCount = 1000;
            addScore = addCount * 30;
        }
        else if (date.Day == 24)
        {
            rates = new float[] { 2.6f, 2.6f, 2.6f, 2.6f };
            addCount = 1000;
            addScore = addCount * 66;
        }
        else
        {
            rates = new float[] { 1f, 1f, 1f, 1f };
        }
        var hours = new int[4] { 6, 12, 18, 24 };
        var len = hours.Length;
        float result = 1f;
        while (len-- > 0)
        {
            if (date.Hour < hours[len])
            {
                result = rates[len];
            }
        }
        _rateValue.Clear();
        _rateValue.Add(result);
        _rateValue.Add(addCount);
        _rateValue.Add(addScore);
        return _rateValue;
    }
    private void OnMiningChanged()
    {
        var sys = MiningSystem.Instance;
        var carlv = sys.Data.CarLevel;
        var rateInfo = GetRate();
        var rate = rateInfo[0];
        var addRole = rateInfo[1];
        var addMoney = rateInfo[2];
        rate = 1f;
        addRole = 1f;
        addMoney = 1f;
        // Utils.Unity.SetActive(this.carClaim_Button, carlv > 0);
        // Utils.Unity.SetActive(this.carStart_Button, carlv < 1);
        // this.allValue_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1101, sys.Data.CarSettlementTokenScore + sys.Data.DiamondSettlementTokenScore);
        Utils.Unity.SetActive(this.carReplace_Button, carlv > 0);
        Utils.Unity.SetActive(this.carclaimNode_GameObject, carlv > 0);
        var cartpl = GameEntry.Table.Mining.Get(1);

        this.cartotalstake_TextMeshProUGUI.text = cartpl.TotalScorePool + "";
        this.cartodaytotalstake_TextMeshProUGUI.text = cartpl.ScorePool + "";
        this.carcount_TextMeshProUGUI.text = Mathf.FloorToInt(sys.Data.CarTotalMiningCount * rate + addRole) + "";
        this.carcountValue_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1130, (long)(sys.Data.CarTotalMiningValue * rate + addMoney));
        // this.carstake_TextMeshProUGUI.text = string.Format(GameEntry.Table.Lang.GetText(1125), GameEntry.Table.CarUpgrade.Get(carlv).MiningValue);
        this.carreceived_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1132, sys.Data.CarSettlementTokenScore);
        this.carunclaim_TextMeshProUGUI.text = (sys.Data.CarPreSettlementTokenScore) + "";


        var moneytpl = GameEntry.Table.Mining.Get(2);
        minMoney = moneytpl.Condition;
        addMoneyValue = moneytpl.Condition;

        money_Slider.minValue = moneytpl.Condition;
        money_Slider.maxValue = Mathf.Max(PlayerSystem.Instance.GetDiamond(), moneytpl.Condition);
        Utils.Unity.SetActive(this.moneyCancel_Button, sys.Data.DiamondValue > 0);
        // Utils.Unity.SetActive(this.moneyClaim_Button, sys.Data.DiamondValue > 0);
        Utils.Unity.SetActive(this.moneyclaimNode_GameObject, sys.Data.DiamondValue > 0);
        this.moneytotalstake_TextMeshProUGUI.text = moneytpl.TotalScorePool + "";
        this.moneytodaytotalstake_TextMeshProUGUI.text = cartpl.ScorePool + "";
        this.moneycount_TextMeshProUGUI.text = Mathf.FloorToInt(rate * sys.Data.DiamondTotalMiningCount) + "";
        this.moneycountValue_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1131, (long)(rate * sys.Data.DiamondTotalMiningValue));
        this.moneystake_TextMeshProUGUI.text = string.Format(GameEntry.Table.Lang.GetText(1124), sys.Data.DiamondValue);
        this.moneyreceived_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1133, sys.Data.DiamondSettlementTokenScore);
        this.moneyunclaim_TextMeshProUGUI.text = (sys.Data.DiamondPreSettlementTokenScore) + "";

        this.totalstake_TextMeshProUGUI.text = (moneytpl.TotalScorePool + cartpl.TotalScorePool).ToString();



        OnUpdateSeconds();
    }
    protected override void OnUpdateSeconds()
    {
        base.OnUpdateSeconds();
        var sys = MiningSystem.Instance;
        var lv = sys.Data.CarLevel;
        this.carClaimCd_TextMeshProUGUI.text = "";
        if (lv > 0 || sys.Data.DiamondValue > 0)
        {
            var now = GameEntry.OfflineManager.GetServerDateTime();
            if (lv > 0)
            {
                var cartpl = GameEntry.Table.Mining.Get(1);
                var targetTime = TimeUtils.GetDateTimeByMilliseconds(sys.Data.CarLastPreSettlementTime).AddMinutes(cartpl.PreSettlementTime);
                var diff = targetTime - now;
                if (diff.TotalSeconds > 0)
                {
                    this.carClaimCd_TextMeshProUGUI.text = TimeUtils.GetRemainder(diff, true);
                    var p = (float)diff.TotalSeconds / (cartpl.PreSettlementTime * 60f);
                    this.carTimeImg_RectTransform.localRotation = Quaternion.Euler(0, 0, (1f - p) * 360f);
                }
            }
            if (sys.Data.DiamondValue > 0)
            {
                var moneytpl = GameEntry.Table.Mining.Get(2);
                var targetTime = TimeUtils.GetDateTimeByMilliseconds(sys.Data.DiamondLastPreSettlementTime).AddMinutes(moneytpl.PreSettlementTime);
                var diff = targetTime - now;
                if (diff.TotalSeconds > 0)
                {
                    this.moneyClaimCd_TextMeshProUGUI.text = TimeUtils.GetRemainder(diff, true);
                    var p = (float)diff.TotalSeconds / (moneytpl.PreSettlementTime * 60f);
                    this.moneyTimeImg_RectTransform.localRotation = Quaternion.Euler(0, 0, (1f - p) * 360f);
                }
            }
        }
        updateTime();
    }
    private void updateTime()
    {
        var sys = MiningSystem.Instance;
        var miningCloseTime = TimeUtils.GetDateTimeByMilliseconds(sys.Data.MiningCloseTime);
        var now = GameEntry.OfflineManager.GetServerDateTime();
        var offset = TimeUtils.GetDiff(now, miningCloseTime);
        if (sys.Data.MiningCloseTime == 0)
        {
            hasFinish = false;
            this.stakeEndTime_TextMeshProUGUI.text = "∞";
        }
        if (offset.TotalSeconds > 0)
        {
            hasFinish = offset.TotalSeconds < 1;
            this.stakeEndTime_TextMeshProUGUI.text = TimeUtils.GetRemainder(offset);
        }
        else
        {
            this.stakeEndTime_TextMeshProUGUI.text = "Ended";
        }
    }

    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.carStart_Button)
        {
            if (hasFinish)
            {
                return;
            }
            Utils.Unity.SetActive(this.carnode_GameObject, true);
            var carlv = CarCultivateSystem.Instance.GetCarLevel();
            this.carlv_TextMeshProUGUI.text = "LV." + carlv;
            // this.carvalue_TextMeshProUGUI.text = "Value=" + GameEntry.Table.CarUpgrade.Get(carlv).MiningValue + "$";

        }
        else if (target == this.moneyStart_Button)
        {
            if (hasFinish)
            {
                return;
            }
            OnSliderChanged(minMoney);
            Utils.Unity.SetActive(this.moneynode_GameObject, true);
            var moneytpl = GameEntry.Table.Mining.Get(2);
            minMoney = moneytpl.Condition;
            var money = PlayerSystem.Instance.GetDiamond();
            Utils.Unity.SetActive(moneyStake_Button, money >= minMoney);
        }
        else if (target == this.carReplace_Button)
        {
            // GameEntry.Http.Handler.StakeCar();
            Utils.Unity.SetActive(this.carCancelnode_GameObject, true);
        }
        else if (target == this.close_Button)
        {
            Close();
        }
        else if (target == this.carNodeClose_Button)
        {
            Utils.Unity.SetActive(this.carnode_GameObject, false);
        }
        else if (target == this.carStake_Button)
        {
            if (hasFinish)
            {
                return;
            }
            GameEntry.Http.Handler.StakeCar();
            MiningSystem.Instance.DelayUpdate();
            Utils.Unity.SetActive(this.carnode_GameObject, false);
        }
        else if (target == this.moneyNodeClose_Button)
        {
            Utils.Unity.SetActive(this.moneynode_GameObject, false);
        }
        else if (target == moneyStake_Button)
        {
            if (hasFinish)
            {
                return;
            }
            var money = PlayerSystem.Instance.GetDiamond();
            if (money < addMoneyValue)
            {
                return;
            }
            GameEntry.Http.Handler.StakeDiamond(addMoneyValue);
            Utils.Unity.SetActive(this.moneynode_GameObject, false);
            MiningSystem.Instance.DelayUpdate();
        }
        else if (target == moneyCancel_Button)
        {
            Utils.Unity.SetActive(this.redeemnode_GameObject, true);

        }
        else if (target == carClaim_Button)
        {
            GameEntry.Http.Handler.StakeCarClaim();
            Utils.Unity.SetActive(target, false);
            GameEntry.Timer.Start(3f, () =>
            {
                OnMiningChanged();
                GameEntry.Http.Handler.GetSystemData(Proto.SystemEnum.MiningSystem);
            });
        }
        else if (target == moneyClaim_Button)
        {
            GameEntry.Http.Handler.StakeDiamondClaim();
            Utils.Unity.SetActive(target, false);
            GameEntry.Timer.Start(3f, () =>
            {
                OnMiningChanged();
                GameEntry.Http.Handler.GetSystemData(Proto.SystemEnum.MiningSystem);
            });
        }
        else if (target == this.moneyadd_Button)
        {
            money_Slider.value += 100;
        }
        else if (target == this.moneysub_Button)
        {
            money_Slider.value -= 100;
        }
        else if (target == this.moneyRedeemOk_Button)
        {
            GameEntry.Http.Handler.StakeDiamondCancel();
            Utils.Unity.SetActive(this.redeemnode_GameObject, false);
            MiningSystem.Instance.DelayUpdate();
        }
        else if (target == this.moneyRedeemCancel_Button)
        {
            Utils.Unity.SetActive(this.redeemnode_GameObject, false);
        }
        else if (target == this.carRedeemCancel_Button)
        {
            Utils.Unity.SetActive(this.carCancelnode_GameObject, false);
        }
        else if (target == this.carRedeemOk_Button)
        {
            Utils.Unity.SetActive(this.carCancelnode_GameObject, false);
            GameEntry.Http.Handler.StakeCanCancel();
            MiningSystem.Instance.DelayUpdate();
        }
        else if (target == this.rankIcon_Button)
        {
            ExecuteAction.ByActionType(ExecuteActionType.Rank);
        }
        else if (target == rankclick_Button)
        {
            ExecuteAction.ByActionType(ExecuteActionType.Rank);
        }
        else if (target == rule_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.ScoreRankRuleUI.Path);
        }
    }
}