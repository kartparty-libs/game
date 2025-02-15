
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class DailyTaskItem
{
	private TMPro.TextMeshProUGUI name_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI desc_TextMeshProUGUI;
	private UnityEngine.UI.Button claim_Button;
	private UnityEngine.GameObject item0_GameObject;
	private UnityEngine.GameObject item1_GameObject;
	private TMPro.TextMeshProUGUI reward0_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI reward1_TextMeshProUGUI;

    protected override void InjectFields(GameObjectFields value)
    {
		this.name_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[0].Value;
		this.desc_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[1].Value;
		this.claim_Button = (UnityEngine.UI.Button)value.FieldItems[2].Value;
		this.item0_GameObject = (UnityEngine.GameObject)value.FieldItems[3].Value;
		this.item1_GameObject = (UnityEngine.GameObject)value.FieldItems[4].Value;
		this.reward0_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[5].Value;
		this.reward1_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[6].Value;
		this.claim_Button.onClick.AddListener(() => { this.OnButtonClick(this.claim_Button); });

    }
}
