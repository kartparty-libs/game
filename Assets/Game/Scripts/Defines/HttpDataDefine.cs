using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static EnumDefine;

class ResponseData
{
    public int code;
    public long date;
    public Dictionary<string, JObject> result;
}

class MsgRequest
{
    public string method;
    public string hash;
    public IMsgBody param;
}
/// <summary>
/// 宝箱数据
/// </summary>
public class TreasureChestData
{
    /// <summary>
    /// 实例Id
    /// </summary>
    public int instId;
    /// <summary>
    /// 宝箱配置Id
    /// </summary>
    public int cfgId;
    /// <summary>
    /// 宝箱来源
    /// </summary>
    public int treasureChestSourceEnum;
    /// <summary>
    /// 订单Hash
    /// </summary>
    public string orderHash;
    /// <summary>
    /// 是否已验证通过
    /// </summary>
    public bool isVerified;
}