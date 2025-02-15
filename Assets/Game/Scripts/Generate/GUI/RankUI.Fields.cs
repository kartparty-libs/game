
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class RankUI
{
	private UnityEngine.RectTransform Content_RectTransform;
	private Framework.GameObjectFields item_GameObjectFields;
	private SuperScrollView.LoopListView2 Scroll_View_LoopListView2;
	private UnityEngine.UI.Button return_Button;
	private UnityEngine.UI.Button close_Button;
	private UnityEngine.UI.Button next_Button;

    protected override void InjectFields(GameObjectFields value)
    {
		this.Content_RectTransform = (UnityEngine.RectTransform)value.FieldItems[0].Value;
		this.item_GameObjectFields = (Framework.GameObjectFields)value.FieldItems[1].Value;
		this.Scroll_View_LoopListView2 = (SuperScrollView.LoopListView2)value.FieldItems[2].Value;
		this.return_Button = (UnityEngine.UI.Button)value.FieldItems[3].Value;
		this.close_Button = (UnityEngine.UI.Button)value.FieldItems[4].Value;
		this.next_Button = (UnityEngine.UI.Button)value.FieldItems[5].Value;
		this.return_Button.onClick.AddListener(() => { this.OnButtonClick(this.return_Button); });
		this.close_Button.onClick.AddListener(() => { this.OnButtonClick(this.close_Button); });
		this.next_Button.onClick.AddListener(() => { this.OnButtonClick(this.next_Button); });

    }
}
