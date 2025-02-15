
using System.Text.RegularExpressions;
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class NicknameUI : UIWindowBase
{
    private int HeadIconId;
    private bool isValid;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.nickName_TMP_InputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }
    private void OnInputFieldValueChanged(string value)
    {
        //bool containsSpecialChar = Regex.IsMatch(value, @"[^a-zA-Z0-9\s]");
        //if (containsSpecialChar)
        //{
        //    isValid = false;
        //}
        //else
        //{
        //    isValid = true;
        //}
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        randomName();
    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.randomBtn_Button)
        {
            randomName();

        }
        else if (target == this.okBtn_Button)
        {
            //send
            //if (!isValid)
            //{
            //    GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", 8);
            //    return;
            //}
            var name = this.nickName_TMP_InputField.text;
            if (!string.IsNullOrEmpty(name))
            {
                // this.Close();
                if (!GameEntry.Context.OfflineMode)
                {
                    GameEntry.Net.Send(CtoS.K_CreateCharReqMsg, name, GameEntry.Context.BindMailName, 1);
                }
                else
                {
                    GameEntry.OfflineManager.SetNickName(name);
                }
            }
        }
    }
    private void randomName()
    {
        var len = GameEntry.Table.Nickname.ItemCount;
        var f = GameEntry.Table.Nickname.GetItem(Random.Range(0, len));
        var name = f.FirstName;
        var m = GameEntry.Table.Nickname.GetItem(Random.Range(0, len));
        if (!string.IsNullOrEmpty(m.MiddleName))
        {
            name += m.MiddleName;
        }
        var l = GameEntry.Table.Nickname.GetItem(Random.Range(0, len));
        if (!string.IsNullOrEmpty(l.LastName))
        {
            name += l.LastName;
        }
        this.nickName_TMP_InputField.text = name;
    }
}