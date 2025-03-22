using System.Collections.Generic;
using Framework.Core;
using UnityEngine;

public class JavascriptBridge : MonoBehaviour
{
    public const string UnityInit = nameof(UnityInit);
    public const string UnityPlayerAccount = nameof(UnityPlayerAccount);
    public const string UnityLogin = nameof(UnityLogin);
    public const string UnityBuy = nameof(UnityBuy);
    public const string UnityConnectWallet = nameof(UnityConnectWallet);
    public const string UnityDisconnectWallet = nameof(UnityDisconnectWallet);
    public const string UnityConnectOkxWallet = nameof(UnityConnectOkxWallet);
    public const string UnityOpenLink = nameof(UnityOpenLink);
    public const string UnityOpenLinkByType = nameof(UnityOpenLinkByType);
    public const string UnityReload = nameof(UnityReload);
    public const string UnityPreRelease = nameof(UnityPreRelease);
    public const string UnityShare = nameof(UnityShare);
    public const string UnityExecute = nameof(UnityExecute);
    public const string UnityReadClipboard = nameof(UnityReadClipboard);


    public const string JavaScriptLoginInfo = nameof(JavaScriptLoginInfo);
    public const string JavaScriptConnectWallet = nameof(JavaScriptConnectWallet);
    public const string JavaScriptDisconnectWallet = nameof(JavaScriptDisconnectWallet);
    public const string JavaScriptBuySuccess = nameof(JavaScriptBuySuccess);
    public const string JavaScriptBuyfail = nameof(JavaScriptBuyfail);
    public const string JavaScriptShowMsg = nameof(JavaScriptShowMsg);
    public const string JavaScriptSetRotation = nameof(JavaScriptSetRotation);
    public const string JavaScriptShowLangIdMsg = nameof(JavaScriptShowLangIdMsg);
    public const string JavaScriptUpgrade = nameof(JavaScriptUpgrade);
    public const string JavaScriptReadClipboard = nameof(JavaScriptReadClipboard);
    public const string JavaScriptSetNftCount = nameof(JavaScriptSetNftCount);
    private static JavascriptBridge instance;
    private JavaScript jsTarget;
    private static List<string> jsArgs = new List<string>();

    public void SetTarget(JavaScript value)
    {
        if (value == null)
        {
            return;
        }
        this.jsTarget = value;
        jsTarget.OnJavascriptCall = CallFromJs;
    }
    private void Awake()
    {
        instance = this;
    }
    public void CallFromJs(string value)
    {
        try
        {
            doCallFromJs(value);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    public static void doCallFromJs(string value)
    {
        if (value.StartsWith("push"))
        {
            var v = value.Substring(4);
            jsArgs.Add(v);
            return;
        }
        if (value.StartsWith("clear"))
        {
            jsArgs.Clear();
            return;
        }
        if (value.StartsWith("call"))
        {
            var v = value.Substring(4);
            var args = new List<string>();
            args.AddRange(jsArgs);
            jsArgs.Clear();
            try
            {
                _doCallFromJs(v, args);
            }
            catch (System.Exception e)
            {
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "error", "exe " + v + ":" + e.ToString());
            }

            return;
        }
    }
    private static void _doCallFromJs(string name, List<string> param)
    {
        if (name == JavaScriptLoginInfo)
        {
            //account,InitDataRaw,Platform,nickname
            var e = ReferencePool.Get<GUIEvent>();
            e.StringValues.Clear();
            e.StringValues.AddRange(param);
            e.EventType = GUIEvent.OnSdkLogin;
            GameEntry.GUIEvent.DispatchEvent(e);
        }
        else if (name == "server")
        {
            GameEntry.Http.UpdateServerInfo(1, param[0]);
        }
        else if (name == JavaScriptConnectWallet)
        {
            //walletInfo.account.address + "," + walletInfo.name + "," + walletInfo.imageUrl + "," + walletInfo.account.chain
            var e = ReferencePool.Get<GUIEvent>();
            var info = new WalletInfo();
            info.Address = param[0];
            info.Name = param[1];
            info.ImageUrl = param[2];
            info.Chain = param[3];
            GameEntry.Context.BindWallet = info;
            e.StringValues.Clear();
            e.StringValues.AddRange(param);
            e.EventType = GUIEvent.OnWalletConnect;
            GameEntry.GUIEvent.DispatchEvent(e);
            Debug.Log("walletInfo " + param[0] + " " + param[1]);
            if (param[1] == "OKX Wallet")
            {
                //完成任务
            }
            GameEntry.Http.Handler.BindWallet(param[0], param[1]);
        }
        else if (name == JavaScriptDisconnectWallet)
        {
            GameEntry.Context.BindWallet = null;
            var e = ReferencePool.Get<GUIEvent>();
            e.StringValues.Clear();
            e.EventType = GUIEvent.OnWalletDisconnect;
            GameEntry.GUIEvent.DispatchEvent(e);
        }
        else if (name == JavaScriptBuySuccess)
        {
            // var account = param[0];
            // var tplId = int.Parse(param[1]);
            // var count = int.Parse(param[2]);
            // var amount = param[3];
            // var order = param[4];
            // var buyType = param[5];
            // Debug.Log("buy " + order + " " + tplId + " " + buyType);
            // GameEntry.Http.Handler.ShopBuy(tplId, order, Proto.TransactionPlatformEnum.TransactionTg, buyType);
        }
        else if (name == JavaScriptBuyfail)
        {
            //buyPayload = UserId + "," + id + "," + count + "," + amount + "," + buyOrderId + "," + buyType;
            var e = ReferencePool.Get<GUIEvent>();
            e.StringValues.Clear();
            e.EventType = GUIEvent.OnSdkBuyFail;
            GameEntry.GUIEvent.DispatchEvent(e);
        }
        else if (name == JavaScriptShowMsg)
        {
            var info = param[0];
            GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "showmsg", info);
        }
        else if (name == JavaScriptSetRotation)
        {
            var info = param[0];
            GameEntry.Context.EnableRotation = info == "1";
        }
        else if (name == JavaScriptShowLangIdMsg)
        {
            var info = param[0];
            if (int.TryParse(info, out var id))
            {
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgBoxTipUI.Path, "showmsg", id);
            }
        }
        else if (name == JavaScriptUpgrade)
        {
            GameEntry.Context.UpgradeContent = true;
        }
        else if (name == JavaScriptReadClipboard)
        {
            var e = ReferencePool.Get<GUIEvent>();
            e.StringValue = param[0];
            e.EventType = GUIEvent.OnReadClipboard;
            GameEntry.GUIEvent.DispatchEvent(e);
        }else if(name== JavaScriptSetNftCount)
        {
            long.TryParse(param[0], out var count);
            ItemSystem.Instance.Set203ItemCount(count);
        }
    }
    public static void Call(string name, params string[] args)
    {
        if (instance == null || instance.jsTarget == null)
        {
            return;
        }
        instance.jsTarget.CallJavascript(name, args);
    }
}