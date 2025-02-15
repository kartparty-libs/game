
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class LoginUI
{
	private UnityEngine.GameObject input_GameObject;
	private UnityEngine.GameObject loging_GameObject;
	private UnityEngine.UI.InputField account_InputField;
	private TMPro.TextMeshProUGUI uid_TextMeshProUGUI;
	private UnityEngine.UI.Button login_Button;

    protected override void InjectFields(GameObjectFields value)
    {
		this.input_GameObject = (UnityEngine.GameObject)value.FieldItems[0].Value;
		this.loging_GameObject = (UnityEngine.GameObject)value.FieldItems[1].Value;
		this.account_InputField = (UnityEngine.UI.InputField)value.FieldItems[2].Value;
		this.uid_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[3].Value;
		this.login_Button = (UnityEngine.UI.Button)value.FieldItems[4].Value;
		this.login_Button.onClick.AddListener(() => { this.OnButtonClick(this.login_Button); });

    }
}
