
using Framework;
using Framework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public partial class MatchUI : UIWindowBase
{
    private List<MatchInfoItem> matchInfoItems = new List<MatchInfoItem>();
    private List<CharacterCreateData> joinPlayers = new List<CharacterCreateData>();
    private bool _hasStart;
    private bool _matchplayerFinish;
    protected override void OnAwake()
    {
        base.OnAwake();

        var len = this.content_RectTransform.childCount;
        for (int i = 0; i < len; i++)
        {
            var w = content_RectTransform.GetChild(i).GetComponent<UGUIWidget>().Widget;
            if (w != null)
            {
                if (w is MatchInfoItem info)
                {
                    matchInfoItems.Add(info);
                }
            }
        }
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        _matchplayerFinish = false;
        GameEntry.GUIEvent.AddEventListener(GUIEvent.OnRoomPlayerInfoChange, OnReceiverMatchPlayer);
        GameEntry.GUIEvent.AddEventListener(GUIEvent.OnStartMateCompeComplete, OnMatchComplete);
        var maxNum = GameEntry.Table.Map.Get(MatchSystem.Instance.GetMapCfgId()).LineMaxPlayerNum;
        this.matchNum_TextMeshProUGUI.text = string.Empty;
        var len = this.content_RectTransform.childCount;
        for (int i = 0; i < len; i++)
        {
            var item = matchInfoItems[i];
            item.OnKickClick = OnKickClick;
            Utils.Unity.SetActive(item.Widget.gameObject, i < maxNum);
        }
        Utils.Unity.SetActive(this.self_GameObject, true);
        var numlen = this.num1_RectTransform.childCount;
        while (numlen-- > 0)
        {
            Utils.Unity.SetActive(this.num1_RectTransform.GetChild(numlen), false);
        }
        Utils.Unity.SetActive(this.num1_RectTransform.GetChild(0), true);
        numlen = this.num2_RectTransform.childCount;
        while (numlen-- > 0)
        {
            Utils.Unity.SetActive(this.num2_RectTransform.GetChild(numlen), false);
        }
        Utils.Unity.SetActive(this.num2_RectTransform.GetChild(maxNum - 1), true);
        GameEntry.GUI.SetParam(GameEntry.GUIPath.SelectRoleUI.Path, "matchstart");
        Utils.Unity.SetActive(this.exit_Button, true);
        _hasStart = false;
        updateInfos();
        Utils.Unity.SetActive(this.exit_Button, false);
        GameEntry.Coroutine.Start(showExit(), this.Widget.GetInstanceID());


    }
    IEnumerator showExit()
    {
        yield return new WaitForSeconds(1f);
        Utils.Unity.SetActive(this.exit_Button, true);
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameEntry.GUIEvent.RemoveEventListener(GUIEvent.OnRoomPlayerInfoChange, OnReceiverMatchPlayer);
        GameEntry.GUIEvent.RemoveEventListener(GUIEvent.OnStartMateCompeComplete, OnMatchComplete);
        GameEntry.GUI.SetParam(GameEntry.GUIPath.SelectRoleUI.Path, "matchcancel");
        if (!_hasStart)
        {
            if (GameEntry.Context.MatchMode)
            {
                ExecuteAction.ByActionType(ExecuteActionType.Map_Pvp);
            }
            else
            {
                ExecuteAction.ByActionType(ExecuteActionType.Map_Pve);
            }
            GameEntry.Match.Close();
        }
    }
    public override void SetParam(string name, object value)
    {
        base.SetParam(name, value);
        if (name == "start")
        {
            _hasStart = true;
        }
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (MatchSystem.Instance.matchFinished && MatchSystem.Instance.m_nStartTime > 0)
        {
            this.matchNum_TextMeshProUGUI.text = $"{Mathf.CeilToInt(MatchSystem.Instance.m_nStartTime)}";
        }
    }

    private void OnReceiverMatchPlayer(IEventData eventData)
    {
        updateInfos();

    }
    private void updateInfos()
    {
        bool hasReady = false;
        bool hasStart = false;
        foreach (var item in matchInfoItems)
        {
            item.Clear();
        }
        var num = MatchSystem.Instance.GetMatchCurrNum();
        var maxNum = GameEntry.Table.Map.Get(MatchSystem.Instance.GetMapCfgId()).LineMaxPlayerNum;
        var list = MatchSystem.Instance.GetMatchPlayerDatas();
        joinPlayers.Clear();
        var selfId = PlayerSystem.Instance.GetUID();
        CharacterCreateData self = null;
        CharacterCreateData track0 = null;
        foreach (var item in list)
        {
            var id = item.Value.playerTrackIdx;
            if (id < 0 || id >= matchInfoItems.Count)
            {
                continue;
            }
            item.Value.UIShowIndex = item.Value.playerTrackIdx;
            if (id == 0)
            {
                track0 = item.Value;
            }
            if (item.Value.uid == selfId)
            {
                self = item.Value;
                self.UIShowIndex = 0;
            }
            joinPlayers.Add(item.Value);
        }
        if (track0 != null && self != track0)
        {
            track0.UIShowIndex = self.playerTrackIdx;
        }
        if (self != null)
        {
            self.OwnerHide = num < maxNum;
        }
        Utils.Unity.SetActive(this.exit_Button, num < maxNum);

        foreach (var item in joinPlayers)
        {
            var id = item.UIShowIndex;
            var info = matchInfoItems[id];
            info.UpdateInfo(item);
            info.SetKickShow(selfId == MatchSystem.Instance.Owner && selfId != item.uid && !MatchSystem.Instance.HasStart() && false);
        }

        this.matchNum_TextMeshProUGUI.text = string.Empty;
        var numlen = this.num1_RectTransform.childCount;
        while (numlen-- > 0)
        {
            Utils.Unity.SetActive(this.num1_RectTransform.GetChild(numlen), false);
        }
        Utils.Unity.SetActive(this.num1_RectTransform.GetChild(num - 1), true);
        Utils.Unity.SetActive(this.self_GameObject, num < maxNum);
        // Utils.Unity.SetActive(this.ready_Button, !self.isReady);

        if (GameEntry.Context.MatchMode)
        {
            // Utils.Unity.SetActive(ready_Button, selfId != MatchSystem.Instance.Owner);
            Utils.Unity.SetActive(ready_Button, hasReady);
            Utils.Unity.SetActive(start_Button, selfId == MatchSystem.Instance.Owner && hasStart);
            if (selfId == MatchSystem.Instance.Owner)
            {
                // if (list.Count >= MatchSystem.Instance.RoomMaxPlayerCount)
                // {
                //     if (GameEntry.Context.MatchMode)
                //     {
                //         GameEntry.Match.Handler.MatchReady();
                //     }
                // }
            }
            // GameEntry.Match.Handler.PlayerReady();
            if (!_matchplayerFinish)
            {
                if (num >= maxNum)
                {
                    _matchplayerFinish = true;
                    GameEntry.Match.Handler.MatchReady();
                }
            }
        }
        else
        {
            Utils.Unity.SetActive(start_Button, false);
            Utils.Unity.SetActive(ready_Button, false);
        }

    }
    private void OnMatchComplete(IEventData eventData)
    {
        updateInfos();
        Utils.Unity.SetActive(this.exit_Button, false);
    }

    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.exit_Button)
        {
            MatchSystem.Instance.EndMateCompe(!GameEntry.Context.MatchMode);
            Close();
        }
        else if (target == ready_Button)
        {
            var selfId = PlayerSystem.Instance.GetUID();
            var info = MatchSystem.Instance.GetPlayerInfo(selfId);
            if (info != null)
            {
                GameEntry.Match.Handler.PlayerReady();
                if (info.isReady)
                {

                }
            }
        }
        else if (target == start_Button)
        {
            GameEntry.Match.Handler.MatchReady();
        }
    }
    private void OnKickClick(CharacterCreateData value)
    {
        MatchSystem.Instance.KickPlayer(value.uid);
    }
}