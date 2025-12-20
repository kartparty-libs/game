
using System;
using System.Collections.Generic;
using Framework;
using Proto;
using UnityEngine.UI;
public partial class OfflineRewardUI : UIWindowBase
{
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.ok_Button)
        {
            Close();
        }
        else if (target == this.close_Button)
        {
            Close();
        }
    }
    public override void SetParam(string name, object value)
    {
        base.SetParam(name, value);
        if (value is ResMsgBodyCarCultivateSystem data)
        {
            var span = TimeSpan.FromMilliseconds(data.IntervalTime);
            var th = GameEntry.Table.Param.Get("OfflineEarningsTime").IntParam;
            if (span.TotalSeconds > 60 * 60 * th)
            {
                span = TimeSpan.FromSeconds(60 * 60 * th);
                this.totalTime_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1528, TimeUtils.GetRemainder(span.Milliseconds));
            }
            else
            {
                this.totalTime_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1528, TimeUtils.GetRemainder(data.IntervalTime));
            }
            this.time_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1418, TimeUtils.GetRemainder(data.IntervalTime));
            var len = this.items_RectTransform.childCount;
            var list = new List<long>();
            for (int i = 0; i < 6; i++)
            {
                list.Add(0);
            }
            list[0] = data.IntervalEarningsGold;

            foreach (var item in data.CarCultivateDatas)
            {
                if (item.IntervalLevelUp < 1)
                {
                    continue;
                }
                if (item.CarModuleTypeEnum == CarModuleTypeEnum.Car)
                {
                    // list[5] = item.Level;
                }
                else
                {
                    list[(int)item.CarModuleTypeEnum] = item.Level;
                }
            }

            for (var i = 0; i < len; i++)
            {
                var c = this.items_RectTransform.GetChild(i);
                var item = c.GetComponent<OfflineItem>();
                var n = list[i];
                if (i == 0)
                {
                    item.Text.text = "+" + FormatUtils.FormatMoney(n);
                }
                else
                {

                    Utils.Unity.SetActive(c, n > 0);
                    item.Text.text = "LV." + n;
                }
            }
        }
    }
}