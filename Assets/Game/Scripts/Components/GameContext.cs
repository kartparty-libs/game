using Framework;
using Framework.Core;
using Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class GameContext
{
    public bool OfflineMode = true;
    public GameConfig GameConfig;
    public RectTransform FlyStart;
    public RectTransform FlyTarget;
    public RectTransform CollectMoneyTarget;
    public MainCharacter FollowPlayer;
    public PlayerInput PlayerInput;
    public GameplayBase Gameplay;
    public bool LoginSuccess;
    public bool LoadingShow;
    public int SelectCarId;
    public int SelectRoleId;
    public int SelectMapId;
    public List<int> SelectModules = new List<int>();
    public List<CarMoudleProperty> Propertys = new List<CarMoudleProperty>();
    private Dictionary<int, int> selectRolePropertyList = new Dictionary<int, int>();
    public SelectRoleSceneConfig SelectRoleSceneConfig;
    public SceneConfig SceneConfig;
    public MatchConfig MatchConfig;
    public EnumDefine.CompetitionMapState ServerCompetitionMapState = EnumDefine.CompetitionMapState.None;
    public bool EnterMain;
    public bool NeedRegister;
    public string NickName = "";
    public string SdkNickName = "";
    public string Account;
    public string InviteAccount = "";
    public string KartKey = "";
    public string BindMailName = "";
    public bool NeedCreateHead;
    public string TG_Platform = "";
    public string ImmutablePassportAddress = "";
    //更新中
    public bool UpgradeContent;
    public PlatformEnum Platform
    {
        get
        {
#if UNITY_EDITOR
            return PlatformEnum.Editor;
#endif
            return PlatformEnum.Imx;
        }
    }
    //tg平台为initdata
    public string PlatformInfo = "";
    public bool DebugMode
    {
        get
        {
            return PlayerPrefs.HasKey("debug") && PlayerPrefs.GetInt("debug") == 1;
        }
    }
    public bool IsHasNetWorkCharacter = false;
    public Offline Offline { get; private set; } = new Offline();
    public Vector2 CameraInputValue;
    public bool EnableRotation = true;
    public WalletInfo BindWallet;
    public bool MatchMode;
    public bool BackToSelectMap;
    public void Init(SceneConfig config)
    {

        SceneConfig = config;
        var tpl = GameEntry.Table.Map.Get(GameEntry.Context.SelectMapId);
        SceneConfig.AISpeedMin = tpl.AISpeedMin;
        SceneConfig.AISpeedMax = tpl.AISpeedMax;
        GameConfig.MainCamera.gameObject.SetActive(config.MainCamera == null);
        if (config.MainCamera != null)
        {
            var list = config.MainCamera.GetUniversalAdditionalCameraData().cameraStack;
            if (!list.Contains(GameConfig.UICamera))
            {
                list.Add(GameConfig.UICamera);
            }
        }
    }
    public void AddCarModule(int id)
    {
        if (SelectModules.Contains(id)) return;
        SelectModules.Add(id);
        var data = GameEntry.Table.CarModules.Get(id);
        foreach (var item in data.Propertys)
        {
            if (selectRolePropertyList.TryGetValue(item.Id, out int value))
            {
                selectRolePropertyList[item.Id] = value + item.Value;
            }
            else
            {
                selectRolePropertyList.Add(item.Id, item.Value);
            }
        }
        foreach (var item in Propertys)
        {
            ReferencePool.Release(item);
        }
        Propertys.Clear();
        foreach (var item in selectRolePropertyList)
        {
            var pid = item.Key;
            var pvalue = item.Value;
            var carp = ReferencePool.Get<CarMoudleProperty>();
            carp.Id = pid;
            carp.Value = pvalue;
            Propertys.Add(carp);
        }
        GameEntry.GUIEvent.DispatchEvent(GUIEvent.SelectRolePropertyChanged);
        //GameEntry.GUI.SetParam(GameEntry.GUIPath.SelectRoleUI, GUIEvent.SelectRolePropertyChanged);
    }
    public void RemoveCarModule(int id)
    {
        SelectModules.Remove(id);
        var data = GameEntry.Table.CarModules.Get(id);
        foreach (var item in data.Propertys)
        {
            if (selectRolePropertyList.TryGetValue(item.Id, out int value))
            {
                selectRolePropertyList[item.Id] = value - item.Value;
            }
        }
        foreach (var item in Propertys)
        {
            ReferencePool.Release(item);
        }
        Propertys.Clear();
        foreach (var item in selectRolePropertyList)
        {
            var pid = item.Key;
            var pvalue = item.Value;
            var carp = ReferencePool.Get<CarMoudleProperty>();
            carp.Id = pid;
            carp.Value = pvalue;
            Propertys.Add(carp);
        }
        GameEntry.GUIEvent.DispatchEvent(GUIEvent.SelectRolePropertyChanged);
        //GameEntry.GUI.SetParam(GameEntry.GUIPath.SelectRoleUI, GUIEvent.SelectRolePropertyChanged);
    }
    public void Update(float deltaTime)
    {

    }
}
