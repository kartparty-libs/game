
using Framework;
using UnityEngine;
using UnityEngine.UI;
using static EnumDefine;
public partial class DailyTaskItem : UIWidgetBase
{
    private int taskId;
    public void SetData(int id)
    {
        taskId = id;
        var data = TaskSystem.Instance.GetTaskData(id);
        var tpl = GameEntry.Table.Task.Get(id);
        var isCup = tpl.TaskEvent == (int)TaskEventEnum.eAccomplishTaskByEvent || tpl.TaskEvent == (int)TaskEventEnum.eAccomplishTaskByType;
        Utils.Unity.SetActive(item1_GameObject, isCup);
        Utils.Unity.SetActive(item0_GameObject, !isCup);
        if (isCup)
        {
        }
        else
        {

        }
        this.reward0_TextMeshProUGUI.text = tpl.TaskAward[1].ToString();
        this.reward1_TextMeshProUGUI.text = "";
        this.name_TextMeshProUGUI.text = tpl.Title;
        var targetvalue = tpl.TaskValueParam;
        var cvalue = Mathf.Clamp(data.taskValue, 0, targetvalue);
        var nowvalue = cvalue.ToString();
        if (tpl.TaskEvent == (int)TaskEventEnum.eOnlineTime)
        {
            nowvalue = string.Empty;
        }


        this.desc_TextMeshProUGUI.text = string.Format(tpl.TaskDesc, nowvalue, tpl.TaskValueParam);
        var completed = data.taskValue >= tpl.TaskValueParam;
        this.claim_Button.gameObject.SetActive(completed);
        this.desc_TextMeshProUGUI.color = completed ? new Color32(1, 239, 97, 255) : new Color32(255, 255, 255, 255);
    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        TaskSystem.Instance.TaskReceiveAward(taskId);
    }
}