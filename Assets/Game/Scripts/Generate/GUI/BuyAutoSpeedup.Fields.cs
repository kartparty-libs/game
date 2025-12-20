
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class BuyAutoSpeedup
{
	private UnityEngine.UI.Button close_Button;
	private UnityEngine.UI.Button buy_Button;
	private UnityEngine.UI.Button done_Button;
	private UnityEngine.GameObject success_GameObject;
	private TMPro.TextMeshProUGUI cost_TextMeshProUGUI;

    protected override void InjectFields(GameObjectFields value)
    {
		this.close_Button = (UnityEngine.UI.Button)value.FieldItems[0].Value;
		this.buy_Button = (UnityEngine.UI.Button)value.FieldItems[1].Value;
		this.done_Button = (UnityEngine.UI.Button)value.FieldItems[2].Value;
		this.success_GameObject = (UnityEngine.GameObject)value.FieldItems[3].Value;
		this.cost_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[4].Value;
		this.close_Button.onClick.AddListener(() => { this.OnButtonClick(this.close_Button); });
		this.buy_Button.onClick.AddListener(() => { this.OnButtonClick(this.buy_Button); });
		this.done_Button.onClick.AddListener(() => { this.OnButtonClick(this.done_Button); });

    }
}
