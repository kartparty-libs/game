
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MatchCharacterInfo
{
	private UnityEngine.UI.Image icon_Image;
	private TMPro.TextMeshProUGUI name_TextMeshProUGUI;

    protected override void InjectFields(GameObjectFields value)
    {
		this.icon_Image = (UnityEngine.UI.Image)value.FieldItems[0].Value;
		this.name_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[1].Value;

    }
}
