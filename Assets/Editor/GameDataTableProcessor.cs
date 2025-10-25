using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Framework;
using Framework.DataTable;
using Newtonsoft.Json;
using UnityEngine;

public class GameDataTableProcessor : DataTableProcessor, IDataTableProcessor
{
    public override void Start()
    {
        base.Start();
        AddParser(new Vector3Ext());
        AddParser(new SceneExitData());
        AddParser(new CarMoudleProperty());
        AddParser(new ItemConfig());
        AddParser(new CfgItemData());

    }
    public override void End()
    {
        base.End();

    }
    public override DataTableManager GetManager()
    {
        return new TableManager();
    }
    public override bool ProcessAllTable(List<string> value)
    {
        var success = base.ProcessAllTable(value);
        if (success)
        {
            var dict = new Dictionary<string, bool>();
            foreach (var item in value)
            {
                dict.Add(item, true);
            }
            var fileList = Directory.GetFileSystemEntries(Path.Combine(Application.dataPath, _dataOutputPath), "*.*");
            foreach (var item in fileList)
            {
                var ext = Path.GetExtension(item);
                if (ext == ".meta") continue;
                if (ext != ".tsv") continue;
                var fileName = Path.GetFileNameWithoutExtension(item);
                var _id = fileName.IndexOf("_");
                if (_id != -1)
                {
                    fileName = fileName.Substring(0, _id);
                }
                if (dict.ContainsKey(fileName))
                {
                    continue;
                }
                // File.Delete(item);
            }

            Dictionary<string, bool> tables = new Dictionary<string, bool>();
            foreach (var file in fileList)
            {
                var ext = Path.GetExtension(file);
                if (ext != ".txt") continue;
                var name = Path.GetFileNameWithoutExtension(file);
                var id = name.IndexOf("_");
                if (id >= 0)
                {
                    name = name.Substring(0, id);
                }
                if (tables.ContainsKey(name)) continue;
                tables.Add(name, true);
            }
            var tplPath = Path.Combine("../server/HttpServer/src/Config/tpl", "manager.txt");
            if (File.Exists(tplPath))
            {
                var tplStr = File.ReadAllText(tplPath);
                var fieldBuilder = new StringBuilder();
                var addBuilder = new StringBuilder();
                foreach (var item in value)
                {
                    // if (!tables.ContainsKey(item)) continue;
                    var cspath = Path.Combine("../server/HttpServer/src/Config/ConfigData", item + ".cs");
                    if (!File.Exists(cspath))
                    {
                        continue;
                    }
                    fieldBuilder.AppendFormat("\tpublic static {0}_Table {0}", item);
                    fieldBuilder.AppendLine(" { get; private set; }");
                    addBuilder.AppendFormat("\t\t{0} = AddTable(new {0}_Table());", item);
                    addBuilder.AppendLine();
                }
                tplStr = tplStr.Replace("$Fields", fieldBuilder.ToString());
                tplStr = tplStr.Replace("$add", addBuilder.ToString());
                var fpath = Path.Combine("../server/HttpServer/src/Config/", "ConfigManager.Load.cs");
                File.WriteAllText(fpath, tplStr);
            }

            var extfile = Path.Combine(Application.dataPath, "Game/Scripts/Defines/ExtDefine.cs");
            if (File.Exists(extfile))
            {
                var dst = Path.Combine("../server/HttpServer/src/Config/ConfigData/", "ExtDefine.cs");
                File.Copy(extfile, dst, true);
            }

        }
        return success;
    }
    public override void Validate()
    {


    }
    protected override bool ProcessTableData(TableMetaData value, string define, string branch)
    {
        var success = base.ProcessTableData(value, define, branch);
        if (define == "s")
        {
            bool hasDefine = value.Header.HasDefine(define);
            if (!hasDefine)
            {
                Debug.LogWarning("server don`t need data " + value.Name);
                return true;
            }
            var tplPath = Path.Combine("../server/HttpServer/src/Config/tpl", "table.txt");
            if (File.Exists(tplPath))
            {
                var tplStr = File.ReadAllText(tplPath);
                tplStr = tplStr.Replace("$Name", value.Name);
                var hasIndex = value.Header.IndexField != null;

                tplStr = tplStr.Replace("$HasIndex", hasIndex ? "true" : "false");
                if (hasIndex)
                {
                    tplStr = tplStr.Replace("$IndexType", value.Header.IndexField.Type.TypeName);
                    tplStr = tplStr.Replace("$IndexName", value.Header.IndexField.Name);
                }
                var csBuilder = new StringBuilder();
                foreach (var item in value.Header.FieldInfos)
                {
                    if (item.HasDefine(define))
                    {
                        if (item.Type.IsArray)
                        {
                            csBuilder.AppendFormat("\tpublic {0}[] {1}; ", item.Type.TypeName, item.Name);
                            // Debug.LogErrorFormat("public {0}[] {1};", field.Type.TypeName, field.Name);
                        }
                        else
                        {
                            csBuilder.AppendFormat("\tpublic {0} {1};", item.Type.TypeName, item.Name);
                            // Debug.LogErrorFormat("public {0} {1};", field.Type.TypeName, field.Name);
                        }
                        csBuilder.Append("\n");
                    }
                }
                tplStr = tplStr.Replace("$Fields", csBuilder.ToString());
                tplPath = Path.Combine("../server/HttpServer/src/Config/ConfigData", value.Name + ".cs");

                File.WriteAllText(tplPath, tplStr);
            }






            var jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[\n");
            //服务器用lua
            var sb = new StringBuilder();
            sb.Append("return\n{\n");
            var haxindex = value.Header.IndexField != null;
            var datalen = value.Data.Count;
            for (int i = 0; i < datalen; i++)
            {
                var data = value.Data[i];
                if (value.Header.IndexField != null)
                {
                    if (value.Header.IndexField.Type.IsString)
                    {
                        sb.Append("[\"" + data[value.Header.IndexField.Index] + "\"]");
                    }
                    else
                    {
                        sb.Append("[" + data[value.Header.IndexField.Index] + "]");
                    }
                }
                else
                {
                    sb.Append("[" + i + "]");
                }

                sb.Append("={");
                jsonBuilder.Append("{");
                var len = value.Header.FieldInfos.Count;
                bool addsplit = false;
                for (var f = 0; f < len; f++)
                {
                    var finfo = value.Header.FieldInfos[f];

                    if (!finfo.Define.Contains(define))
                    {
                        continue;
                    }
                    if (finfo.Name == "Id")
                    {
                        // continue;
                    }
                    if (addsplit)
                    {
                        sb.Append(",");
                        jsonBuilder.Append(",");
                    }
                    jsonBuilder.Append("\"" + finfo.Name);
                    jsonBuilder.Append("\":");
                    addsplit = true;
                    sb.Append(finfo.Name);
                    sb.Append("=");
                    var sdata = value.Data[i][finfo.GetBranchIndex(branch)];
                    if (finfo.Type.IsValueType)
                    {
                        if (string.IsNullOrEmpty(sdata))
                        {
                            sb.Append("0");
                            jsonBuilder.Append("0");
                        }
                        else
                        {
                            sb.Append(sdata);
                            jsonBuilder.Append(sdata);
                        }

                    }
                    else if (finfo.Type.IsString)
                    {
                        sb.Append("\"" + sdata + "\"");
                        jsonBuilder.Append("\"" + sdata + "\"");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(sdata))
                        {
                            if (finfo.Type.IsArray)
                            {
                                sb.Append("{}");
                                jsonBuilder.Append("[]");
                            }
                            else if (finfo.Type.IsClass)
                            {
                                sb.Append("nil");
                                jsonBuilder.Append("null");
                            }
                        }
                        else
                        {
                            sb.Append(sdata);

                            if (_dataParser.TryGetValue(finfo.Type.TypeName, out var parser))
                            {
                                var ptype = parser.GetType();
                                if (finfo.Type.IsArray)
                                {

                                    var sdList = Utils.Parse.ParseList(sdata);
                                    var sdLen = sdList.Length;
                                    var sdl = new List<object>();
                                    for (int sdi = 0; sdi < sdLen; sdi++)
                                    {
                                        var m = ptype.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
                                        var sdv = m.Invoke(null, new object[] { sdList[sdi] });
                                        sdl.Add(sdv);
                                    }
                                    jsonBuilder.Append(JsonConvert.SerializeObject(sdl));
                                }
                                else
                                {
                                    // Debug.LogError(sdata);
                                    if (string.IsNullOrEmpty(sdata))
                                    {
                                        jsonBuilder.Append("null");
                                    }
                                    else
                                    {
                                        var m = ptype.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
                                        var sdv = m.Invoke(null, new object[] { sdata });
                                        jsonBuilder.Append(JsonConvert.SerializeObject(sdv));
                                    }

                                }


                            }
                            else
                            {
                                if (finfo.Type.IsArray)
                                {
                                    var sdList = Utils.Parse.ParseList(sdata);
                                    jsonBuilder.Append("[");
                                    for (int sdi = 0; sdi < sdList.Length; sdi++)
                                    {
                                        if (sdi > 0)
                                        {
                                            jsonBuilder.Append(",");
                                        }
                                        jsonBuilder.Append(sdList[sdi]);
                                    }
                                    jsonBuilder.Append("]");
                                }
                                else
                                {
                                    jsonBuilder.Append(sdata);
                                }
                            }



                        }
                    }

                }
                sb.Append("}");
                jsonBuilder.Append("}");
                if (i < datalen - 1)
                {
                    sb.Append(",\n");
                    jsonBuilder.Append(",\n");
                }
            }
            sb.Append("}");
            jsonBuilder.Append("\n]");

            var fpath = Path.Combine("../server/GameServer/Server/ConfigS", value.Name + "Cfg_S.lua");
            // File.WriteAllText(fpath, sb.ToString());
            fpath = Path.Combine("../server/HttpServer/config/logic", value.Name + ".json");
            File.WriteAllText(fpath, jsonBuilder.ToString());

        }
        return success;
    }
}
