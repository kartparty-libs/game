
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class LoadingUI
{
	private UnityEngine.UI.Image progress_Image;
	private UnityEngine.UI.Text msg_Text;
	private UnityEngine.UI.Slider slider_Slider;

    protected override void InjectFields(GameObjectFields value)
    {
		this.progress_Image = (UnityEngine.UI.Image)value.FieldItems[0].Value;
		this.msg_Text = (UnityEngine.UI.Text)value.FieldItems[1].Value;
		this.slider_Slider = (UnityEngine.UI.Slider)value.FieldItems[2].Value;

    }
}
