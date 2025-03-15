using System.Collections;
using Framework;
using Framework.Core;
using UnityEngine;
using UnityEngine.UI;

public class GameEntry
{
    private static GameObject _builtin;
    private static GameObject _builtinLoading;
    private static string _json;
    private static GameObject _coreGo;
    private static FrameworkCore _core;
    public static Timer Timer { get; private set; }
    public static CoroutineManager Coroutine { get; private set; }
    private static AssetLoader Loader;
    public static ResLoader AssetsLoader { get; private set; }
    public static AtlasLoader Atlas { get; private set; }
    public static UGUIManager GUI { get; private set; }
    public static TableManager Table { get; private set; }
    public static AudioManager Audio { get; private set; }
    public static GUIPathInfo GUIPath { get; private set; }
    public static GameProcedureManager GameProcedure { get; private set; }
    public static EntityManager Entity { get; private set; }
    public static GameContext Context { get; private set; }
    public static GameLogger Logger { get; private set; }
    public static EventDispatcher GUIEvent { get; private set; }
    public static NetMgr Net { get; private set; }
    public static OfflineManager OfflineManager { get; private set; }
    public static NetworkHttp Http { get; private set; }
    public static NetworkMatch Match { get; private set; }
    public static RedDot RedDot { get; private set; }
    public static XiaoZhiHttp XiaoZhi { get; private set; }
    private static string _editorRemoteLoadPath;

    private static bool _loading;
    private static Slider Progress;
    private static Text Msg;
    private static float _progress = 0.0f;
    public static void Startup(GameObject startup, GameObject inner, string json)
    {
        if (_core != null) return;
        new GMDefine();
        Debug.LogWarning(BuildTime.Time);
        _builtin = startup;
        _builtinLoading = inner;
        var img = _builtinLoading.transform.Find("progress");

        if (img != null)
        {
            Progress = img.GetComponent<Slider>();
        }
        else
        {
            Debug.LogError("not find progress img");
        }
        Msg = inner.transform.Find("msg").GetComponent<Text>();
        _loading = true;
        Logger = new GameLogger();
        Logger.Log(startup.gameObject.name);
        //Application.targetFrameRate = 60;
        _json = json;
        Debug.Log("json " + _json);
        RedDot = new RedDot();
        _coreGo = new GameObject("core");
        GameObject.DontDestroyOnLoad(_coreGo);
        _core = new FrameworkCore(_coreGo);
        OfflineManager = new OfflineManager();

        Net = new NetMgr();
        _core.OnUpdate = onUpdate;
        _core.OnApplicationQuit = onQuit;
        Timer = _core.Timer;
        Coroutine = _core.Coroutine;

        GUIPath = new GUIPathInfo();
        Table = new TableManager();
        _coreGo.AddComponent<ManagerScript>();
        _coreGo.AddComponent<SystemScript>();
        AnimatorHashIDs.Initialization();
        bool editorMode = false;
#if UNITY_EDITOR
        _editorRemoteLoadPath = PlayerPrefs.GetString("RemoteLoadPath");
        editorMode = true;
#else
        
#endif
        if (editorMode && string.IsNullOrEmpty(_editorRemoteLoadPath))
        {
            var editorLoader = new EditorAssetLoader();
            editorLoader.Initialize(0.1f);
            Loader = editorLoader;
            AssetsLoader = new ResLoader(Loader);
            _core.Coroutine.Start(initEnv(), "initEnv");
        }
        else
        {
            _core.Coroutine.Start(initBundleAssetLoader(), "initBundleAssetLoader");
        }

    }

