
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class NoticeUI
{
	private TMPro.TextMeshProUGUI msg_TextMeshProUGUI;
	private UnityEngine.RectTransform size_RectTransform;

    protected override void InjectFields(GameObjectFields value)
    {
		this.msg_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[0].Value;
		this.size_RectTransform = (UnityEngine.RectTransform)value.FieldItems[1].Value;

    }
}
