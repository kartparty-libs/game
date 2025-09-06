
using Framework;
using Framework.Core;
using UnityEngine;
using UnityEngine.UI;
public partial class SelectRoleModule : UIWidgetBase
{
    private bool _select = false;
    private bool _selected = false;
    public int Id { get; private set; }
    public bool Enable { get; private set; }
    public void Clear()
    {
        this.select_GameObject.SetActive(false);
    }
    public void Init(int Id)
    {
        this.Id = Id;
        Utils.Unity.SetActive(this.dot_GameObject, false);
        Refresh();
    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        // if (target == this.click_Button)
        // {
        //     if (this.Id < 1) return;
        //     var e = ReferencePool.Get<GUIEvent>();
        //     e.IntValue = this.Id;
        //     e.EventType = GUIEvent.OnModuleSelect;
        //     GameEntry.GUIEvent.DispatchEvent(e);
        // }
    }
    public void Refresh()
    {
        Enable = GameEntry.OfflineManager.OfflineReward.HasModuleUpgrade((CarModuleType)this.Id);
        Utils.Unity.SetActive(this.select_GameObject, Enable);
        var lv = CarCultivateSystem.Instance.GetModuleLevel((CarModuleType)this.Id);
        this.lv_TextMeshProUGUI.text = "lv." + lv;
    }
    public void Select(bool value)
    {
        Utils.Unity.SetActive(this.dot_GameObject, value);
    }
}