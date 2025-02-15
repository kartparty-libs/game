
using Framework;
using System;
using UnityEngine;
using UnityEngine.UI;
public partial class SelectHeadItem : UIWidgetBase
{
    public int Data;
    private Action<SelectHeadItem> onSelect;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.selected_GameObject.SetActive(false);
    }
    public void UpdateData(int id, Action<SelectHeadItem> onSelect)
    {
        Data = id;
        var data = GameEntry.Table.Head.Get(id);
        GameEntry.Atlas.SetSprite(this.icon_Image, data.BigIcon);
        this.onSelect = onSelect;
    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.icon_Button)
        {
            this.onSelect.Invoke(this);
        }
    }
    public void SetSelect(bool value)
    {
        Utils.Unity.SetActive(this.selected_GameObject,value);
    }
}