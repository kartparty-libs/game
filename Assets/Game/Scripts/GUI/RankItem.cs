
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class RankItem : UIWidgetBase
{
    public void SetData(MatchCharacterData data, int index)
    {
        var maptpl = GameEntry.Context.Gameplay.TableData;
        var rank = data.Rank;
        var isSelf = data.RoleID == PlayerSystem.Instance.GetUID();
        this.bg1_GameObject.SetActive(isSelf);
        // this.gift_GameObject.SetActive(rank < 4);
        Utils.Unity.SetActive(this.rank_TextMeshProUGUI, data.Goal);
        this.rank_TextMeshProUGUI.text = (rank).ToString();
        var txtColor = isSelf ? Color.black : Color.white;
        this.rank_TextMeshProUGUI.color = txtColor;
        this.name_TextMeshProUGUI.color = txtColor;
        this.count0_TextMeshProUGUI.color = txtColor;
        this.count1_TextMeshProUGUI.color = txtColor;
        this.name_TextMeshProUGUI.text = data.Name;
        // this.time_TextMeshProUGUI.text = data.Time.ToString();
        var head = GameEntry.Table.Head.Get(data.Head);
        if (head != null)
        {
            GameEntry.Atlas.SetSprite(head_Image, head.SmallIcon, false, true);
            if (isSelf)
            {
                this.head_Image.rectTransform.sizeDelta = new Vector2(53, 53);
            }
            else
            {
                this.head_Image.rectTransform.sizeDelta = new Vector2(44, 44);
            }
        }
        if (GameEntry.Context.MatchMode)
        {
            Utils.Unity.SetActive(this.count0_TextMeshProUGUI.transform.parent, true);
            var info = MapSystem.Instance.GetOterPvpRankReward(data.RankTiersId, data.Goal ? rank : -1);
            // info.Gold
            // info.Token
            this.count0_TextMeshProUGUI.text = FormatUtils.FormatMoney(info.Gold);
            this.count1_TextMeshProUGUI.text = FormatUtils.FormatKMoney(info.Token);
            if (rank > 0 && rank < 4)
            {
                Utils.Unity.SetActive(this.rankimg_RectTransform, data.Goal);
                Utils.Unity.SetActive(this.rank_TextMeshProUGUI, false);
                var len = this.rankimg_RectTransform.childCount;
                while (len-- > 0)
                {
                    Utils.Unity.SetActive(this.rankimg_RectTransform.GetChild(len), len == rank - 1);
                }
            }
            else
            {
                Utils.Unity.SetActive(this.rankimg_RectTransform, false);
            }

        }
        else
        {
            Utils.Unity.SetActive(this.count0_TextMeshProUGUI.transform.parent, false);
            Utils.Unity.SetActive(this.rankimg_RectTransform, false);
        }




    }
}