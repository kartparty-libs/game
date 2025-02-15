
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class NicknameUI
{
	private TMPro.TMP_InputField nickName_TMP_InputField;
	private UnityEngine.UI.Button randomBtn_Button;
	private UnityEngine.UI.Button okBtn_Button;

    protected override void InjectFields(GameObjectFields value)
    {
		this.nickName_TMP_InputField = (TMPro.TMP_InputField)value.FieldItems[0].Value;
		this.randomBtn_Button = (UnityEngine.UI.Button)value.FieldItems[1].Value;
		this.okBtn_Button = (UnityEngine.UI.Button)value.FieldItems[2].Value;
		this.randomBtn_Button.onClick.AddListener(() => { this.OnButtonClick(this.randomBtn_Button); });
		this.okBtn_Button.onClick.AddListener(() => { this.OnButtonClick(this.okBtn_Button); });

    }
}
