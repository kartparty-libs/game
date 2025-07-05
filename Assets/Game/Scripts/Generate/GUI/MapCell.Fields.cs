
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MapCell
{
	private UnityEngine.UI.Image icon_Image;
	private UnityEngine.GameObject lock_GameObject;
	private TMPro.TextMeshProUGUI lv_TextMeshProUGUI;

    protected override void InjectFields(GameObjectFields value)
    {
		this.icon_Image = (UnityEngine.UI.Image)value.FieldItems[0].Value;
		this.lock_GameObject = (UnityEngine.GameObject)value.FieldItems[1].Value;
		this.lv_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[2].Value;

    }
}
