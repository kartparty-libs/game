
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class SelectHeadUI
{
	private UnityEngine.UI.Button ok_Button;
	private TMPro.TMP_Dropdown dropdown_TMP_Dropdown;
	private SuperScrollView.LoopListView2 list_LoopListView2;
	private UnityEngine.UI.Button back_Button;
	private TMPro.TextMeshProUGUI id_TextMeshProUGUI;
	private UnityEngine.UI.Button prerelase_Button;
	private UnityEngine.UI.Button debug_Button;
	private UnityEngine.GameObject debug1_GameObject;

    protected override void InjectFields(GameObjectFields value)
    {
		this.ok_Button = (UnityEngine.UI.Button)value.FieldItems[0].Value;
		this.dropdown_TMP_Dropdown = (TMPro.TMP_Dropdown)value.FieldItems[1].Value;
		this.list_LoopListView2 = (SuperScrollView.LoopListView2)value.FieldItems[2].Value;
		this.back_Button = (UnityEngine.UI.Button)value.FieldItems[3].Value;
		this.id_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[4].Value;
		this.prerelase_Button = (UnityEngine.UI.Button)value.FieldItems[5].Value;
		this.debug_Button = (UnityEngine.UI.Button)value.FieldItems[6].Value;
		this.debug1_GameObject = (UnityEngine.GameObject)value.FieldItems[7].Value;
		this.ok_Button.onClick.AddListener(() => { this.OnButtonClick(this.ok_Button); });
		this.back_Button.onClick.AddListener(() => { this.OnButtonClick(this.back_Button); });
		this.prerelase_Button.onClick.AddListener(() => { this.OnButtonClick(this.prerelase_Button); });
		this.debug_Button.onClick.AddListener(() => { this.OnButtonClick(this.debug_Button); });

    }
}
