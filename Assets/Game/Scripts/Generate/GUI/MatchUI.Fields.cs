
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MatchUI
{
	private TMPro.TextMeshProUGUI matchNum_TextMeshProUGUI;
	private UnityEngine.RectTransform content_RectTransform;
	private UnityEngine.GameObject self_GameObject;
	private UnityEngine.RectTransform num1_RectTransform;
	private UnityEngine.RectTransform num2_RectTransform;
	private UnityEngine.UI.Button exit_Button;
	private UnityEngine.UI.Button start_Button;
	private UnityEngine.UI.Button ready_Button;

    protected override void InjectFields(GameObjectFields value)
    {
		this.matchNum_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[0].Value;
		this.content_RectTransform = (UnityEngine.RectTransform)value.FieldItems[1].Value;
		this.self_GameObject = (UnityEngine.GameObject)value.FieldItems[2].Value;
		this.num1_RectTransform = (UnityEngine.RectTransform)value.FieldItems[3].Value;
		this.num2_RectTransform = (UnityEngine.RectTransform)value.FieldItems[4].Value;
		this.exit_Button = (UnityEngine.UI.Button)value.FieldItems[5].Value;
		this.start_Button = (UnityEngine.UI.Button)value.FieldItems[6].Value;
		this.ready_Button = (UnityEngine.UI.Button)value.FieldItems[7].Value;
		this.exit_Button.onClick.AddListener(() => { this.OnButtonClick(this.exit_Button); });
		this.start_Button.onClick.AddListener(() => { this.OnButtonClick(this.start_Button); });
		this.ready_Button.onClick.AddListener(() => { this.OnButtonClick(this.ready_Button); });

    }
}
