
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MsgTipUI : UIWindowBase
{
    private float time;
    public override void SetParam(string name, object value)
    {
        base.SetParam(name, value);
        if (value is int langId)
        {
            this.msg_TextMeshProUGUI.text = GameEntry.Table.Lang.GetText(langId);
            time = 2f;
        }
        else if (value is string txt)
        {
            if (!string.IsNullOrEmpty(txt))
            {
                this.msg_TextMeshProUGUI.text = txt;
                time = 2f;
                return;
            }else
            {
                Close();
            }
        }
    }
    protected override void OnOpen()
    {
        base.OnOpen();
    }
    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (time > 0)
        {
            time -= deltaTime;
            if (time <= 0f)
            {
                Close();
            }
        }


    }
}