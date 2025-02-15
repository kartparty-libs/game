
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class ScoreRankItem
{
	private UnityEngine.GameObject selfbg_GameObject;
	private UnityEngine.RectTransform rankbg_RectTransform;
	private UnityEngine.UI.Image headIcon_Image;
	private TMPro.TextMeshProUGUI rank_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI name_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI num_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI score_TextMeshProUGUI;

    protected override void InjectFields(GameObjectFields value)
    {
		this.selfbg_GameObject = (UnityEngine.GameObject)value.FieldItems[0].Value;
		this.rankbg_RectTransform = (UnityEngine.RectTransform)value.FieldItems[1].Value;
		this.headIcon_Image = (UnityEngine.UI.Image)value.FieldItems[2].Value;
		this.rank_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[3].Value;
		this.name_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[4].Value;
		this.num_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[5].Value;
		this.score_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[6].Value;

    }
}
