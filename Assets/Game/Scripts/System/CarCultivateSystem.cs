using System;
using System.Collections.Generic;
using Framework;
using Proto;
using UnityEngine;
public class CarCultivateSystem : BaseSystem<CarCultivateSystem>
{
    private bool _firstLogin;
    public ResMsgBodyCarCultivateSystem Data { get; private set; }
    public bool FreeBuy;
    private Dictionary<CarModuleTypeEnum, int> levels;
    private List<int> moduleLevel = new List<int>();
    private ResMsgBodyCarCultivateSystem _firstData;
    private ResMsgBodyCarCultivateSystem LastData;
    public PropertyUpgradeInfo currentLevelInfo = new PropertyUpgradeInfo();
    private bool _showUpgradeInfo;
    public CarCultivateSystem()
    {
        levels = new Dictionary<CarModuleTypeEnum, int>();
        _firstLogin = true;
    }
    public void SetData(ResMsgBodyCarCultivateSystem data)
    {
        this.Data = data;
        levels.Clear();
        moduleLevel.Clear();
        for (var i = 0; i < 4; i++)
        {
            moduleLevel.Add(0);
        }
        foreach (var item in data.CarCultivateDatas)
        {
            levels.Add(item.CarModuleTypeEnum, item.Level);
            if (item.CarModuleTypeEnum != CarModuleTypeEnum.Car)
            {
                moduleLevel[(int)item.CarModuleTypeEnum - 1] = item.Level;
            }
        }
        if (_firstLogin)
        {
            _firstLogin = false;
            if (Data.IntervalEarningsGold > 0)
            {
                _firstData = Data;
            }
            // Debug.LogError("离线收益:" + data.IntervalEarningsGold);
        }
        GameEntry.OfflineManager.OfflineReward.Refresh();
        var now = GameEntry.OfflineManager.GetServerDateTime();
        var finishFreeTime = TimeUtils.GetDateTimeByMilliseconds(Data.LastFreeGetSpeedUpTime);
        finishFreeTime = finishFreeTime.AddMinutes(GameEntry.Table.Param.Get("GetFreeSpeedUpIntervalTime").IntParam);
        var diff = TimeUtils.GetDiff(now, finishFreeTime);
        GameEntry.RedDot.Set(RedDotName.FreeSpeedUp, nameof(CarCultivateSystem), diff.TotalSeconds < 1);
        if (diff.TotalSeconds > 0)
        {
            GameEntry.OfflineManager.SetTimeTask("freespeedup", finishFreeTime, () =>
            {

            });
        }
        if (LastData != null)
        {
            if (Data.SpeedUpEndTime > LastData.SpeedUpEndTime)
            {
                if (!FreeBuy)
                {
                    GameEntry.GUI.Open(GameEntry.GUIPath.MsgBoxTipUI.Path, "lang", 1420);
                }
            }
        }
        FreeBuy = false;
        LastData = Data;
        EventManager.Instance.Dispatch(EventDefine.Global.OnCarCultivateChanged);
        if (_showUpgradeInfo)
        {
            _showUpgradeInfo = false;
            GameEntry.GUI.Open(GameEntry.GUIPath.GarageUpgradeUI.Path, "info", currentLevelInfo);
        }

    }
    public void EnterMain()
    {
        if (_firstData != null)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.OfflineRewardUI.Path, "data", _firstData);
            _firstData = null;
            // Debug.LogError("离线收益:" + data.IntervalEarningsGold);
        }
    }
    public int GetCarLevel()
    {
        if (levels.TryGetValue(CarModuleTypeEnum.Car, out int level))
        {

        }
        return level;
    }
    public List<int> GetModuleLevels()
    {
        return moduleLevel;
    }
    public int GetModuleLevel(CarModuleType type)
    {
        return levels[(CarModuleTypeEnum)type];
    }
    //最大速度
    public float GetCarSpeed()
    {
        var minmax = GameEntry.Table.Param.Get("CarSpeed").IntParams;
        var min = (float)minmax[0];
        var max = (float)minmax[1];
        var p1 = (float)GameEntry.Table.CarUpgrade.MinLevelData.Property1;
        var p2 = (float)GameEntry.Table.CarUpgrade.MaxLevelData.Property1;
        var lv = CarCultivateSystem.Instance.GetCarLevel();
        var p3 = (float)GameEntry.Table.CarUpgrade.Get(lv).Property1;
        //(p3-p1)/(p2-p1)=(speed-min)/(max-min)
        var speed = (p3 - p1) / (p2 - p1) * (max - min) + min;
        return speed;
    }
    public int GetShowSpeed(float speed)
    {
        var minmax = GameEntry.Table.Param.Get("CarSpeed").IntParams;
        var min = (float)minmax[0];
        var max = (float)minmax[1];
        var p1 = (float)GameEntry.Table.CarUpgrade.MinLevelData.Property1;
        var p2 = (float)GameEntry.Table.CarUpgrade.MaxLevelData.Property1;
        if (speed < min)
        {
            return Mathf.FloorToInt(p1 * speed / min);
        }
        else
        {
            //(speed-min)/(max-min)=(value-p1)/(p2-p1)
            var p = (speed - min) / (max - min);
            var s = p2 - p1;
            return Mathf.FloorToInt(p * s + p1);
        }
    }
    //氮气加速cd时间
    public float GetNosCdTime()
    {
        var max = (float)GameEntry.Table.Param.Get("NosCD").IntParam;
        var p1 = (float)GameEntry.Table.CarUpgrade.MinLevelData.Property2;
        var p2 = (float)GameEntry.Table.CarUpgrade.MaxLevelData.Property2;
        var lv = CarCultivateSystem.Instance.GetCarLevel();
        var p3 = (float)GameEntry.Table.CarUpgrade.Get(lv).Property2;
        //p3/(p2-p1)*(p2-p1)*0.1f*max
        var value = p3 - p1;
        var p = value / (p2 - p1) * (p2 - p1);
        var time = (1f - p * 0.001f) * max;
        return time;
    }
    //眩晕时间
    public float GetDizzyTime()
    {
        var max = (float)GameEntry.Table.Param.Get("DizzyTime").IntParam;
        var p1 = (float)GameEntry.Table.CarUpgrade.MinLevelData.Property3;
        var p2 = (float)GameEntry.Table.CarUpgrade.MaxLevelData.Property3;
        var lv = CarCultivateSystem.Instance.GetCarLevel();
        var p3 = (float)GameEntry.Table.CarUpgrade.Get(lv).Property3;
        //p3/(p2-p1)*(p2-p1)*0.1f*max
        var value = p3 - p1;
        var p = value / (p2 - p1) * (p2 - p1);
        var time = (1f - p * 0.001f) * max;
        return time;
    }
    //氮气加速度
    public float GetNosSpeed()
    {
        var max = (float)GameEntry.Table.Param.Get("NosSpeed").IntParam;
        var p1 = (float)GameEntry.Table.CarUpgrade.MinLevelData.Property4;
        var p2 = (float)GameEntry.Table.CarUpgrade.MaxLevelData.Property4;
        var lv = CarCultivateSystem.Instance.GetCarLevel();
        var p3 = (float)GameEntry.Table.CarUpgrade.Get(lv).Property4;
        //p3/(p2-p1)*(p2-p1)*0.1f*max
        var value = p3 - p1;
        var p = value / (p2 - p1) * (p2 - p1);
        var time = (1f + p * 0.001f) * max;
        return time;
    }
    public PropertyInfo GetPropertyInfo(int Id,int level)
    {
        var Info=new PropertyInfo();
        float value = 0f;
        float min = 0f;
        float max = 0f;
        float nextValue = 0f;
        var lv = level;//CarCultivateSystem.Instance.GetModuleLevel((CarModuleType)Id);
        Info.Level = lv;
        var tpl = GameEntry.Table.CarUpgrade.Get(lv);
        var next = GameEntry.Table.CarUpgrade.Get(lv + 1);
        long cost = 0;
        int costitemId = 0;
        long costitemCount = 0;
        int score = 0;
        int maxScore = 0;
        if (Id == (int)CarModuleType.Module1)
        {
            value = tpl.Property1;
            min = GameEntry.Table.CarUpgrade.MinLevelData.Property1;
            max = GameEntry.Table.CarUpgrade.MaxLevelData.Property1;
            if (next != null)
            {
                nextValue = next.Property1;
            }
            cost = tpl.Cost1;
            costitemId = tpl.CostItem1[0];
            costitemCount = tpl.CostItem1[1];
            score = tpl.Scoring1;
            maxScore = GameEntry.Table.CarUpgrade.MaxLevelData.Scoring1;
        }
        else if (Id == (int)CarModuleType.Module2)
        {
            value = tpl.Property2;
            min = GameEntry.Table.CarUpgrade.MinLevelData.Property2;
            max = GameEntry.Table.CarUpgrade.MaxLevelData.Property2;
            if (next != null)
            {
                nextValue = next.Property2;
            }
            cost = tpl.Cost2;
            costitemId = tpl.CostItem2[0];
            costitemCount = tpl.CostItem2[1];
            score = tpl.Scoring2;
            maxScore = GameEntry.Table.CarUpgrade.MaxLevelData.Scoring2;
        }
        else if (Id == (int)CarModuleType.Module3)
        {
            value = tpl.Property3;
            min = GameEntry.Table.CarUpgrade.MinLevelData.Property3;
            max = GameEntry.Table.CarUpgrade.MaxLevelData.Property3;
            if (next != null)
            {
                nextValue = next.Property3;
            }
            cost = tpl.Cost3;
            costitemId = tpl.CostItem3[0];
            costitemCount = tpl.CostItem3[1];
            score = tpl.Scoring3;
            maxScore = GameEntry.Table.CarUpgrade.MaxLevelData.Scoring3;
        }
        else if (Id == (int)CarModuleType.Module4)
        {
            value = tpl.Property4;
            min = GameEntry.Table.CarUpgrade.MinLevelData.Property4;
            max = GameEntry.Table.CarUpgrade.MaxLevelData.Property4;
            if (next != null)
            {
                nextValue = next.Property4;
            }
            cost = tpl.Cost4;
            costitemId = tpl.CostItem4[0];
            costitemCount = tpl.CostItem4[1];
            score = tpl.Scoring4;
            maxScore = GameEntry.Table.CarUpgrade.MaxLevelData.Scoring4;
        }

        if (nextValue < 1)
        {
            nextValue = value;
        }
        Info.Cost = cost;
        Info.CostItemId = costitemId;
        Info.CostItemCount = costitemCount;
        Info.Score = score;
        Info.MaxScore=maxScore;
        Info.Value = value;
        Info.NextValue = nextValue;
        Info.Min = min;
        Info.Max = max;
        if (Id != (int)CarModuleType.Module1)
        {
            // Value = (value * 0.1f).ToString("0.##");
            // Base = (min * 0.1f).ToString("0.##") + "%";
            // Add = ((value - min) * 0.1f).ToString("0.##") + "%";
            // this.value_TextMeshProUGUI.text = "<color #00ff00>" + (value * 0.1f).ToString("0.##") + "%</color>(" + (min * 0.1f).ToString("0.##") + "+<color #00ff00>" + ((value - min) * 0.1f).ToString("0.##") + "</color>)";
            // NextLvAddValue = ((nextValue - value) * 0.1f).ToString("0.##");
        }
        else
        {
            // Value = value.ToString();
            // Base = min.ToString();
            // Add = (value - min).ToString();
            // this.value_TextMeshProUGUI.text = "<color #00ff00>" + value.ToString() + "</color>(" + min + "+<color #00ff00>" + (value - min) + "</color>)";
            // NextLvAddValue = (nextValue - value).ToString();
        }

        var total = max - min;
        if (total > 0)
        {
            Info.Progress = (value - min) / total;
        }
        return Info;
    }
    public void UpgradeCar()
    {
        _showUpgradeInfo = true;
        currentLevelInfo.Grab();
        GameEntry.Http.Handler.CarUpgrade();
    }

}
