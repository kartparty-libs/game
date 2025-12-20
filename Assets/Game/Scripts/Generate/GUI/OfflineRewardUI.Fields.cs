
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class OfflineRewardUI
{
	private UnityEngine.UI.Button close_Button;
	private UnityEngine.UI.Button ok_Button;
	private UnityEngine.RectTransform items_RectTransform;
	private TMPro.TextMeshProUGUI time_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI totalTime_TextMeshProUGUI;

    protected override void InjectFields(GameObjectFields value)
    {
		this.close_Button = (UnityEngine.UI.Button)value.FieldItems[0].Value;
		this.ok_Button = (UnityEngine.UI.Button)value.FieldItems[1].Value;
		this.items_RectTransform = (UnityEngine.RectTransform)value.FieldItems[2].Value;
		this.time_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[3].Value;
		this.totalTime_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[4].Value;
		this.close_Button.onClick.AddListener(() => { this.OnButtonClick(this.close_Button); });
		this.ok_Button.onClick.AddListener(() => { this.OnButtonClick(this.ok_Button); });

    }
}
