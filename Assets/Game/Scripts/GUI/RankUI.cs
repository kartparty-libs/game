
using System.Collections;
using System.Collections.Generic;
using Framework;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;
public partial class RankUI : UIWindowBase
{
    private List<MatchCharacterData> characterInfos = new List<MatchCharacterData>();
    protected override void OnAwake()
    {
        base.OnAwake();
        this.Scroll_View_LoopListView2.InitListView(0, OnGetItemByIndex);
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        GameEntry.Http.Handler.GetChangeDataSystemData();
        characterInfos.Clear();
        var dataMap = new Dictionary<long, MatchCharacterData>();
        if (GameEntry.Context.Gameplay is GameplayRace race)
        {
            var len = race.MatchData.MatchCharacterCount;
            var error = false;
            while (len-- > 0)
            {
                var d = race.MatchData.GetCharacterData(len);
                if (d != null)
                {
                    if (!dataMap.ContainsKey(d.RoleID))
                    {
                        characterInfos.Add(d);
                        dataMap.Add(d.RoleID, d);
                    }
                    else
                    {
                        Debug.LogError("存在相同的roleId");
                        error = true;
                    }

                }
            }
            if (error)
            {
                len = race.MatchData.MatchCharacterCount;
                while (len-- > 0)
                {
                    var d = race.MatchData.GetCharacterData(len);
                    if (d != null)
                    {
                        Debug.LogError("RoleID:" + d.RoleID + " Name:" + d.Name);
                    }
                }
            }
        }

        characterInfos.Sort(SortRank);

        var list = CharacterManager.Instance.GetCharacters();
        foreach (var item in list)
        {
            var info = item.Value.GetCharacterInfo();
            if (!dataMap.ContainsKey(info.RoleId))
            {
                var d = new MatchCharacterData();
                d.Goal = false;
                d.Name = info.Name;
                d.Head = info.HeadIconId;
                characterInfos.Add(d);

            }
        }

        for (int i = 0; i < characterInfos.Count; i++)
        {
            var d = characterInfos[i];
            d.Rank = i + 1;
            var info = MatchSystem.Instance.GetPlayerInfo(d.RoleID);
            if (info != null)
            {
                d.RankTiersId = info.RankTiersId;
            }

        }
        this.Scroll_View_LoopListView2.SetListItemCount(characterInfos.Count);
        Utils.Unity.SetActive(close_Button, !GameEntry.Context.MatchMode);
        Utils.Unity.SetActive(return_Button, !GameEntry.Context.MatchMode);
        Utils.Unity.SetActive(next_Button, GameEntry.Context.MatchMode);

    }
    private int SortRank(MatchCharacterData a, MatchCharacterData b)
    {
        return a.Time - b.Time;
    }
    protected override void OnClose()
    {
        base.OnClose();
        if (Scroll_View_LoopListView2 != null)
        {
            Scroll_View_LoopListView2.SetListItemCount(0, true);
        }

    }
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= characterInfos.Count) return null;
        LoopListViewItem2 item = listView.NewListViewItem("item");
        var witem = item.GetComponent<UGUIWidget>().Widget;
        if (witem is RankItem rankItem)
        {
            var data = characterInfos[index];
            rankItem.SetData(data, index);
        }
        return item;
    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.close_Button)
        {
            GameEntry.Context.BackToSelectMap = false;
            GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameMain.ToString());
        }
        else if (target == this.return_Button)
        {
            GameEntry.Context.BackToSelectMap = true;
            GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameMain.ToString());

        }
        else if (target == this.next_Button)
        {
            Close();
            foreach (var item in characterInfos)
            {
                if (item.RoleID == PlayerSystem.Instance.GetUID())
                {
                    GameEntry.GUI.Open(GameEntry.GUIPath.MatchResultUI.Path, "rank", item.Rank);
                    return;
                }
            }
            GameEntry.GUI.Open(GameEntry.GUIPath.MatchResultUI.Path, "rank", -1);
            // GameEntry.Context.BackToSelectMap = false;
            // GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameMain.ToString());
        }
    }
}