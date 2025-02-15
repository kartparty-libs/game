
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class SelectRoleProperty
{
	private UnityEngine.UI.Image progress_Image;
	private TMPro.TextMeshProUGUI value_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI name_TextMeshProUGUI;
	private UnityEngine.RectTransform icon_RectTransform;

    protected override void InjectFields(GameObjectFields value)
    {
		this.progress_Image = (UnityEngine.UI.Image)value.FieldItems[0].Value;
		this.value_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[1].Value;
		this.name_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[2].Value;
		this.icon_RectTransform = (UnityEngine.RectTransform)value.FieldItems[3].Value;

    }
}
