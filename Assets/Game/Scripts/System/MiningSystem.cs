using Framework;
using Proto;
using System;
public class MiningSystem : BaseSystem<MiningSystem>
{
    public ResMsgBodyMiningSystem Data { get; private set; }
    public bool CanChangeCar { get; private set; }
    private int delayUpdateDataId;
    private float _delayGetInfo;
    private ResMsgBodyMiningSystem LastData;
    public void SetData(ResMsgBodyMiningSystem value)
    {


        this.Data = value;
        CanChangeCar = true;
        if (this.Data.CarLevel > 0)
        {
            cooldownHandler();
        }
        var lv = Data.CarLevel;
        if (lv > 0 || Data.DiamondValue > 0)
        {

            var now = GameEntry.OfflineManager.GetServerDateTime();
            if (lv > 0)
            {
                var cartpl = GameEntry.Table.Mining.Get(1);
                var targetTime = TimeUtils.GetDateTimeByMilliseconds(Data.CarLastPreSettlementTime).AddMinutes(cartpl.PreSettlementTime);
                var diff = targetTime - now;
                if (diff.TotalSeconds > 0)
                {
                    GameEntry.OfflineManager.SetTimeTask("CarLastPreSettlementTime", targetTime, timeout);
                }
            }
            if (Data.DiamondValue > 0)
            {
                var moneytpl = GameEntry.Table.Mining.Get(2);
                var targetTime = TimeUtils.GetDateTimeByMilliseconds(Data.DiamondLastPreSettlementTime).AddMinutes(moneytpl.PreSettlementTime);
                var diff = targetTime - now;
                if (diff.TotalSeconds > 0)
                {
                    GameEntry.OfflineManager.SetTimeTask("DiamondLastPreSettlementTime", targetTime, timeout);
                }
            }
        }
        if (_delayGetInfo > 0)
        {
            DelayUpdate();
            _delayGetInfo = 0f;
        }
        if (LastData != null)
        {
            if (Data.CarLevel > LastData.CarLevel)
            {
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgBoxTipUI.Path, "lang", 1168);
            }
            if (Data.DiamondValue > LastData.DiamondValue)
            {
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgBoxTipUI.Path, "lang", 1168);
            }
        }
        LastData = Data;
        EventManager.Instance.Dispatch(EventDefine.Global.OnMiningChanged);
    }
    public TimeSpan GetChangeCarTimeRemain()
    {
        var cartpl = GameEntry.Table.Mining.Get(1);
        var cooldownTime = TimeUtils.GetDateTimeByMilliseconds(Data.CarLastChangeTime).AddMinutes(cartpl.Cooldown);
        return cooldownTime - GameEntry.OfflineManager.GetServerDateTime();
    }
    public DateTime GetChangeCarTime()
    {
        var cartpl = GameEntry.Table.Mining.Get(1);
        return TimeUtils.GetDateTimeByMilliseconds(Data.CarLastChangeTime).AddMinutes(cartpl.Cooldown);
    }
    public void DelayUpdate()
    {
        GameEntry.Timer.Stop(delayUpdateDataId);
        delayUpdateDataId = GameEntry.Timer.Start(2, udpateInfo, 1);
    }
    private void udpateInfo()
    {
        GameEntry.Http.Handler.GetSystemData(SystemEnum.MiningSystem);
    }
    private void timeout()
    {
        _delayGetInfo = 2f;
        udpateInfo();
    }
    private void cooldownHandler()
    {
        DateTime now = GameEntry.OfflineManager.GetServerDateTime();
        var cooldownTime = GetChangeCarTime();
        if ((cooldownTime - now).TotalSeconds > 0)
        {
            CanChangeCar = true;
            GameEntry.OfflineManager.SetTimeTask("MiningSystem.Cooldown", cooldownTime, cooldownHandler);
        }
        else
        {
            CanChangeCar = true;
        }
        EventManager.Instance.Dispatch(EventDefine.Global.OnMiningChanged);
    }

}