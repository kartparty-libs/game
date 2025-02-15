
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MainRankItem : UIWidgetBase
{
    public void SetRank(int value)
    {
        this.rank_TextMeshProUGUI.text = value.ToString();
    }
    public void SetData(CharacterInfo characterInfo)
    {
        this.name_TextMeshProUGUI.text = characterInfo.Name;
        var head = GameEntry.Table.Head.Get(characterInfo.HeadIconId);
        if (head != null)
        {
            GameEntry.Atlas.SetSprite(this.icon_Image, head.SmallIcon, false, true);
        }
        var isSelf = characterInfo.RoleId == PlayerSystem.Instance.GetUID();
        Utils.Unity.SetActive(this.self_GameObject, isSelf);
        this.rank_TextMeshProUGUI.color = this.name_TextMeshProUGUI.color = isSelf ? Color.black : Color.white;
        this.icon_Image.rectTransform.sizeDelta = new Vector2(isSelf?53:45, isSelf ? 53 : 45);
    }

}