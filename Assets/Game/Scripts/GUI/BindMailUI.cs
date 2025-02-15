
using Framework;
using Framework.Core;
using Proto;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
public partial class BindMailUI : UIWindowBase
{
    protected override void OnAwake()
    {
        base.OnAwake();
        this.check_InputField.onValueChanged.AddListener(mailEdit);
        this.mail_InputField.onValueChanged.AddListener(mailEdit);
        this.code_InputField.onValueChanged.AddListener(oncodeEdit);
        this.nickname_InputField.onValueChanged.AddListener(onNameEdit);
    }

    private void onNameEdit(string arg0)
    {
        var nickName = this.nickname_InputField.text;
        Utils.Unity.SetActive(errorname_GameObject, string.IsNullOrEmpty(nickName));
    }

    private void oncodeEdit(string arg0)
    {
        var code = this.code_InputField.text;
        Utils.Unity.SetActive(errorcode_GameObject, string.IsNullOrEmpty(code));
    }

    private void mailEdit(string txt)
    {
        var mail = this.mail_InputField.text.Trim();
        bool isMail = false;
        if (!string.IsNullOrEmpty(mail))
        {
            isMail = IsValidEmail(mail);
        }
        Utils.Unity.SetActive(errormail_GameObject, !isMail);
        var check = this.check_InputField.text.Trim();
        if (isMail && !string.IsNullOrEmpty(check))
        {
            Utils.Unity.SetActive(errorcheck_GameObject, check != mail);
            if (check != mail)
            {
                this.check_InputField.textComponent.color = Color.red;
            }
            else
            {
                this.check_InputField.textComponent.color = Color.white;
            }
            Utils.Unity.SetActive(success_GameObject, check == mail);
        }
        else
        {
            Utils.Unity.SetActive(errorcheck_GameObject, false);
            Utils.Unity.SetActive(success_GameObject, false);
        }


    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.okBtn_Button)
        {
            var nickName = this.nickname_InputField.text;
            if (string.IsNullOrEmpty(nickName))
            {
                Utils.Unity.SetActive(errorname_GameObject, true);
                return;
            }
            var mail = this.mail_InputField.text.Trim();
            if (!IsValidEmail(mail))
            {
                Utils.Unity.SetActive(errormail_GameObject, true);
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", 14);
                return;
            }
            var check = this.check_InputField.text.Trim();
            if (check != mail)
            {
                GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", 15);
                this.check_InputField.textComponent.color = Color.red;
                return;
            }
            var code = this.code_InputField.text;
            if (string.IsNullOrEmpty(code))
            {
                Utils.Unity.SetActive(errorcode_GameObject, true);
                return;
            }

            Utils.Unity.SetActive(success_GameObject, false);
            Utils.Unity.SetActive(errormail_GameObject, false);
            Utils.Unity.SetActive(errorcheck_GameObject, false);
            Utils.Unity.SetActive(errorcode_GameObject, false);
            Utils.Unity.SetActive(errorname_GameObject, false);
            GameEntry.Http.Handler.Register(GameEntry.Context.Account, mail, code, nickName, GameEntry.Context.InviteAccount);
        }
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        Utils.Unity.SetActive(success_GameObject, false);
        Utils.Unity.SetActive(errormail_GameObject, false);
        Utils.Unity.SetActive(errorcheck_GameObject, false);
        Utils.Unity.SetActive(errorcode_GameObject, false);
        Utils.Unity.SetActive(errorname_GameObject, false);
        if (!string.IsNullOrEmpty(GameEntry.Context.NickName))
        {
            this.nickname_InputField.text = GameEntry.Context.NickName;
        }
        GameEntry.GUIEvent.AddEventListener(GUIEvent.OnErrorCode, onErrorCode);
    }

    private void onErrorCode(IEventData eventData)
    {
        if (eventData is GUIEvent e)
        {
            if (e.IntValue == (int)ResponseCodeEnum.InvalidEmail)
            {
                Utils.Unity.SetActive(errormail_GameObject, true);
            }
            else if (e.IntValue == (int)ResponseCodeEnum.InvalidName)
            {
                Utils.Unity.SetActive(errorname_GameObject, true);
            }
            else if (e.IntValue == (int)ResponseCodeEnum.NotFindKartKey)
            {
                Utils.Unity.SetActive(errorcode_GameObject, true);
            }
            else if (e.IntValue == (int)ResponseCodeEnum.KartKeyDuplication)
            {
                Utils.Unity.SetActive(errorcode_GameObject, true);
            }
        }
    }

    protected override void OnClose()
    {
        base.OnClose();
        GameEntry.GUIEvent.RemoveEventListener(GUIEvent.OnErrorCode, onErrorCode);
    }
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }
        string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
        Regex rgx = new Regex(pattern);
        return rgx.IsMatch(email);
    }
}