
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class RankItem
{
	private UnityEngine.GameObject bg0_GameObject;
	private TMPro.TextMeshProUGUI rank_TextMeshProUGUI;
	private UnityEngine.UI.Image head_Image;
	private TMPro.TextMeshProUGUI name_TextMeshProUGUI;
	private UnityEngine.GameObject bg1_GameObject;
	private TMPro.TextMeshProUGUI time_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI count0_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI count1_TextMeshProUGUI;
	private UnityEngine.RectTransform rankimg_RectTransform;

    protected override void InjectFields(GameObjectFields value)
    {
		this.bg0_GameObject = (UnityEngine.GameObject)value.FieldItems[0].Value;
		this.rank_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[1].Value;
		this.head_Image = (UnityEngine.UI.Image)value.FieldItems[2].Value;
		this.name_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[3].Value;
		this.bg1_GameObject = (UnityEngine.GameObject)value.FieldItems[4].Value;
		this.time_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[5].Value;
		this.count0_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[6].Value;
		this.count1_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[7].Value;
		this.rankimg_RectTransform = (UnityEngine.RectTransform)value.FieldItems[8].Value;

    }
}
