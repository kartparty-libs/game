
using Framework;
using UnityEngine;
using UnityEngine.UI;
using static EnumDefine;
public partial class ScoreRankItem : UIWidgetBase
{
    public int Rank;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.num_TextMeshProUGUI.enableAutoSizing = true;
        this.num_TextMeshProUGUI.fontSizeMin = 18;
        this.num_TextMeshProUGUI.fontSizeMax = 32;
    }
    public void SetSelfData(ValueRankData data)
    {
        if (data == null)
        {
            Utils.Unity.SetActive(this.Widget.gameObject, false);
            return;
        }
        Utils.Unity.SetActive(this.Widget.gameObject, true);
        this.Rank = data.Rank;
        SetData(data, data.Rank);
    }
    public void SetData(ValueRankData data, int myrank)
    {
        this.Rank = data.Rank;
        if (Rank < 1)
        {
            Utils.Unity.SetActive(this.Widget.gameObject, false);
            return;
        }
        var showid = Mathf.Clamp(Rank - 1, 0, 3);
        for (int i = 0; i < 4; i++)
        {
            var c = this.rankbg_RectTransform.GetChild(i);
            Utils.Unity.SetActive(c, i == showid);
        }
        Utils.Unity.SetActive(this.Widget.gameObject, true);
        Utils.Unity.SetActive(selfbg_GameObject, Rank == myrank);
        if (Rank < 4)
        {
            this.rank_TextMeshProUGUI.text = "";
        }
        else
        {
            if (Rank > 2000)
            {
                this.rank_TextMeshProUGUI.text = "2000+";
            }
            else
            {
                this.rank_TextMeshProUGUI.text = Rank.ToString();
            }

        }
        if (!string.IsNullOrEmpty(data.Info.email))
        {
            this.num_TextMeshProUGUI.text = formatEmail(data.Info.email);
        }
        else
        {
            this.num_TextMeshProUGUI.text = "";
        }
        this.score_TextMeshProUGUI.text = FormatUtils.FormatMoney(data.Value);
        this.name_TextMeshProUGUI.text = data.Info.name;
        var headTpl = GameEntry.Table.Head.Get(data.Info.headId);
        if (headTpl != null)
        {
            GameEntry.Atlas.SetSprite(this.headIcon_Image, headTpl.BigIcon, false, true);
        }
    }
    private string formatEmail(string value)
    {
        var code = value;
        var codel = code.Split("@");
        if (codel.Length > 1)
        {
            var p1 = codel[0];
            var len = p1.Length;
            code = "";
            for (int i = 0; i < len; i++)
            {
                var p2 = p1[i];
                if (i < 2)
                {
                    code += p2;
                }
                else
                {
                    code += "*";
                }
            }
            for (int i = 1; i < codel.Length; i++)
            {
                code += "@" + codel[i];
            }
        }
        return code;
    }
    /*
    public void SetData(RankData rankData)
    {
        this.Rank = rankData.rank;
        Utils.Unity.SetActive(this.Widget.gameObject, true);
        // if (Rank > 2500)
        // {
        //     this.rank_TextMeshProUGUI.text = "2500+";
        // }
        // else
        {
            this.rank_TextMeshProUGUI.text = Rank.ToString();
        }
        Utils.Unity.SetActive(selfbg_GameObject, true);
        this.name_TextMeshProUGUI.text = rankData.name;
        this.score_TextMeshProUGUI.text = rankData.score.ToString();
        var code = rankData.email;
        var codel = code.Split("@");
        if (codel.Length > 1)
        {
            var p1 = codel[0];
            var len = p1.Length;
            code = "";
            for (int i = 0; i < len; i++)
            {
                var p2 = p1[i];
                if (i < 2)
                {
                    code += p2;
                }
                else
                {
                    code += "*";
                }
            }
            for (int i = 1; i < codel.Length; i++)
            {
                code += "@" + codel[i];
            }
        }

        this.num_TextMeshProUGUI.text = code;
        var showid = Mathf.Clamp(Rank - 1, 0, 3);
        for (int i = 0; i < 4; i++)
        {
            var c = this.rankbg_RectTransform.GetChild(i);
            Utils.Unity.SetActive(c, i == showid);
        }
        var headTpl = GameEntry.Table.Head.Get(rankData.head);
        if (headTpl != null)
        {
            GameEntry.Atlas.SetSprite(this.headIcon_Image, headTpl.BigIcon, false, true);
        }
    }
    */
}