    private static IEnumerator initBundleAssetLoader()
    {
        //请求服务器返回一些信息
        // 或者从热更代码那里带回信息

        //最新的资源版本号
        var versionFromServer = "abbeab905ab0a5e498c37ef40d2e2b68";
        //远程资源地址
        var updateURLFromServer = _json;
        //updateURLFromServer = "";
        //是否开启更新模式
        var updateEnable = true;
        var versionFile = "AssetBundles/version.bytes";
        var downloader = new BundleDownload(_core.Coroutine);
        if (!updateEnable)
        {
            updateURLFromServer = string.Empty;
        }
#if UNITY_EDITOR
        if (!string.IsNullOrEmpty(_editorRemoteLoadPath))
        {
            // downloader.SetDownloadSpeed(1024 * 1024 * 5);//模拟下载速度
            updateURLFromServer = _editorRemoteLoadPath;
        }
#endif
        setProgress(0.1f);
        // Debug.LogFormat("资源远程下载地址{0}", updateURLFromServer);
        yield return downloader.Initialize(updateURLFromServer, versionFile, versionFromServer);
        downloader.SetDownloadMaxCount(10);
        if (!downloader.Enabled)
        {
            yield break;
        }
        //请求更新资源
        yield return downloader.Request();
        setProgress(0.2f);
        if (!downloader.RequestFinished)
        {
            // Debug.Log("网络请求失败");
            yield break;
        }
        bool downloadNow = true;
        if (downloadNow)
        {
            // 统计并下载文件
            var count = downloader.NeedUpdateFileCount;

            // Logger.LogFormat("需要更新的资源量:{0}", count);
            while (downloader.NeedUpdateFileCount > 0)
            {
                var updatefile = downloader.ReadUpdateFile();
                downloader.Download(updatefile);
            }

            //已下载个数
            int loadedCount = 0;
            while (loadedCount < count)
            {
                var c = downloader.LoadSuccessFiles.Count;
                while (c-- > 0)
                {
                    var file = downloader.LoadSuccessFiles.Dequeue();
                    loadedCount++;
                    setProgress(0.2f + 0.6f * loadedCount / count);
                    Debug.LogFormat("{0}/{1}", loadedCount, count);
                }
                yield return null;
            }
            if (downloader.LoadErrorFiles.Count > 0)
            {
                // Debug.Log("文件下载存在失败的条目");
                //重新执行一次当前方法
                yield break;
            }
        }
        if (Loader == null)
        {
            var bundleLoader = new BundleAssetLoader();
            bundleLoader.Initialize(versionFile, _core.Coroutine, downloader);
            bundleLoader.SetUnloadDelayTime(3f);
            bundleLoader.SetAssetProcessor(new AssetProcessor());
            bundleLoader.SetIdleTime(0);
            Loader = bundleLoader;
            AssetsLoader = new ResLoader(Loader);
        }
        _core.Coroutine.Start(initEnv(), "initEnv");
    }
    private static IEnumerator initEnv()
    {
        Logger.Log("initEnv");
        GUIEvent = new EventDispatcher();
        Context = new GameContext();

        Atlas = new AtlasLoader(Loader);
        var loadAsset = AssetsLoader.LoadAsset("Assets/Res/Prefabs/Entry.prefab");
        yield return loadAsset;
        if (!(loadAsset.Asset is GameObject go))
        {
            Debug.Log(loadAsset.Result.Path + " load error");
            yield break;
        }
        setProgress(0.5f);
        GameObject entry = go;
        GameObject.DontDestroyOnLoad(entry);
        entry.name = "Entry";
        Logger.Log(_coreGo.name);

        _coreGo.transform.SetParent(entry.transform);
        _coreGo.transform.SetAsFirstSibling();
        yield return null;
        var js = entry.AddComponent<JavascriptBridge>();
        js.SetTarget(_builtin.GetComponentInChildren<JavaScript>());
        var gameConfig = entry.GetComponent<GameConfig>();
        Context.GameConfig = gameConfig;
        var uiconfig = entry.GetComponent<GUIConfig>();
        if (uiconfig != null)
        {
            GUI = new UGUIManager();
            GUI.Initialize(uiconfig);
            GUI.Initialize(Loader);
            //安全区域修改器
            GUI.SetModifier(new SafeAreaModifier());


        }
        var audioConfig = entry.GetComponent<AudioConfig>();
        Application.targetFrameRate = 60;
        if (audioConfig != null)
        {
            Audio = new AudioManager(Loader);
            Audio.Initialize(audioConfig.Mixer, audioConfig.Parent, new string[] { "Master", "Master/Music" ,"Master/Effect"});
        }
        Entity = new EntityManager(gameConfig.EneityContainer, Loader);

        // 启用物理系统自动同步转换
        Physics.autoSyncTransforms = true;
        loadAsset = AssetsLoader.LoadAsset("Assets/Res/Fsm/GameProcedure.asset");
        yield return loadAsset;
        Logger.Log("GameProcedure");
        if (loadAsset.Asset is FsmData fsmData)
        {
            GameProcedure = new GameProcedureManager(fsmData.GetFsm());
        }

        setProgress(0.98f);
        AssetsLoader.Unload(loadAsset);
        QualitySettings.vSyncCount = 0;
        Http = new NetworkHttp();
        Match = new NetworkMatch();
        XiaoZhi = new XiaoZhiHttp();
        // _loading = false;
    }
    private static bool _hasshowFade;
    public static void HideDefault()
    {
        if (_hasshowFade)
        {
            return;
        }
        _hasshowFade = true;
        _loading = false;
        var fade = _builtinLoading.transform.Find("fade");
        if (fade != null)
        {
            fade.gameObject.SetActive(true);
        }
        else
        {
            _builtinLoading?.SetActive(false);
        }

    }
    private static void setProgress(float value)
    {
        _progress = value;
        Debug.Log(value);
    }
    private static void onUpdate(float deltaTime, float unscaledDeltaTime)
    {
        if (_loading && Progress != null)
        {
            Progress.value = Mathf.Lerp(Progress.value, 0.1f + _progress * 0.9f, 0.05f);
            if (Msg != null)
            {
                int v = Mathf.FloorToInt(Progress.value * 100);
                Msg.text = "LOADING " + v + "%";
            }
        }
        AssetsLoader?.Update(deltaTime, unscaledDeltaTime);
        // Loader?.Update(deltaTime, unscaledDeltaTime);
        Atlas?.Update(deltaTime, unscaledDeltaTime);
        GUI?.Update(deltaTime, unscaledDeltaTime);
        Entity?.Update(deltaTime, unscaledDeltaTime);
        GameProcedure?.Update(deltaTime, unscaledDeltaTime);
        Logger.Update();
        Context?.Update(deltaTime);
        Net?.Update(deltaTime);
        OfflineManager?.Update(deltaTime);
        Http?.Update(deltaTime);
        Match?.Update();
        RedDot?.Update();
        XiaoZhi?.Update(deltaTime);
    }
    private static void onQuit()
    {
        Match?.OnApplicationQuit();
    }

}