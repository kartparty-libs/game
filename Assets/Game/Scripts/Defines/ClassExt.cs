using Framework;
using Framework.Core;
using Framework.DataTable;
using Newtonsoft.Json;
using UnityEngine;
public class Vector3Ext : IDataParser
{
    public static Vector3 Parse(string value)
    {
        var v3 = new Vector3();
        if (string.IsNullOrEmpty(value)) return v3;
        var lst = Framework.Utils.Parse.ParseList(value);
        var len = lst.Length;
        if (len > 0)
        {
            v3.x = float.Parse(lst[0]);
        }
        if (len > 1)
        {
            v3.y = float.Parse(lst[1]);
        }
        if (len > 2)
        {
            v3.z = float.Parse(lst[2]);
        }
        return v3;
    }
    public static void Encode(Vector3 value, ByteArray buffer)
    {
        buffer.Write(value.x);
        buffer.Write(value.x);
        buffer.Write(value.x);
    }
    public static Vector3 Decode(ByteArray buffer)
    {
        var v3 = new Vector3();
        v3.x = buffer.ReadFloat();
        v3.y = buffer.ReadFloat();
        v3.z = buffer.ReadFloat();
        return v3;
    }

    public string GenDecodeScript()
    {
        return "Vector3Ext.Decode";
    }

    public string GenEncodeScript()
    {
        return "Vector3Ext.Encode";
    }

    public string GenParseScript()
    {
        return "Vector3Ext.Parse";
    }

    public string GetTypeName()
    {
        return "Vector3";
    }
}
public class SceneExitData : IDataParser
{
    public int SceneId;
    public int EnterId;
    public static SceneExitData Parse(string value)
    {
        var data = new SceneExitData();
        var lst = Framework.Utils.Parse.ParseList(value);
        var len = lst.Length;
        if (len > 0)
        {
            data.SceneId = int.Parse(lst[0]);
        }
        if (len > 1)
        {
            data.EnterId = int.Parse(lst[1]);
        }
        return data;
    }
    public static void Encode(SceneExitData value, ByteArray buffer)
    {
        buffer.Write(value.SceneId);
        buffer.Write(value.EnterId);
    }
    public static SceneExitData Decode(ByteArray buffer)
    {
        var d = new SceneExitData();
        d.SceneId = buffer.ReadInt();
        d.EnterId = buffer.ReadInt();
        return d;
    }
    public string GenEncodeScript()
    {
        return "SceneExitData.Encode";
    }

    public string GenParseScript()
    {
        return "SceneExitData.Parse";
    }

    public string GetTypeName()
    {
        return "SceneExitData";
    }

    public string GenDecodeScript()
    {
        return "SceneExitData.Decode";
    }
}
public partial class CarMoudleProperty : IDataParser, IReference
{
    [JsonIgnore]
    public bool IsReferenceActive { get; set; }

    public static CarMoudleProperty Parse(string value)
    {
        var data = new CarMoudleProperty();
        var lst = Framework.Utils.Parse.ParseList(value);
        var len = lst.Length;
        if (len > 0)
        {
            data.Id = int.Parse(lst[0]);
        }
        if (len > 1)
        {
            data.Value = int.Parse(lst[1]);
        }
        return data;
    }
    public static void Encode(CarMoudleProperty value, ByteArray buffer)
    {
        buffer.Write(value.Id);
        buffer.Write(value.Value);
    }
    public static CarMoudleProperty Decode(ByteArray buffer)
    {
        var d = new CarMoudleProperty();
        d.Id = buffer.ReadInt();
        d.Value = buffer.ReadInt();
        return d;
    }
    public string GenEncodeScript()
    {
        return "CarMoudleProperty.Encode";
    }

    public string GenParseScript()
    {
        return "CarMoudleProperty.Parse";
    }

    public string GetTypeName()
    {
        return "CarMoudleProperty";
    }

    public string GenDecodeScript()
    {
        return "CarMoudleProperty.Decode";
    }

    public void Clear()
    {

    }
}

public partial class ItemConfig : IDataParser
{
    public string GenDecodeScript()
    {
        return "ItemConfig.Decode";
    }

    public string GenEncodeScript()
    {
        return "ItemConfig.Encode";
    }

    public string GenParseScript()
    {
        return "ItemConfig.Parse";
    }

    public string GetTypeName()
    {
        return "ItemConfig";
    }
    public static ItemConfig Parse(string value)
    {
        var data = new ItemConfig();
        var lst = Framework.Utils.Parse.ParseList(value);
        var len = lst.Length;
        if (len > 0)
        {
            data.ItemId = int.Parse(lst[0]);
        }
        if (len > 1)
        {
            data.ItemCount = int.Parse(lst[1]);
        }
        if (len > 2)
        {
            data.MaxCount = int.Parse(lst[1]);
        }
        return data;
    }
    public static void Encode(ItemConfig value, ByteArray buffer)
    {
        buffer.Write(value.ItemId);
        buffer.Write(value.ItemCount);
        buffer.Write(value.MaxCount);
    }
    public static ItemConfig Decode(ByteArray buffer)
    {
        var d = new ItemConfig();
        d.ItemId = buffer.ReadInt();
        d.ItemCount = buffer.ReadInt();
        d.MaxCount = buffer.ReadInt();
        return d;
    }
}
public partial class CfgItemData : IDataParser
{
    public string GenDecodeScript()
    {
        return "CfgItemData.Decode";
    }

    public string GenEncodeScript()
    {
        return "CfgItemData.Encode";
    }

    public string GenParseScript()
    {
        return "CfgItemData.Parse";
    }

    public string GetTypeName()
    {
        return "CfgItemData";
    }
    public static CfgItemData Parse(string value)
    {
        var lst = Framework.Utils.Parse.ParseList(value);
        if (lst == null)
        {
            return null;
        }
        var data = new CfgItemData();
        var len = lst.Length;
        if (len > 0)
        {
            data.Id = int.Parse(lst[0]);
        }
        if (len > 1)
        {
            data.Count = long.Parse(lst[1]);
        }
        return data;
    }
    public static void Encode(CfgItemData value, ByteArray buffer)
    {
        if (value == null)
        {
            buffer.WriteByte(0);
        }
        else
        {
            buffer.WriteByte(1);
            buffer.Write(value.Id);
            buffer.Write(value.Count);
        }

    }
    public static CfgItemData Decode(ByteArray buffer)
    {
        if (buffer.ReadByte() == 1)
        {
            var d = new CfgItemData();
            d.Id = buffer.ReadInt();
            d.Count = buffer.ReadLong();
            return d;
        }
        else
        {
            return null;
        }


    }
}