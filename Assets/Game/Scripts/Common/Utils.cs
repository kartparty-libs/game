using System;
using System.Globalization;
using Framework;
using UnityEngine;
using UnityEngine.UI;

public class FormatUtils
{
    private static string[] suffixes = { "", "k", "m", "g", "t", "p" };
    public static string FormatMoney(long value)
    {
        if (value < 1000000)
        {
            return value.ToString();
        }
        double number = value;
        if (number == 0) return "0";
        // 确定使用哪个后缀  
        int suffixIndex = 0;
        while (number >= 1000000 && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000;
            suffixIndex++;
        }
        var decimalPlaces = Mathf.Max(suffixIndex, 0);
        var s = number.ToString();
        var dotid = s.IndexOf(".");
        if (dotid > 0)
        {
            var len = s.Length - dotid;
            if (len > 2)
            {
                len = 2;
            }
            s = s.Substring(0, dotid);// + s.Substring(dotid, len);
        }
        return $"{s}{suffixes[suffixIndex]}";
        // 格式化数字并添加后缀  
        return $"{number.ToString("F1")}{suffixes[suffixIndex]}";
    }
    private static int kRate = -1;
    public static string FormatKMoney(long value)
    {
        if (kRate == -1)
        {
            var tpl = GameEntry.Table.Item.Get(3);
            kRate = tpl.ShowRate;
            if (kRate < 1)
            {
                kRate = 1;
            }
        }
        double v = (double)value / (double)kRate;
        return v.ToString("0.###");
    }
}
public class TimeUtils
{
    public static DateTime GetServerTime()
    {
        return GameEntry.OfflineManager.GetServerDateTime();
    }
    public static DateTime GetDateTimeByMilliseconds(long value)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(value).DateTime;
    }
    public static TimeSpan GetDiff(DateTime from, DateTime to)
    {
        TimeSpan diff = to - from;
        return diff;
    }
    public static string GetRemainder(TimeSpan value, bool trim = false)
    {
        var h = value.Hours;
        if (value.Days > 0 && !trim)
        {
            return string.Format("{0}:{1:D2}:{2:D2}:{3:D2}", value.Days, value.Hours, value.Minutes, value.Seconds);
        }
        if (!trim || value.Hours > 0)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", value.Hours, value.Minutes, value.Seconds);
        }
        if (!trim || value.Minutes > 0)
        {
            return string.Format("{0:D2}:{1:D2}", value.Minutes, value.Seconds);
        }
        return string.Format("{0:D2}", value.Seconds);

    }
    public static string GetRemainder(long milliseconds)
    {
        return GetRemainder(TimeSpan.FromMilliseconds(milliseconds));
    }
    public static string GetDate(long value)
    {
        var dt = GetDateTimeByMilliseconds(value);
        return dt.ToString("MM/dd/HH/mm");
    }
    /// <summary>  
    /// 将日期时间字符串转换为UTC时间戳（毫秒）  
    /// </summary>  
    /// <param name="dateTimeString">日期时间字符串</param>  
    /// <param name="format">日期时间字符串的格式</param>  
    /// <returns>UTC时间戳（毫秒）</returns>  
    public static long ConvertToUtcTimestampMilliseconds(string dateTimeString, string format = "yyyyMMddHHmm")
    {
        try
        {
            if (dateTimeString.Length > 12)
            {
                dateTimeString = dateTimeString.Substring(0, 12);
            }
            if (DateTime.TryParseExact(dateTimeString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                TimeSpan timeSpan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return (long)timeSpan.TotalMilliseconds;
            }
            return 0;
        }
        catch (Exception e)
        {
            return 0;
        }
    }
}

