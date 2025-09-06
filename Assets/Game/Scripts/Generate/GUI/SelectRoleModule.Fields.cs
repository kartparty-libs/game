
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class SelectRoleModule
{
	private UnityEngine.UI.Button click_Button;
	private UnityEngine.GameObject select_GameObject;
	private UnityEngine.GameObject dot_GameObject;
	private TMPro.TextMeshProUGUI lv_TextMeshProUGUI;

    protected override void InjectFields(GameObjectFields value)
    {
		this.click_Button = (UnityEngine.UI.Button)value.FieldItems[0].Value;
		this.select_GameObject = (UnityEngine.GameObject)value.FieldItems[1].Value;
		this.dot_GameObject = (UnityEngine.GameObject)value.FieldItems[2].Value;
		this.lv_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[3].Value;
		this.click_Button.onClick.AddListener(() => { this.OnButtonClick(this.click_Button); });

    }
}
