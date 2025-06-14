
using Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnumDefine;
public partial class DailyTaskUI : UIWindowBase
{
    private List<DailyTaskItem> items = new List<DailyTaskItem>();
    private Dictionary<int, List<Task_Data>> taskEventMap = new Dictionary<int, List<Task_Data>>();
    private List<Task_Data> showList = new List<Task_Data>();

    protected override void OnAwake()
    {
        base.OnAwake();
        var goclone = this.items_GameObject.transform.GetChild(0).gameObject;
        for (int i = 0; i < 5; i++)
        {
            var go = GameObject.Instantiate(goclone);
            go.transform.SetParent(this.items_GameObject.transform, false);
            var w = go.GetComponent<UGUIWidget>();
            if (w.Widget is DailyTaskItem item)
            {
                items.Add(item);
            }
        }
        goclone.SetActive(false);
        var len = GameEntry.Table.Task.ItemCount;
        for (int i = 0; i < len; i++)
        {
            var task = GameEntry.Table.Task.GetItem(i);
            if (task.TaskType == (int)TaskTypeEnum.eDailyTask || task.TaskEvent == (int)TaskEventEnum.eAccomplishTaskByEvent)
            {
                if (!taskEventMap.TryGetValue(task.TaskEvent, out var list))
                {
                    list = new List<Task_Data>();
                    taskEventMap.Add(task.TaskEvent, list);
                }
                list.Add(task);
            }
        }

    }
    protected override void OnOpen()
    {
        base.OnOpen();
        updateTaskList();
        EventManager.Instance.Regist(EventDefine.Global.OnChangeTaskData, updateTaskList);
    }
    protected override void OnClose()
    {
        base.OnClose();
        EventManager.Instance.Remove(EventDefine.Global.OnChangeTaskData, updateTaskList);
    }
    private void updateTaskList()
    {
        showList.Clear();
        var d = getEventTask(TaskEventEnum.eTaskEvent1);
        if (d != null)
        {
            showList.Add(d);
        }
        d = getEventTask(TaskEventEnum.eTaskEvent2);
        if (d != null)
        {
            showList.Add(d);
        }
        d = getEventTask(TaskEventEnum.eTaskEvent3);
        if (d != null)
        {
            showList.Add(d);
        }
        d = getEventTask(TaskEventEnum.eTaskEvent4);
        if (d != null)
        {
            showList.Add(d);
        }
        d = getEventTask(TaskEventEnum.eTaskEvent5);
        if (d != null)
        {
            showList.Add(d);
        }
        showList.Clear();
        foreach (var item in items)
        {
            item.Widget.gameObject.SetActive(false);
        }
        var len = showList.Count;
        Utils.Unity.SetActive(this.title_GameObject, len > 0);
        for (int i = 0; i < len; i++)
        {
            var tpl = showList[i];
            var item = items[i];
            item.Widget.gameObject.SetActive(true);
            item.SetData(tpl.Id);
        }
    }
    private Task_Data getEventTask(TaskEventEnum type)
    {
        Task_Data show = null;
        if (taskEventMap.TryGetValue((int)type, out var list))
        {
            var len = list.Count;

            int minValue = int.MaxValue;
            for (int i = 0; i < len; i++)
            {
                var tpl = list[i];
                var data = TaskSystem.Instance.GetTaskData(tpl.Id);
                if (data == null)
                {

                    continue;
                }
                if (data.isReceiveAward)
                {
                    continue;
                }
                if (data.taskValue >= tpl.TaskValueParam)
                {
                    return tpl;
                }
                if (tpl.TaskValueParam < minValue)
                {
                    minValue = tpl.TaskValueParam;
                    show = tpl;
                }
            }
        }
        return show;
    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == click_Button)
        {
            // GameEntry.GUI.Open(GameEntry.GUIPath.AchievementTaskUI.Path, "tab", "task");
        }
    }

    public override void SetParam(string name, object value)
    {
        base.SetParam(name, value);
        if (name == "time")
        {
            if (value is string time)
            {
                this.time_TextMeshProUGUI.text = time;
            }
        }
    }

}