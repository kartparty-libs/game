
using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class BuySpeedup : UIWindowBase
{
    private CarCultivateSystem carCultivateSystem;
    private int[] costList;
    protected override void OnOpen()
    {
        base.OnOpen();
        EventManager.Instance.Regist(EventDefine.Global.OnCarCultivateChanged, OnCarCultivateChanged);
        carCultivateSystem = CarCultivateSystem.Instance;
        var timeList = GameEntry.Table.Param.Get("EarningsSpeedUpTime").IntParams;
        costList = GameEntry.Table.Param.Get("EarningsSpeedUpDiamond").IntParams;
        var timetxtList = new List<TMPro.TextMeshProUGUI>() { this.time0_TextMeshProUGUI, this.time1_TextMeshProUGUI, this.time2_TextMeshProUGUI };
        var len = timetxtList.Count;
        for (var i = 0; i < len; i++)
        {
            timetxtList[i].text = timeList[i] + "min";
        }
        this.cost1_TextMeshProUGUI.text = costList[1].ToString();
        this.cost2_TextMeshProUGUI.text = costList[2].ToString();
        Refresh();
    }

    private void OnCarCultivateChanged()
    {
        Refresh();
    }

    protected override void OnClose()
    {
        base.OnClose();
        EventManager.Instance.Remove(EventDefine.Global.OnCarCultivateChanged, OnCarCultivateChanged);
    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.close_Button)
        {
            Close();
        }
        else if (target == this.slot1_Button)
        {
            var finishFreeTime = TimeUtils.GetDateTimeByMilliseconds(carCultivateSystem.Data.LastFreeGetSpeedUpTime).AddMinutes(GameEntry.Table.Param.Get("GetFreeSpeedUpIntervalTime").IntParam);
            var diff = TimeUtils.GetDiff(GameEntry.OfflineManager.GetServerDateTime(), finishFreeTime);
            if (diff.TotalSeconds > 0)
            {
                return;
            }
            carCultivateSystem.FreeBuy = true;
            GameEntry.Http.Handler.BuyGoldSpeedup(0);
        }
        else if (target == this.slot2_Button)
        {
            if (PlayerSystem.Instance.GetDiamond() < costList[1])
            {
                JumpUtils.GetMoney(1);
                return;
            }
            GameEntry.Http.Handler.BuyGoldSpeedup(1);
        }
        else if (target == this.slot3_Button)
        {
            if (PlayerSystem.Instance.GetDiamond() < costList[2])
            {
                JumpUtils.GetMoney(1);
                return;
            }
            GameEntry.Http.Handler.BuyGoldSpeedup(2);
        }
        else if (target == add_Button)
        {
            JumpUtils.GetMoney(1);
        }
    }
    protected override void OnUpdateSeconds()
    {
        base.OnUpdateSeconds();

        Refresh();
    }
    private void Refresh()
    {
        var now = GameEntry.OfflineManager.GetServerDateTime();
        var finishTime = TimeUtils.GetDateTimeByMilliseconds(carCultivateSystem.Data.SpeedUpEndTime);
        var diff = TimeUtils.GetDiff(now, finishTime);
        if (diff.TotalSeconds > 0)
        {
            this.finishTime_TextMeshProUGUI.text = TimeUtils.GetRemainder(diff);
        }
        else
        {
            this.finishTime_TextMeshProUGUI.text = "";
        }
        var finishFreeTime = TimeUtils.GetDateTimeByMilliseconds(carCultivateSystem.Data.LastFreeGetSpeedUpTime);
        // var minutes = GameEntry.Table.Param.Get("GetFreeSpeedUpIntervalTime").IntParam;
        // var finishFreeTime = lastfreeTime.AddMinutes(minutes);
        diff = TimeUtils.GetDiff(now, finishFreeTime);
        if (diff.TotalSeconds > 0)
        {
            this.freeCDTime_TextMeshProUGUI.text = TimeUtils.GetRemainder(diff);
        }
        else
        {
            this.freeCDTime_TextMeshProUGUI.text = GameEntry.Table.Lang.GetText(1411);
        }
        this.money_TextMeshProUGUI.text = FormatUtils.FormatMoney(PlayerSystem.Instance.GetDiamond());

    }
}