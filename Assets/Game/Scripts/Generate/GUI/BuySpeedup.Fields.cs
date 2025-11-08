
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class BuySpeedup
{
	private UnityEngine.UI.Button slot1_Button;
	private UnityEngine.UI.Button slot2_Button;
	private UnityEngine.UI.Button slot3_Button;
	private UnityEngine.UI.Button close_Button;
	private TMPro.TextMeshProUGUI finishTime_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI freeCDTime_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI money_TextMeshProUGUI;
	private UnityEngine.UI.Button add_Button;
	private TMPro.TextMeshProUGUI time2_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI time1_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI cost1_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI cost2_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI time0_TextMeshProUGUI;

    protected override void InjectFields(GameObjectFields value)
    {
		this.slot1_Button = (UnityEngine.UI.Button)value.FieldItems[0].Value;
		this.slot2_Button = (UnityEngine.UI.Button)value.FieldItems[1].Value;
		this.slot3_Button = (UnityEngine.UI.Button)value.FieldItems[2].Value;
		this.close_Button = (UnityEngine.UI.Button)value.FieldItems[3].Value;
		this.finishTime_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[4].Value;
		this.freeCDTime_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[5].Value;
		this.money_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[6].Value;
		this.add_Button = (UnityEngine.UI.Button)value.FieldItems[7].Value;
		this.time2_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[8].Value;
		this.time1_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[9].Value;
		this.cost1_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[10].Value;
		this.cost2_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[11].Value;
		this.time0_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[12].Value;
		this.slot1_Button.onClick.AddListener(() => { this.OnButtonClick(this.slot1_Button); });
		this.slot2_Button.onClick.AddListener(() => { this.OnButtonClick(this.slot2_Button); });
		this.slot3_Button.onClick.AddListener(() => { this.OnButtonClick(this.slot3_Button); });
		this.close_Button.onClick.AddListener(() => { this.OnButtonClick(this.close_Button); });
		this.add_Button.onClick.AddListener(() => { this.OnButtonClick(this.add_Button); });

    }
}
