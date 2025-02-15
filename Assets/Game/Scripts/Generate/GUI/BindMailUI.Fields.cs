
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class BindMailUI
{
	private UnityEngine.UI.Button okBtn_Button;
	private UnityEngine.GameObject success_GameObject;
	private UnityEngine.GameObject errormail_GameObject;
	private UnityEngine.GameObject errorcode_GameObject;
	private UnityEngine.GameObject errorname_GameObject;
	private UnityEngine.GameObject errorcheck_GameObject;
	private UnityEngine.UI.InputField nickname_InputField;
	private UnityEngine.UI.InputField mail_InputField;
	private UnityEngine.UI.InputField check_InputField;
	private UnityEngine.UI.InputField code_InputField;

    protected override void InjectFields(GameObjectFields value)
    {
		this.okBtn_Button = (UnityEngine.UI.Button)value.FieldItems[0].Value;
		this.success_GameObject = (UnityEngine.GameObject)value.FieldItems[1].Value;
		this.errormail_GameObject = (UnityEngine.GameObject)value.FieldItems[2].Value;
		this.errorcode_GameObject = (UnityEngine.GameObject)value.FieldItems[3].Value;
		this.errorname_GameObject = (UnityEngine.GameObject)value.FieldItems[4].Value;
		this.errorcheck_GameObject = (UnityEngine.GameObject)value.FieldItems[5].Value;
		this.nickname_InputField = (UnityEngine.UI.InputField)value.FieldItems[6].Value;
		this.mail_InputField = (UnityEngine.UI.InputField)value.FieldItems[7].Value;
		this.check_InputField = (UnityEngine.UI.InputField)value.FieldItems[8].Value;
		this.code_InputField = (UnityEngine.UI.InputField)value.FieldItems[9].Value;
		this.okBtn_Button.onClick.AddListener(() => { this.OnButtonClick(this.okBtn_Button); });

    }
}
