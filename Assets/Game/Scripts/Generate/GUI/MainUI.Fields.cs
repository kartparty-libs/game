
//自动生成
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MainUI
{
	private UnityEngine.RectTransform bg_RectTransform;
	private UnityEngine.UI.Image power_Image;
	private TMPro.TextMeshProUGUI speed_TextMeshProUGUI;
	private UnityEngine.UI.Button close_Button;
	private UnityEngine.GameObject wait_GameObject;
	private TMPro.TextMeshProUGUI loopTxt_TextMeshProUGUI;
	private TMPro.TextMeshProUGUI time_TextMeshProUGUI;
	private VariableJoystick joystick_VariableJoystick;
	private UnityEngine.UI.Button jump_Button;
	private UnityEngine.UI.Button boost_Button;
	private VariableJoystick cameraJoystick_VariableJoystick;
	private UnityEngine.GameObject win_GameObject;
	private UnityEngine.UI.Button exit_Button;
	private UnityEngine.UI.Button yes_Button;
	private UnityEngine.UI.Button cancel_Button;
	private UnityEngine.GameObject exit_GameObject;
	private UIDragBehaviour drag_UIDragBehaviour;
	private TMPro.TextMeshProUGUI endTime_TextMeshProUGUI;
	private UnityEngine.RectTransform list_RectTransform;
	private UnityEngine.RectTransform n1_RectTransform;
	private UnityEngine.RectTransform n2_RectTransform;
	private UnityEngine.UI.Image boostcd_Image;

    protected override void InjectFields(GameObjectFields value)
    {
		this.bg_RectTransform = (UnityEngine.RectTransform)value.FieldItems[0].Value;
		this.power_Image = (UnityEngine.UI.Image)value.FieldItems[1].Value;
		this.speed_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[2].Value;
		this.close_Button = (UnityEngine.UI.Button)value.FieldItems[3].Value;
		this.wait_GameObject = (UnityEngine.GameObject)value.FieldItems[4].Value;
		this.loopTxt_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[5].Value;
		this.time_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[6].Value;
		this.joystick_VariableJoystick = (VariableJoystick)value.FieldItems[7].Value;
		this.jump_Button = (UnityEngine.UI.Button)value.FieldItems[8].Value;
		this.boost_Button = (UnityEngine.UI.Button)value.FieldItems[9].Value;
		this.cameraJoystick_VariableJoystick = (VariableJoystick)value.FieldItems[10].Value;
		this.win_GameObject = (UnityEngine.GameObject)value.FieldItems[11].Value;
		this.exit_Button = (UnityEngine.UI.Button)value.FieldItems[12].Value;
		this.yes_Button = (UnityEngine.UI.Button)value.FieldItems[13].Value;
		this.cancel_Button = (UnityEngine.UI.Button)value.FieldItems[14].Value;
		this.exit_GameObject = (UnityEngine.GameObject)value.FieldItems[15].Value;
		this.drag_UIDragBehaviour = (UIDragBehaviour)value.FieldItems[16].Value;
		this.endTime_TextMeshProUGUI = (TMPro.TextMeshProUGUI)value.FieldItems[17].Value;
		this.list_RectTransform = (UnityEngine.RectTransform)value.FieldItems[18].Value;
		this.n1_RectTransform = (UnityEngine.RectTransform)value.FieldItems[19].Value;
		this.n2_RectTransform = (UnityEngine.RectTransform)value.FieldItems[20].Value;
		this.boostcd_Image = (UnityEngine.UI.Image)value.FieldItems[21].Value;
		this.close_Button.onClick.AddListener(() => { this.OnButtonClick(this.close_Button); });
		this.jump_Button.onClick.AddListener(() => { this.OnButtonClick(this.jump_Button); });
		this.boost_Button.onClick.AddListener(() => { this.OnButtonClick(this.boost_Button); });
		this.exit_Button.onClick.AddListener(() => { this.OnButtonClick(this.exit_Button); });
		this.yes_Button.onClick.AddListener(() => { this.OnButtonClick(this.yes_Button); });
		this.cancel_Button.onClick.AddListener(() => { this.OnButtonClick(this.cancel_Button); });

    }
}
