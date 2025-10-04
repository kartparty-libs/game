
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class OfflineReward : IServerTime
{
    private int CarLevel = 1;
    private bool _hasNext;
    private long _nextUpgradeGold;
    private bool _carUpgrade;
    private float _cd;
    private int _nextUpgradeType;
    private List<long> _moduleCostGold = new List<long>();
    private CarUpgrade_Data _tplData;
    private int _addValue;
    private bool _speedUpMode;
    public OfflineReward()
    {
        for (int i = 0; i < 5; i++)
        {
            _moduleCostGold.Add(int.MaxValue);
        }
    }
    public void NewDay()
    {

    }
    public void Refresh()
    {
        CarLevel = CarCultivateSystem.Instance.GetCarLevel();
        _tplData = GameEntry.Table.CarUpgrade.Get(CarLevel);
        if (CarLevel >= GameEntry.Table.CarUpgrade.MaxLevelData.Id)
        {
            _hasNext = false;
        }
        else
        {
            _hasNext = true;
        }
        _moduleCostGold[0] = _tplData.Cost1;
        _moduleCostGold[1] = _tplData.Cost2;
        _moduleCostGold[2] = _tplData.Cost3;
        _moduleCostGold[3] = _tplData.Cost4;
        _carUpgrade = false;
        _nextUpgradeGold = -1;

        var lv = Mathf.Max(1, CarLevel);
        var cardata = GameEntry.Table.CarUpgrade.Get(lv);
        var carCultivateSystem = CarCultivateSystem.Instance;
        float rate = 1f;
        _speedUpMode = false;
        if (carCultivateSystem.Data != null)
        {
            var now = GameEntry.OfflineManager.GetServerDateTime();
            var finishTime = TimeUtils.GetDateTimeByMilliseconds(carCultivateSystem.Data.SpeedUpEndTime);
            var diff = TimeUtils.GetDiff(now, finishTime);
            if (diff.TotalSeconds > 0)
            {
                rate = GameEntry.Table.Param.Get("EarningsSpeedMultiplier").IntParam;
                _speedUpMode = true;

            }
        }
        int addValue = 0;
        var tpl = GameEntry.Table.CarUpgrade.Get(CarCultivateSystem.Instance.GetModuleLevel(CarModuleType.Module1));
        // addValue += tpl.ProductSeconds1;
        tpl = GameEntry.Table.CarUpgrade.Get(CarCultivateSystem.Instance.GetModuleLevel(CarModuleType.Module2));
        // addValue += tpl.ProductSeconds2;
        tpl = GameEntry.Table.CarUpgrade.Get(CarCultivateSystem.Instance.GetModuleLevel(CarModuleType.Module3));
        // addValue += tpl.ProductSeconds3;
        tpl = GameEntry.Table.CarUpgrade.Get(CarCultivateSystem.Instance.GetModuleLevel(CarModuleType.Module4));
        // addValue += tpl.ProductSeconds4;

        // _addValue = Mathf.FloorToInt((cardata.ProductSeconds + addValue) * rate);
        updateInfo();

    }
    public void Reset()
    {
        _cd = 0f;
    }

    public void UpdateMinutes()
    {
    }
    public bool HasModuleUpgrade(CarModuleType type)
    {
        var moduleLevel = CarCultivateSystem.Instance.GetModuleLevels();
        var id = (int)type - 1;
        var lv = moduleLevel[id];
        if (lv > CarLevel)
        {
            return true;
        }
        return false;
    }
    public int GetUpgradePart()
    {
        return _nextUpgradeType;
    }
    public long GetUpgradeGold()
    {
        if (_nextUpgradeGold < 1)
        {
            return 0;
        }
        return _nextUpgradeGold;
    }
    public bool CanCarUpgrade()
    {
        var moduleLevel = CarCultivateSystem.Instance.GetModuleLevels();
        foreach (var item in moduleLevel)
        {
            if (item == CarLevel)
            {
                return false;
            }
        }
        return true;
    }
    public int GetAddValue(int carLevel)
    {
        var tpl = GameEntry.Table.CarUpgrade.Get(carLevel);
        int addvalue = 0;
        // addValue += tpl.ProductSeconds;
        // addValue += tpl.ProductSeconds1;
        // addValue += tpl.ProductSeconds2;
        // addValue += tpl.ProductSeconds3;
        // addValue += tpl.ProductSeconds4;
        return addvalue;
    }
    // public int GetPrevAddValue(int carLevel)
    // {
    //     var tpl = GameEntry.Table.CarUpgrade.Get(carLevel);
    //     int addValue = tpl.ProductSeconds;
    //     addValue += tpl.ProductSeconds1;
    //     addValue += tpl.ProductSeconds2;
    //     addValue += tpl.ProductSeconds3;
    //     tpl = GameEntry.Table.CarUpgrade.Get(carLevel - 1);
    //     addValue += tpl.ProductSeconds4;
    //     return addValue;
    // }
    public int GetAddValue()
    {
        return _addValue;
    }
    public bool IsSpeedUp()
    {
        return _speedUpMode;
    }
    public void UpdateSeconds()
    {
        /*
        var lv = Mathf.Max(1, CarLevel);
        var cardata = GameEntry.Table.CarUpgrade.Get(lv);
        float rate = 1f;
        var carCultivateSystem = CarCultivateSystem.Instance;
        if (carCultivateSystem.Data != null)
        {
            var now = GameEntry.OfflineManager.GetServerDateTime();
            var finishTime = TimeUtils.GetDateTimeByMilliseconds(carCultivateSystem.Data.SpeedUpEndTime);
            var diff = TimeUtils.GetDiff(now, finishTime);
            var speedup = false;
            if (diff.TotalSeconds > 0)
            {
                rate = GameEntry.Table.Param.Get("EarningsSpeedMultiplier").IntParam;
                speedup = true;
            }
            GameEntry.GUI.SetParam(GameEntry.GUIPath.SelectRoleUI.Path, SelectRoleUI.SpeedUpMode, speedup);
            _addValue = Mathf.FloorToInt(cardata.ProductSeconds * rate);
            if (_speedUpMode != speedup)
            {
                EventManager.Instance.Dispatch(EventDefine.Global.OnCarCultivateChanged);
            }
            _speedUpMode = speedup;
        }
        int addValue = 0;
        var tpl = GameEntry.Table.CarUpgrade.Get(CarCultivateSystem.Instance.GetModuleLevel(CarModuleType.Module1));
        addValue += tpl.ProductSeconds1;
        tpl = GameEntry.Table.CarUpgrade.Get(CarCultivateSystem.Instance.GetModuleLevel(CarModuleType.Module2));
        addValue += tpl.ProductSeconds2;
        tpl = GameEntry.Table.CarUpgrade.Get(CarCultivateSystem.Instance.GetModuleLevel(CarModuleType.Module3));
        addValue += tpl.ProductSeconds3;
        tpl = GameEntry.Table.CarUpgrade.Get(CarCultivateSystem.Instance.GetModuleLevel(CarModuleType.Module4));
        addValue += tpl.ProductSeconds4;

        _addValue = Mathf.FloorToInt((cardata.ProductSeconds + addValue) * rate);

        PlayerSystem.Instance.AddGoldPool(_addValue);
        if (_cd > 0)
        {
            _cd -= 1f;
            return;
        }
        if (_nextUpgradeGold > 0 && PlayerSystem.Instance.GetGold() >= _nextUpgradeGold)
        {
            _carUpgrade = true;


            _cd = 5f;
            // Debug.LogError("upgrade");
        }
        if (_carUpgrade)
        {
            if (CarCultivateSystem.Instance.Data.IsAutoUpgradeCar && _hasNext)
            {
                GameEntry.Http.Handler.GetChangeDataSystemData();
                // Debug.LogError("send");
            }
        }
        */

    }
    private void updateInfo()
    {
        if (_nextUpgradeGold < 1)
        {
            var gold = long.MaxValue;

            var full = true;
            var cardata = GameEntry.Table.CarUpgrade.Get(CarLevel);
            var moduleLevel = CarCultivateSystem.Instance.GetModuleLevels();
            var len = moduleLevel.Count;
            for (var i = 0; i < len; i++)
            {
                var lv = moduleLevel[i];
                if (lv == CarLevel)
                {
                    full = false;
                    var cost = _moduleCostGold[i];
                    // if (cost < gold)
                    {
                        gold = cost;
                        _nextUpgradeType = i + 1;
                        if (i == 1)
                        {
                            gold += _moduleCostGold[2];
                        }
                        break;
                    }
                }
            }
            if (full)
            {
                // gold = cardata.Cost0;
                _nextUpgradeType = 0;
                //升级车
            }
            else
            {
                //升级部件
            }
            _nextUpgradeGold = gold;
            // Debug.LogError("next upgrade gold " + _nextUpgradeGold);
        }
    }
}