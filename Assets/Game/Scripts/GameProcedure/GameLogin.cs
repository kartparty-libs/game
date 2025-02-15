using Framework;
using UnityEngine;
using System.Collections;
using Framework.Core;
using System;
using System.Collections.Generic;
public class GameLogin : FsmStateAction
{
    private bool _inited;
    private Dictionary<string, List<string>> appStartParam = new Dictionary<string, List<string>>();
    protected override void OnEnter()
    {
        base.OnEnter();
        GameEntry.Context.EnterMain = false;
        GameEntry.Context.PlatformInfo = "";
        GameEntry.GUIEvent.AddEventListener(GUIEvent.OnSdkLogin, OnSdkLogin);
        GameEntry.GUIEvent.AddEventListener(GUIEvent.NeedRegister, NeedRegister);
        JavascriptBridge.Call(JavascriptBridge.UnityPreRelease, GameDefine.IsPreRelease() ? "1" : "0");
        GameEntry.GUI.Open(GameEntry.GUIPath.LoginUI.Path);
        GameEntry.GUI.Open(GameEntry.GUIPath.GuideUI.Path);
        GameEntry.Coroutine.Start(check());
        if (!PlayerPrefs.HasKey("UI_InviteFriend"))
        {
            GameEntry.RedDot.Set(RedDotName.Action_Check_Invite, "", true);
            GameEntry.RedDot.Set(RedDotName.IconInvateFriendInfo, "", true);
        }

    }

    private void NeedRegister(IEventData eventData)
    {
        GameEntry.Http.Handler.Register(GameEntry.Context.Account, "", "", GameUtils.GetRandomName(), GameEntry.Context.InviteAccount);
    }

    private void OnSdkLogin(IEventData eventData)
    {
        try
        {
            if (eventData is GUIEvent e)
            {
                var args = e.StringValues;
                //account,address,email,nickname,startapp
                if (args.Count < 4)
                {
                    Debug.LogError("info error");
                    foreach (var item in args)
                    {
                        Debug.LogError(item);
                    }
                    GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "error", "login info error");
                    return;
                }
                var userid = args[0];
                string address = args[1];
                string platform = args[2];
                string nickname = args[3];
                if (args.Count > 4)
                {
                    ParseStartParam(args[4]);
                }
                if (string.IsNullOrEmpty(userid))
                {
                    GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "error", 3);
                    return;
                }
                if (GameEntry.Context.DebugMode)
                {
                    GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", userid);
                }
                var startargs = GetStartParam("s");
                if (startargs != null)
                {
                    var inviteAccount = "";
                    if (startargs.Count == 1)
                    {
                        inviteAccount = startargs[0];
                    }
                    if (string.IsNullOrEmpty(inviteAccount))
                    {
                        inviteAccount = "";
                    }
                    GameEntry.Context.InviteAccount = inviteAccount;
                }
                if (string.IsNullOrEmpty(nickname))
                {
                    nickname= GameUtils.GetRandomName();
                }
                GameEntry.Context.Account = userid;
                GameEntry.Context.TG_Platform = platform;
                GameEntry.Context.ImmutablePassportAddress=address;
                var info = new UserInfo();
                info.Address = userid;
                info.Sub = "";
                info.Email = platform;
                info.NickName = nickname;
                GameEntry.Context.PlatformInfo = "";
                Debug.Log("OnSdkLogin " + userid + "   " + GameEntry.Context.PlatformInfo);
                GameEntry.Context.NickName = nickname;
                // GameEntry.Context.SdkNickName = nickname;
                GameEntry.GUI.SetParam(GameEntry.GUIPath.LoginUI.Path, LoginUI.HasAccount, userid);
                var upgrading = false;
                if (GameEntry.Context.UpgradeContent)
                {
                    upgrading = true;
                    if (PlayerPrefs.GetString("login") == "test")
                    {
                        upgrading = false;
                    }
                }
                if (upgrading)
                {
                    //停服了
                    GameEntry.GUI.Open(GameEntry.GUIPath.OfflineUI.Path);
                    return;
                }
                GameEntry.OfflineManager.Login(userid);
            }
        }
        catch (System.Exception e)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "error", e.ToString());
        }

    }

    protected override void OnExit()
    {
        base.OnExit();
        GameEntry.GUIEvent.RemoveEventListener(GUIEvent.OnSdkLogin, OnSdkLogin);
        XiaoZhiAction.Speak("来了老弟！检测到好友“闪电侠”在线，今天是复仇还是赢一把就睡？");
    }
    IEnumerator check()
    {
        GameEntry.GUI.Close(GameEntry.GUIPath.RankUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.NewRankUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.SelectHeadUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.DailyTaskUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.BindMailUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.MainUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.MatchUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.LoadingUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.SelectRoleUI.Path);
        GameEntry.GUI.Close(GameEntry.GUIPath.NicknameUI.Path);
        yield return null;

        while (!GameEntry.GUI.IsOpen(GameEntry.GUIPath.LoginUI.Path))
        {
            yield return null;
        }
        GameEntry.HideDefault();
        GameEntry.GUI.PreLoad(GameEntry.GUIPath.SelectRoleUI.Path);
        if (!_inited)
        {
            _inited = true;
            JavascriptBridge.Call(JavascriptBridge.UnityInit);
        }
        while (!GameEntry.Context.EnterMain)
        {
            yield return null;
        }
        GameEntry.GUI.ScreenInfo.Dirty = true;
        GameEntry.GUI.Open(GameEntry.GUIPath.LoadingUI.Path);
        while (!GameEntry.GUI.IsOpen(GameEntry.GUIPath.LoadingUI.Path))
        {
            yield return null;
        }
        this.Finished = true;
    }
    private void ParseStartParam(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }
        appStartParam = new Dictionary<string, List<string>>();
        var lst = value.Split("_");
        var len = lst.Length;
        List<string> args = null;
        for (var i = 0; i < len; i++)
        {
            var p = lst[i];
            if (p.Length == 2)
            {
                if (p.StartsWith("f"))
                {
                    args = new List<string>();
                    var k = p.Substring(1);
                    if (!appStartParam.ContainsKey(k))
                    {
                        appStartParam.Add(k, args);
                    }
                    continue;
                }
            }
            if (args == null)
            {
                continue;
            }
            args.Add(p);
        }
    }
    private List<string> GetStartParam(string name)
    {
        appStartParam.TryGetValue(name, out var args);
        return args;
    }
    internal class UserInfo
    {
        public string Address;
        public string Sub;
        public string Email;
        public string NickName;
    }
}