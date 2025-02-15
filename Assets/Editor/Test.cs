using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Framework;
using UnityEngine;

public class Test : IFrameworkTest
{
    public void Execute()
    {
        Debug.LogError("hello");

        // MapInfo();
        // onlineInfo();
        TaskInfo();
        SimpleOrder crypto = new SimpleOrder();
        var dstr = crypto.GenOrderId("7372306911", 3, 1, 10854551201, "ton");
        Debug.LogError(dstr);
        if (crypto.Parse(dstr))
        {
            Debug.LogError(crypto.Data);
        }

        if (crypto.Parse("kart1DIvjfJgq5Ntfb2TwhrCkCgZ+z50yZoS+UK7/HlcSm7Q/uG8ApehKS0fBuy5GaMYA"))
        {
            Debug.LogError(crypto.Data);
        }


    }
    private void onlineInfo()
    {
        var p = Path.Combine(Application.streamingAssetsPath, "account_login.csv");
        if (!File.Exists(p))
        {
            return;
        }
        Dictionary<string, int> time = new Dictionary<string, int>();
        Dictionary<string, int> roleStartTime = new Dictionary<string, int>();
        var file = File.ReadAllLines(p);
        var len = file.Length;
        for (int i = 0; i < len; i++)
        {
            var line = file[i];
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            var args = line.Split(",");
            if (args.Length < 4)
            {
                Debug.LogError("skip1 " + i + "   " + line);
                continue;
            }
            if (!args[3].StartsWith("20240515"))
            {
                Debug.LogError("skip2 " + i + "   " + args[3]);
                continue;
            }
            var k = args[0];
            TimeSpan sp = GetTimeSpan(args[3].Substring(8));
            var ts = (int)sp.TotalSeconds;
            if (args[2] == "1")
            {

                if (!roleStartTime.TryGetValue(k, out int value))
                {

                    roleStartTime.Add(k, ts);
                }
                else
                {
                    roleStartTime[k] = ts;
                }
            }
            else if (args[2] == "0")
            {
                if (roleStartTime.TryGetValue(k, out int value))
                {
                    var ttime = ts - value;
                    if (time.TryGetValue(k, out var ttime2))
                    {
                        time[k] = ttime2 + ttime;
                    }
                    else
                    {
                        time.Add(k, ttime);
                    }
                    roleStartTime.Remove(k);
                }
            }
            else
            {
                Debug.LogError("skip3 " + i + "   " + line);
            }
        }
        int timevalue = 0;
        foreach (var item in time)
        {
            timevalue += item.Value;
        }
        var sb = new StringBuilder();
        sb.AppendLine("在线时长信息");
        sb.AppendLine("总人数" + time.Count + "个");
        sb.AppendLine("总时间" + timevalue + "秒");
        sb.AppendLine(((float)timevalue / (float)time.Count).ToString());
        Debug.LogError(sb.ToString());
    }
    private void MapInfo()
    {
        var p = Path.Combine(Application.streamingAssetsPath, "map_log.csv");
        if (!File.Exists(p))
        {
            return;
        }
        var date = "20240516";
        var file = File.ReadAllLines(p);
        var len = file.Length;
        var dict = new Dictionary<string, string[]>();
        var list = new List<MapData>();
        for (int i = 0; i < len; i++)
        {
            var line = file[i];
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            var args = line.Split(",");
            if (args.Length < 5)
            {
                continue;
            }
            if (!args[4].StartsWith(date))
            {
                continue;
            }
            var k = args[0] + "-" + args[2];
            if (args[3] == "0")
            {
                if (dict.ContainsKey(k))
                {
                    dict[k] = args;
                }
                else
                {
                    dict.Add(k, args);
                }
            }
            else if (args[3] == "1")
            {
                if (dict.TryGetValue(k, out var val))
                {
                    var m = new MapData();
                    m.Name = args[0];
                    m.Map = args[2];
                    m.Start = val[4];
                    m.End = args[4];
                    list.Add(m);
                    dict.Remove(k);
                }
            }
        }
        var mapDict = new Dictionary<string, int>();
        var roleDict = new Dictionary<string, MapData>();
        foreach (var item in list)
        {
            var t1 = GetTimeSpan(item.Start.Substring(8));
            var t2 = GetTimeSpan(item.End.Substring(8));
            var v = (int)t2.TotalSeconds - (int)t1.TotalSeconds;
            if (v > 0)
            {
                if (!mapDict.TryGetValue(item.Map, out var v2))
                {
                    v2 = int.MaxValue;
                    mapDict.Add(item.Map, v2);
                }
                if (v < v2)
                {
                    v2 = v;
                    mapDict[item.Map] = v2;
                    if (!roleDict.TryGetValue(item.Name, out var v3))
                    {
                        v3 = item;
                        roleDict.Add(item.Name, v3);
                    }
                    v3.Time = v2;
                    roleDict[item.Name] = v3;
                }
            }
        }
        var sb = new StringBuilder();
        sb.AppendLine("地图信息" + date);
        foreach (var item in mapDict)
        {
            sb.AppendLine(item.Key + " 时间:" + item.Value);
            int max = int.MaxValue;
            MapData r = null;
            foreach (var role in roleDict)
            {
                if (role.Value.Map != item.Key)
                {
                    continue;
                }
                if (role.Value.Time < max)
                {
                    r = role.Value;
                    max = role.Value.Time;
                }
            }
            sb.AppendLine(r.Name + " " + r.End);
        }
        Debug.LogError(sb.ToString());
    }
    private TimeSpan GetTimeSpan(string value)
    {
        int.TryParse(value.Substring(0, 2), out var h);
        int.TryParse(value.Substring(2, 2), out var m);
        int.TryParse(value.Substring(4, 2), out var s);
        TimeSpan sp = new TimeSpan(h, m, s);
        var ts = (int)sp.TotalSeconds;
        return sp;
    }

