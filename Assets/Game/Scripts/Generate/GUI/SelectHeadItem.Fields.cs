
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class SelectHeadItem
{
	private UnityEngine.UI.Button icon_Button;
	private UnityEngine.UI.Image icon_Image;
	private UnityEngine.GameObject selected_GameObject;

    protected override void InjectFields(GameObjectFields value)
    {
		this.icon_Button = (UnityEngine.UI.Button)value.FieldItems[0].Value;
		this.icon_Image = (UnityEngine.UI.Image)value.FieldItems[1].Value;
		this.selected_GameObject = (UnityEngine.GameObject)value.FieldItems[2].Value;
		this.icon_Button.onClick.AddListener(() => { this.OnButtonClick(this.icon_Button); });

    }
}