public class JumpUtils
{
    public static void Open(int id)
    {
        switch (id)
        {
            case 1:
                GameEntry.GUI.Open(GameEntry.GUIPath.SelectMapUI.Path);
                GameEntry.GUI.Close(GameEntry.GUIPath.NewTaskUI.Path);
                break;
            case 3:
                GameEntry.GUI.Open(GameEntry.GUIPath.OpenBoxUI.Path);
                GameEntry.GUI.Close(GameEntry.GUIPath.NewTaskUI.Path);
                break;
            case 4:
                return;
                if (!TaskSystem.Instance.IsOkxFinished())
                {
                    GameEntry.GUI.Open(GameEntry.GUIPath.MsgBoxTipUI.Path, "msg", 1519);
                    return;
                }
                GameEntry.GUI.Open(GameEntry.GUIPath.StakeUI.Path);
                GameEntry.GUI.Close(GameEntry.GUIPath.NewTaskUI.Path);
                break;
            case 5:
                //商店
                GameEntry.GUI.Open(GameEntry.GUIPath.ShopUI.Path);
                GameEntry.GUI.Close(GameEntry.GUIPath.NewTaskUI.Path);
                break;
            case 6:
                //okx
                // GameEntry.GUI.Open(GameEntry.GUIPath.OKXUI.Path);
                JavascriptBridge.Call(JavascriptBridge.UnityConnectOkxWallet);
                break;
            case 2:
                GameEntry.GUI.SetParam(GameEntry.GUIPath.SelectRoleUI.Path, SelectRoleUI.ShowTip1);
                GameEntry.GUI.Close(GameEntry.GUIPath.NewTaskUI.Path);
                break;
            case 7:
                GameEntry.Http.Handler.FinishFollow();
                // JavascriptBridge.Call(JavascriptBridge.UnityOpenLinkByType, "followx");
                break;
            case 8:
                GameEntry.Http.Handler.FinishFollowCEO();
                // JavascriptBridge.Call(JavascriptBridge.UnityOpenLinkByType, "followxceo");
                break;
            case 9:
                JavascriptBridge.Call(JavascriptBridge.UnityShare, GameEntry.Context.Account);
                // GameEntry.Timer.Start(3f, () => { GameEntry.Http.Handler.FinishShare(); }, 1);
                break;
            case 10:
                GameEntry.GUI.Open(GameEntry.GUIPath.NewTaskUI.Path);
                // GameEntry.Timer.Start(3f, () => { GameEntry.Http.Handler.FinishShare(); }, 1);
                break;
            case 11:
                GameEntry.GUI.Close(GameEntry.GUIPath.NewTaskUI.Path);
                ExecuteAction.ByActionType(ExecuteActionType.Map_Pvp);
                break;
            case 12:
                GameEntry.GUI.Close(GameEntry.GUIPath.NewTaskUI.Path);
                ExecuteAction.ByActionType(ExecuteActionType.GarageUI);
                break;
        }
    }
    public static void OpenLink(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }
#if UNITY_EDITOR
        Application.OpenURL(url);
#else
        JavascriptBridge.Call(JavascriptBridge.UnityOpenLink, url);
#endif
    }
    public static void OpenLinkByType(string type)
    {
        JavascriptBridge.Call(JavascriptBridge.UnityOpenLinkByType, type);
    }
    public static void GetMoney(int type = 1)
    {
        GameEntry.GUI.Open(GameEntry.GUIPath.NewTaskUI.Path);
    }
    public static void Reload()
    {
        JavascriptBridge.Call(JavascriptBridge.UnityReload);
    }
    public static void Share()
    {
        JavascriptBridge.Call(JavascriptBridge.UnityExecute, "share", "s_" + GameEntry.Context.Account);
    }
}
public class GameUtils
{
    public static bool IsNewMap(int mapId)
    {
        var key = PlayerSystem.Instance.GetUID() + ":unlockmap" + mapId;
        if (StorageManager.Instance.HasKey(key))
        {
            return false;
        }
        var lv = CarCultivateSystem.Instance.GetCarLevel();
        var tpl = GameEntry.Table.Map.Get(mapId);
        if (tpl.SceneType != (int)EnumDefine.MapTypeEnum.Competition)
        {
            return false;
        }
        if (tpl.CarLevelLimit > 0 && lv >= tpl.CarLevelLimit)
        {
            return true;
        }
        return false;
    }
    public static void ReadNewMap(int mapId)
    {
        var key = PlayerSystem.Instance.GetUID() + ":unlockmap" + mapId;
        if (StorageManager.Instance.HasKey(key))
        {
            return;
        }
        StorageManager.Instance.SetBool(key, true);
        CheckNewMap();
    }
    public static bool CheckNewMap()
    {
        var maplen = GameEntry.Table.Map.ItemCount;
        while (maplen-- > 0)
        {
            var tpl = GameEntry.Table.Map.GetItem(maplen);
            var isnew = IsNewMap(tpl.Id);
            if (isnew)
            {
                GameEntry.RedDot.Set(RedDotName.NewMap, "check", true);
                return true;
            }
        }
        GameEntry.RedDot.Set(RedDotName.NewMap, "check", false);
        return false;
    }
    public static void SetHead(Image target, int headId)
    {
        var headTpl = GameEntry.Table.Head.Get(headId);
        if (headTpl != null)
        {
            GameEntry.Atlas.SetSprite(target, headTpl.BigIcon);
        }
    }
    public static string GetRandomName()
    {
        var len = GameEntry.Table.Nickname.ItemCount;
        var f = GameEntry.Table.Nickname.GetItem(UnityEngine.Random.Range(0, len));
        var name = f.FirstName;
        var m = GameEntry.Table.Nickname.GetItem(UnityEngine.Random.Range(0, len));
        if (!string.IsNullOrEmpty(m.MiddleName))
        {
            name += m.MiddleName;
        }
        var l = GameEntry.Table.Nickname.GetItem(UnityEngine.Random.Range(0, len));
        if (!string.IsNullOrEmpty(l.LastName))
        {
            name += l.LastName;
        }
        return name;
    }
}