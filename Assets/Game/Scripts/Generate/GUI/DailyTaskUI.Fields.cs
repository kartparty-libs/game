
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class DailyTaskUI
{
	private UnityEngine.GameObject items_GameObject;
	private UnityEngine.GameObject title_GameObject;
	private UnityEngine.UI.Button click_Button;
	private TMPro.TextMeshProUGUI time_TextMeshProUGUI;

    protected override void InjectFields(GameObjectFields value)
    {
		this.items_GameObject = (UnityEngine.GameObject)value.FieldItems[0].Value;
		this.title_GameObject = (UnityEngine.GameObject)value.FieldItems[1].Value;
		this.click_Button = (UnityEngine.UI.Button)value.FieldItems[2].Value;
		this.time_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[3].Value;
		this.click_Button.onClick.AddListener(() => { this.OnButtonClick(this.click_Button); });

    }
}
