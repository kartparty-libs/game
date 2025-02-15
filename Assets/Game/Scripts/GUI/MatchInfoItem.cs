
using System;
using Framework;
using UnityEngine;
using UnityEngine.UI;
public partial class MatchInfoItem : UIWidgetBase
{
    public CharacterCreateData Data { get; private set; }
    public Action<CharacterCreateData> OnKickClick;
    protected override void OnAwake()
    {
        base.OnAwake();
        Clear();
    }
    public void Clear()
    {
        Utils.Unity.SetActive(this.player_GameObject, false);
        Utils.Unity.SetActive(this.ready_GameObject, false);
        Utils.Unity.SetActive(this.kick_Button, false);
    }
    public void UpdateInfo(CharacterCreateData data)
    {
        this.Data = data;
        Utils.Unity.SetActive(this.player_GameObject, true);
        if (data.OwnerHide)
        {
            Utils.Unity.SetActive(this.player_GameObject, false);
            Utils.Unity.SetActive(this.ready_GameObject, false);
        }
        Utils.Unity.SetActive(this.ready_GameObject, data.isReady);
        Utils.Unity.SetActive(this.owner_GameObject, data.IsRoomOwner);
        this.name_TextMeshProUGUI.text = data.playerName;
        var headData = GameEntry.Table.Head.Get(data.head);
        GameEntry.Atlas.SetSprite(this.icon_Image, headData.BigIcon, false, true);
    }
    public void SetKickShow(bool value)
    {
        Utils.Unity.SetActive(this.kick_Button, value);
    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.kick_Button)
        {
            OnKickClick.Invoke(this.Data);
        }
    }
}