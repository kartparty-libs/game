
using System.Collections.Generic;
using UnityEngine;
using static EnumDefine;

public class TaskOnlineTime : IServerTime
{
    public const string TaskOnlineTimeValue = nameof(TaskOnlineTimeValue);
    public const string TaskOnlineTimeDay = nameof(TaskOnlineTimeDay);
    private List<int> _sendValues = new List<int>();
    private List<int> _checkSecondsValues = new List<int>();
    private int _minutes;
    private int _today;
    private int _seconds;
    public TaskOnlineTime()
    {

    }
    public void UpdateMinutes()
    {
        _minutes++;
        var nowdate = GameEntry.OfflineManager.GetServerDateTime();
        // Debug.LogError(nowdate.ToString() + " " + _minutes);
        if (nowdate.Day != _today)
        {
            _today = nowdate.Day;
            _minutes = 0;
        }
        PlayerPrefs.SetInt(TaskOnlineTimeValue, _minutes);
        PlayerPrefs.SetInt(TaskOnlineTimeDay, nowdate.Day);


    }
    public void NewDay()
    {
        ResetData();
    }
    private void ResetData()
    {
        _minutes = 0;
        _seconds = 0;
        PlayerPrefs.DeleteKey(TaskOnlineTimeValue);
        PlayerPrefs.DeleteKey(TaskOnlineTimeDay);
        var len = GameEntry.Table.Task.ItemCount;
        int p = 0;
        _checkSecondsValues.Clear();
        _sendValues.Clear();
        for (int i = 0; i < len; i++)
        {
            var task = GameEntry.Table.Task.GetItem(i);
            if (task.TaskType == (int)TaskTypeEnum.eDailyTask && task.TaskEvent == (int)TaskEventEnum.eOnlineTime)
            {
                _checkSecondsValues.Add(task.TaskValueParam * 60);
                _sendValues.Add(task.TaskValueParam - p);
                p = task.TaskValueParam;
            }
        }
    }

    public void UpdateSeconds()
    {
        _seconds++;
        if (_sendValues.Count > 0)
        {
            var p = _checkSecondsValues[0];
            if (_seconds >= p)
            {
                _checkSecondsValues.RemoveAt(0);
                p = _sendValues[0];
                _sendValues.RemoveAt(0);
                GameEntry.Http.Handler.UploadOnlineTime(p);
            }
        }

    }

    public void Reset()
    {
        ResetData();
        var nowdate = GameEntry.OfflineManager.GetServerDateTime();
        _today = nowdate.Day;
        if (PlayerPrefs.HasKey(TaskOnlineTimeValue) && PlayerPrefs.HasKey(TaskOnlineTimeDay))
        {
            var day = PlayerPrefs.GetInt(TaskOnlineTimeDay);
            if (day == nowdate.Day)
            {
                _minutes = PlayerPrefs.GetInt(TaskOnlineTimeValue);
                _seconds = _minutes * 60;
            }
        }
    }
}