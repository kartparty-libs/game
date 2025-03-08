using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using static EnumDefine;

public class OfflineManager
{
    public string Account { get; private set; }
    public OfflineReward OfflineReward { get; private set; }
    public TaskOnlineTime TaskOnlineTime { get; private set; }
    private PlayerData data;
    private long _serverStartTime;
    private float _serverTime;
    private int _today;
    private float _secondsTime;
    private float _minutesTime;
    private float _newDayDelay;
    private bool _hasLogin;
    private float _realtimeSinceStartup;
    private List<IServerTime> _serverTimeBehaviour;
    private Dictionary<string, TimeTask> _timeTask = new Dictionary<string, TimeTask>();
    private Dictionary<string, UpdateSystemTask> _updateSystemTask = new Dictionary<string, UpdateSystemTask>();
    private float _updateSystemTime;
    public OfflineManager()
    {
        _today = -1;
        _secondsTime = 0f;
        _minutesTime = 0f;
        OfflineReward = new OfflineReward();
        TaskOnlineTime = new TaskOnlineTime();
        _updateSystemTime = 0;
    }
    private string _loginAccount;
    public void Login(string account)
    {
        _loginAccount = account;
        GameEntry.Http.Handler.Login(_loginAccount, "123");
        _timeTask.Clear();
        // LoginSuccess(account);
        // https://api.etherscan.io/api?module=account&action=txlist&address=0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae&startblock=0&endblock=99999999&page=1&offset=10&sort=asc&apikey=YourApiKeyToken
        // var req = GameEntry.Http.Send("https://api.etherscan.io/api?module=account&action=txlist&address=0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae&startblock=0&endblock=99999999&page=1&offset=10&sort=asc&apikey=YourApiKeyToken");
    }
    public void LoginSuccess(string account)
    {
        Account = account;
        _hasLogin = true;
        GameEntry.Context.LoginSuccess = true;
        PlayerSystem.Instance.SetRoleId(account);

        var json = PlayerPrefs.GetString(account);
        if (!string.IsNullOrEmpty(json))
        {
            data = JsonUtility.FromJson<PlayerData>(json);
        }
        if (data == null)
        {
            GameEntry.Context.NeedCreateHead = true;
            data = new PlayerData();
        }
        // GameEntry.Context.NeedCreateHead = false;
        // GameEntry.Context.NeedCreateNickName = false;
        PlayerSystem.Instance.OnChangePlayerCfgId(data.RoleId);
        PlayerSystem.Instance.OnChangeCarCfgId(data.CarId);

        if (_serverTimeBehaviour == null)
        {
            _serverTimeBehaviour = new List<IServerTime>
            {
                TaskOnlineTime,
                OfflineReward
            };
            foreach (var d in _serverTimeBehaviour)
            {
                d.Reset();
            }
        }
        var myNickName = GameEntry.Context.SdkNickName;
        if (!string.IsNullOrEmpty(myNickName))
        {
            if (PlayerSystem.Instance.GetName() != myNickName)
            {
                GameEntry.Http.Handler.SetNickName(myNickName);
            }
        }

    }
    public void SetNickName(string value)
    {
        SavePlayerData();
        GameEntry.Http.Handler.SetNickName(value);

    }
    public void SetHeadId(int value)
    {
        GameEntry.Context.NeedCreateHead = false;
        PlayerSystem.Instance.OnChangeHead(value);
        SavePlayerData();
        GameEntry.Http.Handler.SetHeadId(value);
    }
    public void StartPveMatch(int mapId)
    {
        data.RoleId = GameEntry.Context.SelectRoleId;
        data.CarId = GameEntry.Context.SelectCarId;

        MatchSystem.Instance.SetPveMatchMapId(mapId, data.RoleId, data.CarId);
        SavePlayerData();

        MatchSystem.Instance.OnStartMateCompeComplete();
    }
    public void ExitMatch()
    {
        MatchSystem.Instance.OnStartMateCompeComplete(false);
    }
    private void SavePlayerData()
    {
        PlayerPrefs.SetString(Account, JsonUtility.ToJson(data));
    }
    public void SetServerTime(long milliseconds)
    {
        var p = _serverStartTime;
        _serverStartTime = milliseconds;
        _serverTime = 0f;
        var d = GetServerDateTime();
        CheckNewDay();
    }
    public DateTime GetServerDateTime()
    {
        var current = DateTimeOffset.FromUnixTimeMilliseconds(_serverStartTime).DateTime;
        var now = current.AddSeconds(_serverTime);
        return now;
    }
    public long GetServerDateValue()
    {
        return _serverStartTime + (long)_serverTime;
    }
    private bool CheckNewDay()
    {
        var d = GetServerDateTime();
        if (_today > 0)
        {
            if (_today != d.Day)
            {
                Debug.LogError("跨天了 " + d.ToString());
                _newDayDelay = 3f;
            }
        }
        _today = d.Day;
        GameEntry.GUI.SetParam(GameEntry.GUIPath.DailyTaskUI.Path, "time", d.ToString());
        return false;
    }
    public void Update(float deltaTime)
    {
        if (!_hasLogin)
        {
            return;
        }
        if (_serverTimeBehaviour == null)
        {
            return;
        }
        if (_realtimeSinceStartup > 0f)
        {
            deltaTime = Time.realtimeSinceStartup - _realtimeSinceStartup;
        }
        _realtimeSinceStartup = Time.realtimeSinceStartup;
        _secondsTime += deltaTime;
        _serverTime += deltaTime;
        _minutesTime += deltaTime;
        if (_secondsTime > 1f)
        {
            _secondsTime -= 1f;
            var newDay = CheckNewDay();
            foreach (var item in _serverTimeBehaviour)
            {
                if (newDay)
                {
                    item.NewDay();
                }
                item.UpdateSeconds();
            }
            if (newDay)
            {
                RankSystem.Instance.NewDay();
            }
            var now = GetServerDateTime();
            foreach (var item in _timeTask)
            {
                var task = item.Value;
                if (!task.Enable)
                {
                    continue;
                }
                task.Execute(now);
            }

        }
        if (_minutesTime > 60f)
        {
            _minutesTime -= 60f;
            foreach (var item in _serverTimeBehaviour)
            {
                item.UpdateMinutes();
            }
        }
        if (_newDayDelay > 0)
        {
            _newDayDelay -= deltaTime;
            if (_newDayDelay <= 0)
            {
                GameEntry.Http.Handler.Login(_loginAccount, "123");
            }
        }
        foreach (var item in _updateSystemTask)
        {
            if (item.Value.Execute(deltaTime))
            {
                if (_updateSystemTime <= 0f)
                {
                    _updateSystemTime = 1;
                }
            }
        }
        if (_updateSystemTime > 0f)
        {
            _updateSystemTime -= deltaTime;
            if (_updateSystemTime <= 0f)
            {
                GameEntry.Http.Handler.GetChangeDataSystemData();
            }
        }
    }
    public string GetRandomNickName()
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
    public int GetRandomRoleId()
    {
        var c = GameEntry.Table.Role.ItemCount;
        var r = UnityEngine.Random.Range(0, c);
        return GameEntry.Table.Role.GetItem(r).Id;
    }
    public int GetRandomCarId()
    {
        var c = GameEntry.Table.Car.ItemCount;
        var r = UnityEngine.Random.Range(0, c);
        return GameEntry.Table.Car.GetItem(r).Id;
    }
    public int GetRandomHeadId()
    {
        var c = GameEntry.Table.Head.ItemCount;
        var r = UnityEngine.Random.Range(0, c);
        return GameEntry.Table.Head.GetItem(r).Id;
    }
    public void SetTimeTask(string name, DateTime finishTime, Action action)
    {
        if (!_timeTask.TryGetValue(name, out var timeTask))
        {
            timeTask = new TimeTask();
            timeTask.Name = name;
            _timeTask.Add(name, timeTask);
        }
        timeTask.Callback = action;
        timeTask.TargetTime = finishTime;
        timeTask.Enable = true;
    }
    public void AddUpdateSystemTask(string name, int count, float interval)
    {
        if (count < 1)
        {
            count = 1;
        }
        if (interval < 1f)
        {
            interval = 1f;
        }
        if (!_updateSystemTask.TryGetValue(name, out var task))
        {
            task = new UpdateSystemTask();
            task.Name = name;
            _updateSystemTask.Add(name, task);
        }
        task.Interval = interval;
        task.Count = count;
        task.Stoped = false;
        task.Start();

    }
    public void DeleteUpdateSystemTask(string name)
    {
        if (_updateSystemTask.TryGetValue(name, out var task))
        {
            task.Stoped = true;
        }
    }
}
internal class PlayerData
{
    public int RoleId;
    public int CarId;

}

internal class TimeTask
{
    public string Name;
    public Action Callback;
    public DateTime TargetTime;
    public float exeTime;
    public bool Enable;
    public void Execute(DateTime now)
    {
        if (Callback == null)
        {
            Enable = false;
        }
        var diff = now - TargetTime;
        if (diff.TotalSeconds > 0)
        {
            exeTime = Time.realtimeSinceStartup;
            Enable = false;
            if (Callback != null)
            {
                try
                {
                    Callback.Invoke();
                }
                catch (System.Exception e)
                {
                    GameEntry.Logger.LogError(e.ToString());
                }

            }
        }
    }
}
internal class UpdateSystemTask
{
    public string Name;
    public int Count;
    public float Interval;
    public bool Stoped;
    public float Now;
    private float _interval;
    public void Start()
    {
        Now = Time.time;
        _interval = Interval;
    }
    public bool Execute(float deltaTime)
    {
        if (Stoped)
        {
            return false;
        }
        if (Count < 0)
        {
            return false;
        }
        if (_interval > 0)
        {
            _interval -= deltaTime;
            if (_interval <= 0f)
            {
                Count--;
                _interval = Interval;
                return true;
            }
        }

        return false;
    }

}