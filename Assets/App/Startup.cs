using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public partial class Startup : MonoBehaviour
{
    private const string CDN_URL = "http://192.168.1.97/";
    [Header("内置显示节点")]
    public GameObject Node;
    [Header("进度")]
    public GameObject Progress;
    public string EntryAssemblyName;
    public string EntryClassType;
    public string EntryMethod;
    [Header("模拟加载")]
    public string RemoteLoadPath;
    private Assembly entry;
    private float targetProgress;
    private int debugCount = 0;
    private bool hasComplete;
    private Slider Slider;
    private Text Msg;
    private void Start()
    {
        Application.targetFrameRate = 30;
        this.StartCoroutine(Load());
    }
    private IEnumerator Load()
    {
        DebugLogManager.Instance.PopupEnabled = false;
        DebugLogConsole.AddCommand<string, string>("set", "添加设置", setPlayerPrefs);
        DebugLogConsole.AddCommand<string>("remove", "删除设置", removePlayerPrefs);
        hasComplete = false;
        Dictionary<string, string> param = new Dictionary<string, string>();
        var manifestFile = "manifest.json";
        var mode = "Release";
        var lang = Application.systemLanguage.ToString();
        if (PlayerPrefs.HasKey("Debug"))
        {
            mode = "Debug";
            manifestFile = "debug.json";
        }
        param.Add("mode", mode);
        param.Add("lang", lang);
        string guid;
        if (PlayerPrefs.HasKey("PlayerGUID"))
        {
            guid = PlayerPrefs.GetString("PlayerGUID");
            if (string.IsNullOrEmpty(guid))
            {
                guid = System.Guid.NewGuid().ToString();
                PlayerPrefs.SetString("PlayerGUID", guid);
            }
        }
        else
        {
            guid = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("PlayerGUID", guid);
        }
        param.Add("version", Application.version);
        param.Add("platform", Application.platform.ToString());
        param.Add("guid", guid);
        Msg = Node.transform.Find("msg").GetComponent<Text>();
        if (Msg != null)
        {
            // Msg.gameObject.SetActive(false);
        }
        if (Progress != null)
        {
            Slider = Progress.gameObject.GetComponent<Slider>();
            var rect = Slider.GetComponent<RectTransform>();
            var canvas = rect.parent.GetComponent<RectTransform>();
            rect.offsetMin = new Vector2(0f, 0f);
            rect.offsetMax = new Vector2(1f, 0f);

            rect.sizeDelta = new Vector2(-canvas.rect.width % 38, 64);
            rect.anchoredPosition = new Vector2(0, 53);
        }
        bool isEditor = false;
#if UNITY_EDITOR
        isEditor = true;
#endif
        string AssemblyPath = string.Empty;
        string AssetBundlePath = string.Empty;
        bool cdnMode = true;
        if (cdnMode)
        {
            string remoteUrl = "";
#if HybridCLR
            remoteUrl = CDN_URL;
            if (PlayerPrefs.HasKey("cdnurl"))
            {
                remoteUrl = PlayerPrefs.GetString("cdnurl");
            }
#else
            remoteUrl = "";
#endif
            //如果没有服务器则要用 模式 和 语言 获取资源路径
            if (!string.IsNullOrEmpty(remoteUrl))
            {
                targetProgress = 0f;
                var url = Path.Combine(remoteUrl, Application.platform.ToString());
                var manifestPath = new System.Uri(Path.Combine(url, manifestFile)).AbsoluteUri;
                Debug.Log(manifestPath);
                var request = UnityWebRequest.Get(manifestPath + "?" + UnityEngine.Random.Range(100000, 9999999) + "_" + (DateTime.Now.Ticks));
                request.timeout = 6;
                yield return request.SendWebRequest();
                Debug.LogError(request.result);
                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        var manifestJson = JsonUtility.FromJson<ManifestJson>(request.downloadHandler.text);
                        AssemblyPath = manifestJson.Assembly;
                        AssetBundlePath = manifestJson.AssetBundle;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            else
            {
                targetProgress = 1f;
            }

        }

        Debug.LogFormat("AssemblyPath {0}", AssemblyPath);
        Debug.LogFormat("AssetBundlePath {0}", AssetBundlePath);
        var loader = new HotfixLoader(AssemblyPath);
        loader.OnProgress = () =>
        {
            targetProgress = loader.Progress;
        };
        yield return loader.Load(EntryAssemblyName, isEditor);
        entry = loader.HotfixAssembly;
        if (entry != null)
        {
            var appType = entry.GetType(EntryClassType);
            var mainMethod = appType.GetMethod(EntryMethod);
            PlayerPrefs.SetString("RemoteLoadPath", RemoteLoadPath);
            Debug.Log("enter");
            mainMethod.Invoke(null, new object[] { this.gameObject, this.Node, AssetBundlePath });
            hasComplete = true;
        }
        else
        {
            Debug.LogError("没有找到入口程序集");
        }
    }
    private void Update()
    {
        if (!hasComplete && Slider != null)
        {
            Slider.value = Mathf.Lerp(Slider.value, targetProgress * 0.3f, 0.1f);
            if (Msg != null)
            {
                int v = Mathf.FloorToInt(Slider.value * 100);
                Msg.text = "LOADING " + v + "%";
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x > 0.8f * Screen.width && Input.mousePosition.y > 0.8f * Screen.height)
            {
                debugCount++;
                if (debugCount > 10)
                {
                    DebugLogManager.Instance.PopupEnabled = true;
                }
            }
            else
            {
                debugCount = 0;
            }
        }
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
}

public class ManifestJson
{
    public string Assembly;
    public string AssetBundle;
}