
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MatchInfoItem
{
	private UnityEngine.GameObject player_GameObject;
	private UnityEngine.UI.Image icon_Image;
	private TMPro.TextMeshProUGUI name_TextMeshProUGUI;
	private UnityEngine.GameObject ready_GameObject;
	private UnityEngine.UI.Button kick_Button;
	private UnityEngine.GameObject owner_GameObject;

    protected override void InjectFields(GameObjectFields value)
    {
		this.player_GameObject = (UnityEngine.GameObject)value.FieldItems[0].Value;
		this.icon_Image = (UnityEngine.UI.Image)value.FieldItems[1].Value;
		this.name_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[2].Value;
		this.ready_GameObject = (UnityEngine.GameObject)value.FieldItems[3].Value;
		this.kick_Button = (UnityEngine.UI.Button)value.FieldItems[4].Value;
		this.owner_GameObject = (UnityEngine.GameObject)value.FieldItems[5].Value;
		this.kick_Button.onClick.AddListener(() => { this.OnButtonClick(this.kick_Button); });

    }
}
