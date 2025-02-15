
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class ScoreRankUI
{
	private UnityEngine.UI.Button close_Button;
	private SuperScrollView.LoopListView2 list_LoopListView2;
	private UnityEngine.UI.Button back_Button;
	private UnityEngine.GameObject myrank_GameObject;
	private TMPro.TextMeshProUGUI time_TextMeshProUGUI;
	private UnityEngine.GameObject tip_GameObject;
	private UnityEngine.UI.Button info_Button;
	private UnityEngine.UI.Button rule_Button;

    protected override void InjectFields(GameObjectFields value)
    {
		this.close_Button = (UnityEngine.UI.Button)value.FieldItems[0].Value;
		this.list_LoopListView2 = (SuperScrollView.LoopListView2)value.FieldItems[1].Value;
		this.back_Button = (UnityEngine.UI.Button)value.FieldItems[2].Value;
		this.myrank_GameObject = (UnityEngine.GameObject)value.FieldItems[3].Value;
		this.time_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[4].Value;
		this.tip_GameObject = (UnityEngine.GameObject)value.FieldItems[5].Value;
		this.info_Button = (UnityEngine.UI.Button)value.FieldItems[6].Value;
		this.rule_Button = (UnityEngine.UI.Button)value.FieldItems[7].Value;
		this.close_Button.onClick.AddListener(() => { this.OnButtonClick(this.close_Button); });
		this.back_Button.onClick.AddListener(() => { this.OnButtonClick(this.back_Button); });
		this.info_Button.onClick.AddListener(() => { this.OnButtonClick(this.info_Button); });
		this.rule_Button.onClick.AddListener(() => { this.OnButtonClick(this.rule_Button); });

    }
}
