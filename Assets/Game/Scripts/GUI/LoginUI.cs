
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class LoginUI : UIWindowBase
{
    public const string HasAccount = nameof(HasAccount);
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.login_Button)
        {
            Utils.Unity.SetActive(loging_GameObject, true);
            // GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", "1.clicklogin");

#if UNITY_EDITOR

#else
           JavascriptBridge.Call(JavascriptBridge.UnityLogin);return;
#endif
            // Screen.fullScreen = true;
            var account = this.account_InputField.text;
            if (string.IsNullOrEmpty(account))
            {
                return;
            }
            GameEntry.Context.KartKey = account;
            GameEntry.Context.Account = account;
            GameEntry.Context.NickName = account;
            GameEntry.Context.BindMailName = account + "@qq.com";
            GameEntry.OfflineManager.Login(account);
            StorageManager.Instance.SetString(EnumDefine.LocalStorage.Login_Account.ToString(), this.account_InputField.text);
        }
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        this.uid_TextMeshProUGUI.text = "";
        Utils.Unity.SetActive(input_GameObject, false);
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        // GameEntry.GUI.Open(GameEntry.GUIPath.MsgTipUI.Path, "msg", "0.showlogin");

        string sAccount = StorageManager.Instance.GetString(EnumDefine.LocalStorage.Login_Account.ToString());
        this.account_InputField.text = sAccount;
        Utils.Unity.SetActive(loging_GameObject, false);
#if UNITY_EDITOR
        Utils.Unity.SetActive(input_GameObject, true);
#else
        Utils.Unity.SetActive(input_GameObject,false);    
#endif
        if (GameEntry.Context.DebugMode)
        {
            Utils.Unity.SetActive(input_GameObject, true);
        }

    }
    public override void SetParam(string name, object value)
    {
        base.SetParam(name, value);
        if (name == HasAccount)
        {
            Utils.Unity.SetActive(input_GameObject, false);
            Utils.Unity.SetActive(login_Button, false);
            if (value is string uid)
            {
                this.uid_TextMeshProUGUI.text = uid;
            }

        }
    }
}