
using Framework;
using Framework.Core;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class SelectRoleUI : UIWindowBase
{
    public const string PlayShowAni = nameof(PlayShowAni);
    public const string SpeedUpMode = nameof(SpeedUpMode);
    public const string ShowTip1 = nameof(ShowTip1);
    private Dictionary<int, SelectRoleProperty> properties = new Dictionary<int, SelectRoleProperty>();
    private List<SelectRoleModule> modules = new List<SelectRoleModule>();
    private int _selectCar;
    private int _selectRole;
    private int _latestScore = -1;

    private const float MapWidth = 80f;
    private List<CarPartUpgradeCell> carParts = new List<CarPartUpgradeCell>();
    private MapSelectItem SelectMapItem;
    private List<MapSelectItem> _allMapItem = new List<MapSelectItem>();
    private List<UIItemNode> _partInfos;
    private int _showAddValue;
    protected override void OnAwake()
    {
        base.OnAwake();
        GameEntry.Context.CollectMoneyTarget = this.item0Icon_RectTransform;
        GameEntry.RedDot.Bind(shop_Button.transform.GetChild(0), RedDotName.FreeShopBuy);
        GameEntry.RedDot.Bind(giftBtn_Button.transform.GetChild(0), RedDotName.TaskAll);
        GameEntry.RedDot.Bind(race_Button.transform.GetChild(0), RedDotName.NewMap);
        GameEntry.RedDot.Bind(stake_Button.transform.GetChild(0), RedDotName.OKXTask);
        GameEntry.RedDot.Bind(turntable_Button.transform.GetChild(0), RedDotName.OKXTask);
        GameEntry.RedDot.Bind(turntable_Button.transform.GetChild(1), RedDotName.TurntableTask);
        GameEntry.RedDot.Bind(stake_Button.transform.GetChild(1), RedDotName.OKXTaskSuccess);
        GameEntry.RedDot.Bind(turntable_Button, RedDotName.TurntableEntry);
        GameEntry.Context.SelectRoleId = 1;
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        GameEntry.GUIEvent.AddEventListener(GUIEvent.OnHeadChange, onHeadChanged);
        GameEntry.GUIEvent.AddEventListener(GUIEvent.OnPlayerInfoChanged, OnPlayerInfoChanged);
        EventManager.Instance.Regist(EventDefine.Global.OnChangeScore, updateScore);
        EventManager.Instance.Regist(EventDefine.Global.OnChangeTaskData, updateTaskData);
        EventManager.Instance.Regist(EventDefine.Global.OnGoldChanged, updateGold);
        EventManager.Instance.Regist(EventDefine.Global.OnCarCultivateChanged, OnCarCultivateChanged);
        EventManager.Instance.Regist(EventDefine.Global.OnEnergyChanged, OnEnergyChanged);
        EventManager.Instance.Regist(EventDefine.Global.OnSeasonSystemChanged, OnSeasonSystemChanged);
        this.nickName_TextMeshProUGUI.text = PlayerSystem.Instance.GetName();
        _selectCar = 0;
        updateRoleShow();
        updateCarShow();
        //GameEntry.GUI.Open(GameEntry.GUIPath.NicknameUI.Path);
        var head = PlayerSystem.Instance.GetHead();
        var hdata = GameEntry.Table.Head.Get(head);
        //GameEntry.Atlas.SetSprite()

        updateScore();
        updateTaskData();
        updateGold();
        doCultivateChanged();
        OnSeasonSystemChanged();
        // SelectMapItem.Init(_competitionMaps[_selectMapIndex], 0);


        GameEntry.Context.SelectCarId = _selectCar + 1;
        Utils.Unity.SetActive(lvup_GameObject, false);
        if (StorageManager.Instance.HasKey("SelectRoleId"))
        {
            GameEntry.Context.SelectRoleId = StorageManager.Instance.GetInt("SelectRoleId");
        }
        else
        {
            GameEntry.Context.SelectRoleId = _selectRole + 1;
        }
        _selectRole = GameEntry.Context.SelectRoleId - 1;
        GameEntry.Context.SelectRoleSceneConfig.HidePart();
        GameUtils.CheckNewMap();
        OnEnergyChanged();
        this.turntabletime_TextMeshProUGUI.text = "";
        this.item2Count_TextMeshProUGUI.text = EnergySystem.Instance.GetEnergy();
    }

    private void OnEnergyChanged()
    {
        this.item2Count_TextMeshProUGUI.text = EnergySystem.Instance.GetEnergy();
        Utils.Unity.SetActive(item2info1_TextMeshProUGUI.transform.parent, false && EnergySystem.Instance.Recovering);
        if (EnergySystem.Instance.Recovering)
        {
            item2info1_TextMeshProUGUI.text = EnergySystem.Instance.GetRecoverRemain() + "(+1)";
            item2info2_TextMeshProUGUI.text = EnergySystem.Instance.GetRecoverAllRemain() + "(MAX)";
        }

    }


    private void updateTaskData()
    {
    }

    private void updateScore()
    {
        var score = PlayerSystem.Instance.GetScore();
        if (_latestScore < 0)
        {
            // this.item0Count_TextMeshProUGUI.text = PlayerSystem.Instance.GetScore().ToString();
        }
        else
        {
            if (score != _latestScore)
            {
                GameEntry.GUI.SetParam(GameEntry.GUIPath.EffectUI.Path, "collectmoney");
            }
        }
        _latestScore = score;
    }
    private void updateGold()
    {
        this.item0Count_TextMeshProUGUI.text = FormatUtils.FormatMoney(PlayerSystem.Instance.GetGold());
        this.item1Count_TextMeshProUGUI.text = FormatUtils.FormatMoney(PlayerSystem.Instance.GetDiamond());
    }
    public override void SetParam(string name, object value)
    {
        base.SetParam(name, value);
        if (name == "matchstart")
        {
            GameEntry.GUI.Close(GameEntry.GUIPath.DailyTaskUI.Path);
        }
        else if (name == "matchcancel")
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.DailyTaskUI.Path);
        }
        else if (name == "updateScore")
        {
            // this.item0Count_TextMeshProUGUI.text = PlayerSystem.Instance.GetScore().ToString();
        }
        else if (name == "getflytargget")
        {
            GameEntry.Context.FlyTarget = this.item0Icon_RectTransform;
        }
    }
    protected override void OnFocus()
    {
        base.OnFocus();
        // this.lv_GameObject.SetActive(true);
        onHeadChanged(null);
    }
    private void onHeadChanged(IEventData eventData)
    {
        var head = PlayerSystem.Instance.GetHead();
        var hdata = GameEntry.Table.Head.Get(head);
        GameEntry.Atlas.SetSprite(this.changeIcon_Image, hdata.TinyIcon, false, true);
    }

    protected override void OnClose()
    {
        base.OnClose();
        // GameEntry.GUIEvent.RemoveEventListener(GUIEvent.SelectRolePropertyChanged, onSelectRolePropertyChanged);
        GameEntry.GUIEvent.RemoveEventListener(GUIEvent.OnHeadChange, onHeadChanged);
        GameEntry.GUIEvent.RemoveEventListener(GUIEvent.OnPlayerInfoChanged, OnPlayerInfoChanged);
        EventManager.Instance.Remove(EventDefine.Global.OnChangeScore, updateScore);
        EventManager.Instance.Remove(EventDefine.Global.OnChangeTaskData, updateTaskData);
        EventManager.Instance.Remove(EventDefine.Global.OnGoldChanged, updateGold);
        EventManager.Instance.Remove(EventDefine.Global.OnCarCultivateChanged, OnCarCultivateChanged);
        EventManager.Instance.Remove(EventDefine.Global.OnEnergyChanged, OnEnergyChanged);
        EventManager.Instance.Remove(EventDefine.Global.OnSeasonSystemChanged, OnSeasonSystemChanged);

    }

    private void OnPlayerInfoChanged(IEventData eventData)
    {
        this.nickName_TextMeshProUGUI.text=PlayerSystem.Instance.GetName();
    }

    private void OnSeasonSystemChanged()
    {

    }

    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.roleBtn_Button)
        {
            _selectRole++;
            updateRoleShow();
            GameEntry.Context.SelectRoleId = _selectRole + 1;
            StorageManager.Instance.SetInt("SelectRoleId", GameEntry.Context.SelectRoleId);
            PlayerSystem.Instance.SetPlayerCfgId(GameEntry.Context.SelectRoleId);
        }
        else if (target == carBtn_Button)
        {
            _selectCar++;
            updateCarShow();
            GameEntry.Context.SelectCarId = _selectCar + 1;
            PlayerSystem.Instance.SetCarCfgId(GameEntry.Context.SelectCarId);
        }
        else if (target == this.changeAction_Button)
        {
            startRoleAction();
        }
        else if (target == this.changeIcon_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.SettingUI.Path);
            // GameEntry.GUI.Open(GameEntry.GUIPath.SelectHeadUI.Path);
        }
        else if (target == this.giftBtn_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.NewTaskUI.Path);
        }
        else if (target == this.rankBtn_Button)
        {
            ExecuteAction.ByActionType(ExecuteActionType.Rank);
        }
        else if (target == this.trophyBtn_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.OpenBoxUI.Path);
        }

        else if (target == stake_Button)
        {
            JumpUtils.Open(4);
        }
        else if (target == shop_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.ShopUI.Path);
        }
        else if (target == item0add_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.OpenBoxUI.Path);
        }
        else if (target == item1add_Button)
        {
            JumpUtils.GetMoney(1);
        }

        else if (target == this.race_Button)
        {
            GameEntry.Http.Handler.SetRoleId(GameEntry.Context.SelectRoleId);
            GameEntry.Http.Handler.SetCarId(GameEntry.Context.SelectCarId);
            // GameEntry.GUI.Open(GameEntry.GUIPath.SelectMapUI.Path);
            GameEntry.GUI.Open(GameEntry.GUIPath.SelectMapTypeUI.Path);
        }
        else if (target == okx_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.OKXUI.Path);
            // JavascriptBridge.Call(JavascriptBridge.UnityConnectOkxWallet);
        }
        else if (target == change_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.ChangeCodeUI.Path);
        }
        else if (target == this.item2add_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.BuyEnergyUI.Path);
        }
        else if (target == this.turntable_Button)
        {
            // if (!TaskSystem.Instance.IsOkxFinished())
            // {
            //     GameEntry.GUI.Open(GameEntry.GUIPath.MsgBoxTipUI.Path, "msg", 1811);
            //     return;
            // }
            GameEntry.GUI.Open(GameEntry.GUIPath.TurntableUI.Path);
        }
        else if (target == this.pve_Button)
        {
            GameEntry.Context.MatchMode = false;
            ExecuteAction.ByActionType(ExecuteActionType.Map_Pve);
        }
        else if (target == this.pvp_Button)
        {

            ExecuteAction.ByActionType(ExecuteActionType.Map_Pvp);
        }
        else if (target == this.paihang_Button)
        {
            ExecuteAction.ByActionType(ExecuteActionType.Rank);
        }
        else if (target == this.assets_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.MyAssetsUI.Path);
        }else if (target == this.invite_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.InviteFriendInfoUI.Path);
        }
        else if (target == this.garage_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.GarageUI.Path);
        }
        else if (target == this.shequ_Button)
        {
            var p = GameEntry.Table.Param.Get("HomeLink").TextParam;
            if (!string.IsNullOrEmpty(p))
            {
                JumpUtils.OpenLink(p);
            }
        }
        else if (target == this.saishi_Button)
        {
            var p = GameEntry.Table.Param.Get("NewsLink").TextParam;
            if (!string.IsNullOrEmpty(p))
            {
                JumpUtils.OpenLink(p);
            }
        }
        else if (target == this.yugao_Button)
        {
            var p = GameEntry.Table.Param.Get("SeasonLink").TextParam;
            if (!string.IsNullOrEmpty(p))
            {
                JumpUtils.OpenLink(p);
            }
        }
        else if (target == this.kmoney_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.AboutKUI.Path);
        }
    }
    private void updateCarShow()
    {
        var config = GameEntry.Context.SelectRoleSceneConfig;
        if (config == null)
        {
            return;
        }
        _selectCar = 0;
        var lv = CarCultivateSystem.Instance.GetCarLevel();
        var paramLv = GameEntry.Table.Param.Get("KartAdvancedLV").IntParams;
        var len = paramLv.Length;
        for (var i = 0; i < len; i++)
        {
            var plv = paramLv[i];
            if (lv >= plv)
            {
                _selectCar = i + 1;
            }
        }
        var lst = config.Cars;
        _selectCar %= lst.Count;
        if (_selectCar < 0)
        {
            _selectCar = 0;
        }
        len = lst.Count;
        while (len-- > 0)
        {
            var go = lst[len];
            Utils.Unity.SetActive(go, len == _selectCar);
        }
        GameEntry.Context.SelectCarId = _selectCar + 1;
    }
    private void updateRoleShow()
    {
        var config = GameEntry.Context.SelectRoleSceneConfig;
        if (config == null)
        {
            return;
        }
        var lst = config.Roles;
        _selectRole %= lst.Count;
        if (_selectRole < 0)
        {
            _selectRole += lst.Count;
        }
        GameEntry.Context.SelectRoleId = _selectRole + 1;
        var len = lst.Count;
        while (len-- > 0)
        {
            var go = lst[len];
            go.SetActive(len == _selectRole);
        }

    }

    private void startRoleAction()
    {
        var config = GameEntry.Context.SelectRoleSceneConfig;
        if (config == null)
        {
            return;
        }
        var lst = config.Roles;
        _selectRole %= lst.Count;
        var go = lst[_selectRole];
        var ani = go.GetComponent<RoleRandAni>();
        ani.playRelaxation();
    }
    protected override void OnUpdateSeconds()
    {
        base.OnUpdateSeconds();
        doCultivateChanged();
        if (collectShowParam == null)
        {
            collectShowParam = new List<ItemConfig>();
            var param = GameEntry.Table.Param.Get("FlyMoneyParams").TextParam;
            var lst = param.Split("|");
            var len = lst.Length;
            for (var i = 0; i < len; i++)
            {
                var kv = lst[i].Split(",");
                if (kv.Length == 2)
                {
                    var cfg = new ItemConfig();
                    cfg.ItemId = int.Parse(kv[0]);
                    cfg.ItemCount = int.Parse(kv[1]);
                    collectShowParam.Add(cfg);
                }
            }
        }

        if (EnergySystem.Instance.Recovering)
        {
            item2info1_TextMeshProUGUI.text = EnergySystem.Instance.GetRecoverRemain() + "(+1)";
            item2info2_TextMeshProUGUI.text = EnergySystem.Instance.GetRecoverAllRemain() + "(MAX)";
        }
        var now = GameEntry.OfflineManager.GetServerDateTime();
        var turntableEndTime = TimeUtils.GetDateTimeByMilliseconds(LuckyTurntableSystem.Instance.Data.EndTime);
        var diff = TimeUtils.GetDiff(now, turntableEndTime);
        if (diff.TotalSeconds > 0)
        {
            this.turntabletime_TextMeshProUGUI.text = TimeUtils.GetRemainder(diff);
        }
        else
        {
            this.turntabletime_TextMeshProUGUI.text = "";
        }

        var d = SeasonSystem.Instance.Data;
        if (SeasonSystem.Instance.IsStarted())
        {
            this.pvpstate_TextMeshProUGUI.text = TimeUtils.GetRemainder(SeasonSystem.Instance.GetTimeToEnd());
        }
        else
        {
            if (true)
            {
                this.pvpstate_TextMeshProUGUI.text = GameEntry.Table.Lang.GetText(3116);
            }
            else
            {
                var toStartTime = SeasonSystem.Instance.GetTimeToStart();
                if (toStartTime.TotalSeconds > 0)
                {
                    this.pvpstate_TextMeshProUGUI.text = ">" + TimeUtils.GetRemainder(toStartTime);
                }
                else
                {
                    var toendTime = SeasonSystem.Instance.GetTimeToEnd();
                    if (toendTime.TotalSeconds < 0)
                    {
                        this.pvpstate_TextMeshProUGUI.text = GameEntry.Table.Lang.GetText(3116);
                    }
                }
            }
        }
    }
    private List<ItemConfig> collectShowParam;
    private void doCultivateChanged()
    {

    }
    private void OnCarCultivateChanged()
    {
        doCultivateChanged();


    }

}