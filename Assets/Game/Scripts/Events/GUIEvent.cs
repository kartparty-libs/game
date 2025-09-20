using Framework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIEvent : IEventData
{
    public const string SelectRolePropertyChanged = nameof(SelectRolePropertyChanged);
    public const string OnLoadingHide = nameof(OnLoadingHide);
    public const string OnRoomPlayerInfoChange = nameof(OnRoomPlayerInfoChange);
    public const string OnStartMateCompeComplete = nameof(OnStartMateCompeComplete);
    public const string OnHeadChange = nameof(OnHeadChange);
    public const string OnTrophyItemSelect = nameof(OnTrophyItemSelect);
    public const string OnModuleSelect = nameof(OnModuleSelect);
    public const string OnSdkLogin = nameof(OnSdkLogin);
    public const string OnErrorCode = nameof(OnErrorCode);
    public const string OnShopBuyItem = nameof(OnShopBuyItem);
    public const string FlyEffecct = nameof(FlyEffecct);
    public const string FlyEffecctOne = nameof(FlyEffecctOne);
    public const string FlyEffecctOneFinish = nameof(FlyEffecctOneFinish);
    public const string NeedRegister = nameof(NeedRegister);
    public const string OnWalletConnect = nameof(OnWalletConnect);
    public const string OnWalletDisconnect = nameof(OnWalletDisconnect);
    public const string OnSdkBuySuccess = nameof(OnSdkBuySuccess);
    public const string OnSdkBuyFail = nameof(OnSdkBuyFail);
    public const string OnChangeCodeResult = nameof(OnChangeCodeResult);
    public const string OnReadClipboard = nameof(OnReadClipboard);
    public const string OnInviteSystemChanged = nameof(OnInviteSystemChanged);
    public const string SetGuideTarget= nameof(SetGuideTarget);
    public const string StartGuide= nameof(StartGuide);
    public const string OnPlayerInfoChanged= nameof(OnPlayerInfoChanged);
    public string EventType { get; set; }
    public bool IsReferenceActive { get; set; }
    public int IntValue;
    public string StringValue;
    public Vector3 Vector3Value;
    public float FloatValue;
    public RectTransform Object1;
    public RectTransform Object2;
    public int Count;
    public float RandomValue;
    public object UserData;
    public List<string> StringValues = new List<string>();
    public void Clear()
    {
    }
    public void Reset()
    {
        StringValue = null;
        Vector3Value = Vector3.zero;
        FloatValue = 0f;
        RandomValue = 0f;
    }
}
