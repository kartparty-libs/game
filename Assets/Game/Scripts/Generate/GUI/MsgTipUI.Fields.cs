
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MsgTipUI
{
	private TMPro.TextMeshProUGUI msg_TextMeshProUGUI;

    protected override void InjectFields(GameObjectFields value)
    {
		this.msg_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[0].Value;

    }
}
