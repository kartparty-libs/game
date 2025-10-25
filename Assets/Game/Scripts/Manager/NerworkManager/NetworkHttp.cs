using System;
using System.IO;
using Framework;
using Newtonsoft.Json;
using UnityEngine;

public class NetworkHttp
{
    private HttpManager HttpManager;
    public string serverUrl { get; private set; }
    public HttpHandler Handler { get; private set; }

    private string _account;
    private string _token;
    private float _sendDelay;
    private string gateServerUrl;
    private string gameServerUrl;
    public NetworkHttp()
    {
        serverUrl = GameDefine.LoginServer;
#if UNITY_EDITOR
        var p = new Uri(Path.Combine(Application.dataPath, "../", "server.user"));
        if (File.Exists(p.AbsolutePath))
        {
            var f = File.ReadAllText(p.AbsolutePath);
            if (!string.IsNullOrEmpty(f))
            {
                serverUrl = f;
                Debug.LogError("custom server " + f);
            }
        }
#endif
        HttpManager = new HttpManager();
        HttpManager.SetCoroutineManager(GameEntry.Coroutine);
        Handler = new HttpHandler();
    }
    public void Update(float deltaTime)
    {
        HttpManager.Update(deltaTime, deltaTime);
        if (_sendDelay > 0)
        {
            _sendDelay -= deltaTime;
        }
    }
    public void SetAccountToken(string account, string token)
    {
        _account = account;
        _token = token;
    }
    public void UpdateServerInfo(int id, string host)
    {
        serverUrl = host;
    }
    public void SetGameServer(int id, string host)
    {
        if (string.IsNullOrEmpty(gameServerUrl))
        {
            gateServerUrl = serverUrl;
        }
        serverUrl = gameServerUrl = host;
    }
    public void Send(byte[] data, string customUrl = null)
    {
        // if (_sendDelay > 0f)
        // {
        //     // Debug.LogError("----");
        //     return;
        // }
        var customIp = PlayerPrefs.GetString("ip");
        if (string.IsNullOrEmpty(customUrl))
        {
            customUrl = serverUrl;
            if (!string.IsNullOrEmpty(customIp))
            {
                customUrl = customIp;
            }
        }
        var url = customUrl;
        var item = HttpManager.Request(url, callback);
        item.AddFileData(data);
        _sendDelay = 1f;

    }
    private void callback(HttpResult result)
    {
        _sendDelay = 0f;
        // Debug.LogError(result.Text);
        Handler.ResponseData(result.Bytes);
        // Handler.ResponseData(result.Text);
    }
}