    private void TaskInfo()
    {
        var p = Path.Combine(Application.streamingAssetsPath, "task_log.csv");
        if (!File.Exists(p))
        {
            return;
        }
        var date = "20240515";
        var file = File.ReadAllLines(p);
        var len = file.Length;
        var dict = new Dictionary<string, List<int>>();
        for (int i = 0; i < len; i++)
        {
            var line = file[i];
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            var args = line.Split(",");
            if (args.Length < 4)
            {
                continue;
            }
            if (!args[3].StartsWith(date))
            {
                continue;
            }
            if (!dict.TryGetValue(args[0], out var list))
            {
                list = new List<int>();
                dict.Add(args[0], list);
            }
            list.Add(int.Parse(args[2]));
        }
        var sb = new StringBuilder();
        foreach (var kv in dict)
        {
            kv.Value.Sort();
            sb.Append(kv.Key.Substring(0, 8) + "\t");
            var tlen = kv.Value.Count;
            for (int j = 0; j < tlen; j++)
            {
                if (j > 0)
                {
                    sb.Append(",");
                }
                sb.Append(kv.Value[j]);
            }
            sb.AppendLine();
        }
        File.WriteAllText(p + date + ".txt", sb.ToString());
    }
}
class MapData
{
    public string Name;
    public string Map;
    public string Start;
    public string End;
    public int Time;
    public override string ToString()
    {
        return Name + "     " + Map + "   " + Start.Substring(8) + "   " + End.Substring(8);
    }
}


public class SimpleOrder
{
    private const string VERSION = "kart1";
    public string Version { get; private set; }
    public string Account { get; private set; }
    public string ShopId { get; private set; }
    public int BuyCount { get; private set; }
    public long Amount { get; private set; }
    public string BuyType { get; private set; }
    public DateTime Time { get; private set; }
    public string Data { get; private set; }
    public SimpleOrder()
    {
    }
    public string GenOrderId(string account, int shopId, int count, long amount, string buyType)
    {
        if (string.IsNullOrEmpty(account) || shopId < 1 || count < 1)
        {
            return null;
        }
        long unixTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        var v1 = new OrderV1();
        var str = v1.Encrypt(account + "," + shopId + "," + count + "," + amount + "," + buyType + "," + unixTimestamp);
        return VERSION + str;
    }
    public bool Parse(string str)
    {
        string version = string.Empty;
        string data = string.Empty;
        if (!string.IsNullOrEmpty(str) && str.Length > 6)
        {
            version = str.Substring(0, 5);
            data = str.Substring(5);
        }
        if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(data))
        {
            return false;
        }
        if (version == "kart1")
        {
            var v1 = new OrderV1();
            Data = v1.Decrypt(data);
            this.Version = version;
            //account + "," + shopId + "," + count + "," + amount + "," + buyType + "," + unixTimestamp
            var args = Data.Split(",");
            Account = args[0];
            ShopId = args[1];
            if (!int.TryParse(args[2], out var buycount))
            {
                return false;
            }
            BuyCount = buycount;
            if (!long.TryParse(args[3], out var cost))
            {
                return false;
            }
            Amount = cost;
            this.BuyType = args[4];
            if (!long.TryParse(args[5], out var seconds))
            {
                return false;
            }
            this.Time = DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime;
            return true;
        }
        return false;
    }
    internal class OrderV1
    {
        private Aes aesAlg;
        public OrderV1()
        {
            aesAlg = Aes.Create();
            //修改后更改版本号
            var key32 = "f,4P;6/a&f*<6f/8932hfnnlkdsa1f4as5d6f385f4fasd3221f3a";//32个字符
            var iv16 = "GF7-/ad#8f^452@dsa221daopfe651fdsa12";//16个字符
            //修改后更改版本号
            aesAlg.Key = Encoding.UTF8.GetBytes(key32.Substring(0, 32)); // 密钥  
            aesAlg.IV = Encoding.UTF8.GetBytes(iv16.Substring(0, 16)); // 初始化向量  
        }
        public string Encrypt(string data)
        {
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                    }
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
        public string Decrypt(string data)
        {
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            try
            {
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(data)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            string decryptedData = srDecrypt.ReadToEnd();
                            return decryptedData;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }
    }
}