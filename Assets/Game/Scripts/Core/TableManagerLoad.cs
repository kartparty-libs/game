using Framework;
using Framework.DataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEngine;

public partial class TableManager
{
    private Dictionary<string, TableConfig> _configs;
    public int LoadCount { get; private set; }
    public IEnumerator Load(ResLoader loader)
    {
        LoadCount = 0;
        //设置语言
        this.Lang.SetBranch("en");
        // this.Lang.SetDecodeCountPreFrame(1000);
        var fileFormat = "Assets/Res/DataTable/{0}.bytes";
        //加载文件大小描述
        var sizefile = string.Format(fileFormat, "filesize");
        Dictionary<string, int> sizeDict = new Dictionary<string, int>();
        var asset = loader.LoadAsset(sizefile);
        yield return asset;
        loader.Unload(asset);
        if (asset.Asset is TextAsset sizeAsset)
        {
            var sizeBuffer = Utils.Unity.ReadTextAssetToByteArray(sizeAsset);
            try
            {
                var fileCount = sizeBuffer.ReadInt();
                while (fileCount-- > 0)
                {
                    var n = sizeBuffer.ReadString();
                    var s = sizeBuffer.ReadInt();
                    sizeDict.Add(n, s);
                }
            }
            catch (System.Exception)
            {

            }
            sizeBuffer.Dispose();
        }
        _configs = new Dictionary<string, TableConfig>();
        var list = new List<TableConfig>();
        var len = this.Count;
        int fileSize = 0;
        ByteArray bigBuffer = null;
        while (len-- > 0)
        {
            var tableLoader = GetLoader(len);
            var cfg = new TableConfig();
            _configs.Add(tableLoader.GetTableName(), cfg);
            list.Add(cfg);
            cfg.Loader = tableLoader;

            if (sizeDict.TryGetValue(tableLoader.GetTableName(), out int size))
            {
                cfg.Size = size;
                if (!tableLoader.DecodeInstant())
                {
                    //运行时读取要保存buffer
                    var loadbuffer = new ByteArray(size);
                    cfg.Buffer = loadbuffer;
                    if (bigBuffer == null)
                    {
                        bigBuffer = loadbuffer;
                    }
                    else
                    {
                        if (bigBuffer.Buffer.Length < loadbuffer.Buffer.Length)
                        {
                            bigBuffer = loadbuffer;
                        }
                    }
                }
                else
                {
                    if (size > fileSize)
                    {
                        fileSize = size;
                    }
                }
            }
            else
            {
                Debug.Log("size error " + tableLoader.GetTableName());
            }
        }
        if (bigBuffer == null || fileSize > bigBuffer.Buffer.Length)
        {
            bigBuffer = new ByteArray(fileSize);

        }
        Debug.Log("big size " + bigBuffer.Buffer.Length);
        foreach (var v in _configs)
        {
            var cfg = v.Value;
            var item = v.Value.Loader;//因为共享了buffer所以要先加载
            if (item.DecodeInstant())
            {
                cfg.Buffer = bigBuffer;
                yield return LoadItem(loader, cfg);
                //LoadCount++;
            }
        }
        foreach (var v in _configs)
        {
            var cfg = v.Value;
            var item = v.Value.Loader;
            if (!item.DecodeInstant())
            {
                yield return LoadItem(loader, cfg);
                //LoadCount++;
            }
        }
    }
    private IEnumerator LoadItem(ResLoader loader, TableConfig config)
    {
        var item = config.Loader;
        var fileName = item.GetFileName();
        if (config.branch == fileName)
        {
            yield break;
        }
        config.branch = fileName;
        var path = string.Format("Assets/Res/DataTable/{0}.bytes", fileName);
        var asset = loader.LoadAsset(path);
        yield return asset;
        loader.Unload(asset);
        var buffer = config.Buffer;
        if (asset.Asset is TextAsset text)
        {
            Utils.Unity.ReadTextAssetToByteArray(text, buffer);
            buffer.Clear();
            item.ReadDecodeHeader(buffer);
            while (item.TryDecode() > 0)
            {
                yield return null;
            }
            LoadCount++;
        }
        else
        {
            Debug.LogError("load file error " + path);
        }
    }
    private class TableConfig
    {
        public IDataTableLoader Loader;
        public int Size;
        public ByteArray Buffer;
        public string branch;
    }
}