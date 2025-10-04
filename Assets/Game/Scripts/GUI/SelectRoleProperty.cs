
using Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public partial class SelectRoleProperty : UIWidgetBase
{
    public int Id;
    private int _value;
    public string Value;
    public string Base;
    public string Add;
    public string NextLvAddValue;
    public void Clear()
    {
        this.name_TextMeshProUGUI.text = string.Empty;
        this.value_TextMeshProUGUI.text = "0";
        this.progress_Image.fillAmount = 0;
        _value = 0;
    }
    public void Init(int id)
    {
        this.name_TextMeshProUGUI.text = GameEntry.Table.Lang.GetText(91000 + id);
        this.Id = id;
        var len = this.icon_RectTransform.childCount;
        while (len-- > 0)
        {
            var t = this.icon_RectTransform.GetChild(len);
            Utils.Unity.SetActive(t, len == (this.Id - 1));
        }
        Refresh();
    }
    public void Refresh()
    {
        float value = 0f;
        float min = 0f;
        float max = 0f;
        float nextValue = 0f;
        var lv = CarCultivateSystem.Instance.GetModuleLevel((CarModuleType)this.Id);
        var tpl = GameEntry.Table.CarUpgrade.Get(lv);
        var next = GameEntry.Table.CarUpgrade.Get(lv + 1);
        if (Id == (int)CarModuleType.Module1)
        {
            value = tpl.Property1;
            min = GameEntry.Table.CarUpgrade.MinLevelData.Property1;
            max = GameEntry.Table.CarUpgrade.MaxLevelData.Property1;
            if (next != null)
            {
                nextValue = next.Property1;
            }
        }
        else if (Id == (int)CarModuleType.Module2)
        {
            value = tpl.Property2;
            min = GameEntry.Table.CarUpgrade.MinLevelData.Property2;
            max = GameEntry.Table.CarUpgrade.MaxLevelData.Property2;
            if (next != null)
            {
                nextValue = next.Property2;
            }
        }
        else if (Id == (int)CarModuleType.Module3)
        {
            value = tpl.Property3;
            min = GameEntry.Table.CarUpgrade.MinLevelData.Property3;
            max = GameEntry.Table.CarUpgrade.MaxLevelData.Property3;
            if (next != null)
            {
                nextValue = next.Property3;
            }
        }
        else if (Id == (int)CarModuleType.Module4)
        {
            value = tpl.Property4;
            min = GameEntry.Table.CarUpgrade.MinLevelData.Property4;
            max = GameEntry.Table.CarUpgrade.MaxLevelData.Property4;
            if (next != null)
            {
                nextValue = next.Property4;
            }
        }

        if (nextValue < 1)
        {
            nextValue = value;
        }


        if (Id != (int)CarModuleType.Module1)
        {
            Value = (value * 0.1f).ToString("0.##");
            Base = (min * 0.1f).ToString("0.##") + "%";
            Add = ((value - min) * 0.1f).ToString("0.##") + "%";
            this.value_TextMeshProUGUI.text = "<color #00ff00>" + (value * 0.1f).ToString("0.##") + "%</color>(" + (min * 0.1f).ToString("0.##") + "+<color #00ff00>" + ((value - min) * 0.1f).ToString("0.##") + "</color>)";
            NextLvAddValue = ((nextValue - value) * 0.1f).ToString("0.##");
        }
        else
        {
            Value = value.ToString();
            Base = min.ToString();
            Add = (value - min).ToString();
            this.value_TextMeshProUGUI.text = "<color #00ff00>" + value.ToString() + "</color>(" + min + "+<color #00ff00>" + (value - min) + "</color>)";
            NextLvAddValue = (nextValue - value).ToString();
        }

        var total = max - min;
        if (total > 0)
        {
            this.progress_Image.fillAmount = (value - min) / total;
        }

    }
}