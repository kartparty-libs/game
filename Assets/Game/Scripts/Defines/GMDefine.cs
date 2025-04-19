using Framework;
using IngameDebugConsole;
using System;
using UnityEngine;

public class GMDefine
{
    public GMDefine()
    {
        DebugLogConsole.AddCommand<string>("setip", "自定义地址", setIp);
        DebugLogConsole.AddCommand<string, string>("setprefs", "添加设置", setPlayerPrefs);
        DebugLogConsole.AddCommand<string>("removeprefs", "删除设置", removePlayerPrefs);
        DebugLogConsole.AddCommand("connect", "连接钱包", connectWallet);
        DebugLogConsole.AddCommand("disconnect", "类型钱包", disconnectWallet);
        DebugLogConsole.AddCommand("okx", "okx钱包", connectokxWallet);
        DebugLogConsole.AddCommand<string>("rotation", "旋转", TestRotation);
        DebugLogConsole.AddCommand<int>("prerelease", "新版本", setPrelease);
        DebugLogConsole.AddCommand<int>("showlang", "提示", showMsgBox);
        DebugLogConsole.AddCommand("reload", "重新加载", reload);
        DebugLogConsole.AddCommand<string>("exe", "执行命令", exe);
        DebugLogConsole.AddCommand<string>("invite", "邀请人", invite);
#if UNITY_EDITOR
        DebugLogConsole.AddCommand("rank", "刷新排行", ReqBufferRank);
        DebugLogConsole.AddCommand<int>("robot", "执行机器人", ExecuteRobot);
        // DebugLogConsole.AddCommand<int>("additem", "添加道具", AddItem);
        DebugLogConsole.AddCommand<int, long>("gmaddmoney", "添加货币", (int id, long count) =>
        {
            GameEntry.Http.Handler.GMAddMoney(id, count);
        });
        DebugLogConsole.AddCommand<int, long>("gmadditem", "添加道具", (int id, long count) =>
       {
           GameEntry.Http.Handler.GMAddItem(id, count);
       });
        DebugLogConsole.AddCommand<int>("buy", "模拟购买", id =>
        {
            var account = GameEntry.Context.Account;
            if (string.IsNullOrEmpty(account))
            {
                return;
            }
            var tpl = GameEntry.Table.Shop.Get(id);
            // UserId + "," + id + "," + count + "," + amount + "," + buyOrderId + "," + buyType;
            var str = "JavaScriptBuySuccess," + account + "," + id + "," + "1" + "," + tpl.TonCost + "," + Guid.NewGuid().ToString() + ",Editor";
            JavascriptBridge.doCallFromJs("clear");
            JavascriptBridge.doCallFromJs("push" + account);
            JavascriptBridge.doCallFromJs("push" + id.ToString());
            JavascriptBridge.doCallFromJs("push" + "1");
            JavascriptBridge.doCallFromJs("push" + tpl.TonCost);
            JavascriptBridge.doCallFromJs("push" + Guid.NewGuid().ToString());
            JavascriptBridge.doCallFromJs("push" + "Editor");
            JavascriptBridge.doCallFromJs("call" + "JavaScriptBuySuccess");

        });

        DebugLogConsole.AddCommand("bindwallet", "绑定钱包", () =>
        {
            GameEntry.Http.Handler.BindWallet("0", "OKX Wallet");
        });
        DebugLogConsole.AddCommand<int,string>("invite", "邀请任务", (int id,string account) =>
        {
            GameEntry.Http.Handler.GetProtoClient().TestInvite(id,account);
        });
        DebugLogConsole.AddCommand<int>("gmaddgift", "礼包", (int id) =>
        {
            GameEntry.Http.Handler.GMAddGift(id);
        });
        DebugLogConsole.AddCommand<int>("gmrandomgift", "随机礼包", (int id) =>
        {
            GameEntry.Http.Handler.GMAddRandomGift(id);
        });
#endif
    }

    private void setPrelease(int obj)
    {
        PlayerPrefs.SetInt(GameDefine.PreRelease, obj);
    }

    private void setIp(string ip)
    {
        PlayerPrefs.SetString("ip", ip);
        Debug.LogWarning("自定义服务器" + ip);
    }
    private void setPlayerPrefs(string key, string value)
    {
        var k = key.Trim();
        if (string.IsNullOrEmpty(k))
        {
            return;
        }
        var v = value.Trim();
        if (string.IsNullOrEmpty(v))
        {
            Debug.LogWarning("删除设置" + key);
            PlayerPrefs.DeleteKey(k);
        }
        else
        {
            Debug.LogWarning("添加设置" + key + " : " + v);
            PlayerPrefs.SetString(k, v);
        }

    }
    private void removePlayerPrefs(string key)
    {
        var k = key.Trim();
        if (string.IsNullOrEmpty(k))
        {
            return;
        }
        Debug.LogWarning("删除设置" + key);
        PlayerPrefs.DeleteKey(k);

    }
    private void ReqBufferRank()
    {
        GameEntry.Net.Send(CtoS.K_ReqBufferRankTest);
    }
    private void ExecuteRobot(int type)
    {
        GameEntry.Net.Send(CtoS.K_ReqTastExecuteRobot, type);
    }
    private void TestRotation(string obj)
    {
        GameEntry.Context.EnableRotation = obj == "1";
        GameEntry.GUI.ScreenInfo.Dirty = true;
    }
    private void connectokxWallet()
    {
        JavascriptBridge.Call(JavascriptBridge.UnityConnectOkxWallet);
    }
    private void connectWallet()
    {
        JavascriptBridge.Call(JavascriptBridge.UnityConnectWallet);
    }
    private void disconnectWallet()
    {
        JavascriptBridge.Call(JavascriptBridge.UnityDisconnectWallet);
    }
    private void showMsgBox(int id)
    {
        GameEntry.GUI.Open(GameEntry.GUIPath.MsgBoxTipUI.Path, "msg", id);
    }
    private void reload()
    {
        JavascriptBridge.Call(JavascriptBridge.UnityReload);
    }
    private void exe(string arg)
    {
        var lst = arg.Split(" ");
        JavascriptBridge.Call(JavascriptBridge.UnityExecute, lst);
    }
    private void invite(string arg)
    {
        GameEntry.Context.InviteAccount = arg;
    }
}
