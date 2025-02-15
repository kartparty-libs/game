
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MainRankItem
{
	private TMPro.TextMeshProUGUI rank_TextMeshProUGUI;
	private UnityEngine.UI.Image icon_Image;
	private TMPro.TextMeshProUGUI name_TextMeshProUGUI;
	private UnityEngine.GameObject self_GameObject;

    protected override void InjectFields(GameObjectFields value)
    {
		this.rank_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[0].Value;
		this.icon_Image = (UnityEngine.UI.Image)value.FieldItems[1].Value;
		this.name_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[2].Value;
		this.self_GameObject = (UnityEngine.GameObject)value.FieldItems[3].Value;

    }
}
