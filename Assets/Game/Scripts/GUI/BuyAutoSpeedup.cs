
using System;
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class BuyAutoSpeedup : UIWindowBase
{
    public static bool Tip = false;
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.buy_Button)
        {
            var cost = GameEntry.Table.Param.Get("BuyAutoUpgradeCost").IntParam;
            var now = PlayerSystem.Instance.GetDiamond();
            if (now < cost)
            {
                JumpUtils.GetMoney(1);
                return;
            }
            GameEntry.Http.Handler.BuyAutoUpgrade();
        }
        else if (target == this.close_Button)
        {
            Close();
        }
        else if (target == this.done_Button)
        {
            Close();
        }
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        Tip = false;
        OnCarCultivateChanged();
        EventManager.Instance.Regist(EventDefine.Global.OnCarCultivateChanged, OnCarCultivateChanged);
    }



    protected override void OnClose()
    {
        base.OnClose();
        EventManager.Instance.Remove(EventDefine.Global.OnCarCultivateChanged, OnCarCultivateChanged);
    }
    private void OnCarCultivateChanged()
    {
        Utils.Unity.SetActive(this.buy_Button, !CarCultivateSystem.Instance.Data.IsAutoUpgradeCar);
        Utils.Unity.SetActive(this.success_GameObject, CarCultivateSystem.Instance.Data.IsAutoUpgradeCar);
        var cost = GameEntry.Table.Param.Get("BuyAutoUpgradeCost").IntParam;
        var now = PlayerSystem.Instance.GetDiamond();
        this.cost_TextMeshProUGUI.text = "x" + cost;
        // Utils.Unity.SetActive(buy_Button, now >= cost);
    }
}