
using Framework;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;
public partial class ScoreRankUI : UIWindowBase
{
    private ScoreRankItem myRankItem;
    private int myRankValue;
    private RankConfigData Info;
    private float _secTime;
    private float _endTime;
    private bool _hasTime;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.list_LoopListView2.InitListView(0, OnGetItemByIndex);
        if (myrank_GameObject.GetComponent<UGUIWidget>().Widget is ScoreRankItem item)
        {
            myRankItem = item;
            var g = myRankItem.Widget.gameObject.AddComponent<CanvasGroup>();
            g.interactable = false;
            g.blocksRaycasts = false;
        }
        myRankValue = -1;
        _endTime = Time.time;
        this.time_TextMeshProUGUI.gameObject.SetActive(false);
        list_LoopListView2.mOnEndDragAction = OnEndDrag;
    }
    void OnEndDrag()
    {
        if (Info == null)
        {
            return;
        }
        if (list_LoopListView2.ShownItemCount == 0)
        {
            return;
        }

        LoopListViewItem2 item = list_LoopListView2.GetShownItemByItemIndex(Info.All.Count);
        if (item == null)
        {
            return;
        }
        RankSystem.Instance.Request(Proto.RankTypeEnum.MiningTokenScore);
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        myRankValue = -1;
        myrank_GameObject.SetActive(false);
        RankSystem.Instance.CheckRefreshTime(Proto.RankTypeEnum.MiningTokenScore);
        Info = RankSystem.Instance.GetRankInfo(Proto.RankTypeEnum.MiningTokenScore);
        if (Info.All.Count < RankSystem.pageCount)
        {
            RankSystem.Instance.Request(Proto.RankTypeEnum.MiningTokenScore);
        }
        EventManager.Instance.Regist(EventDefine.Global.OnRankChange, OnRankChange);
        this.time_TextMeshProUGUI.text = "";
        _hasTime = false;
        list_LoopListView2.MovePanelToItemIndex(0, 0);
        OnRankChange();
    }

    private void OnRankChange()
    {
        Info = RankSystem.Instance.GetRankInfo(Proto.RankTypeEnum.MiningTokenScore);
        if (Info == null)
        {
            return;
        }
        var selfRank = Info.SelfData;
        if (selfRank != null)
        {
            myRankValue = selfRank.Rank;
            Utils.Unity.SetActive(myrank_GameObject, true);
        }

        this.list_LoopListView2.SetListItemCount(Info.All.Count + 1, false);
        list_LoopListView2.RefreshAllShownItem();
        myRankItem.SetSelfData(selfRank);
        Utils.Unity.SetActive(tip_GameObject, Info.All.Count < 1);
        _endTime = Time.time + RankSystem.Instance.RefreshRemainSeconds;
        _hasTime = Info.NextUpdateTime > 0;
        // Utils.Unity.SetActive(myrank_GameObject, true);
        updateTime();

    }

    protected override void OnClose()
    {
        base.OnClose();
        EventManager.Instance.Remove(EventDefine.Global.OnRankChange, OnRankChange);
    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.close_Button || target == this.back_Button)
        {
            this.Close();
        }
        else if (target == this.info_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.ScoreRankRewardUI.Path);
        }
        else if (target == this.rule_Button)
        {
            GameEntry.GUI.Open(GameEntry.GUIPath.ScoreRankRuleUI.Path);
        }
    }
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || Info == null) return null;
        if (index == Info.All.Count)
        {
            if (Info.FullData)
            {
                return null;
            }
            return listView.NewListViewItem("LoadMore");
        }
        if (index > Info.All.Count)
        {
            return null;
        }
        LoopListViewItem2 item = listView.NewListViewItem("ScoreRankItem");
        var witem = item.GetComponent<UGUIWidget>().Widget;
        if (witem is ScoreRankItem rankItem)
        {
            rankItem.SetData(Info.All[index], myRankValue);
        }
        return item;
    }
    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (Info == null) return;
        bool showself = true;
        if (Info != null && Info.All.Count > 0)
        {
            list_LoopListView2.UpdateAllShownItemSnapData();
            int count = list_LoopListView2.ShownItemCount;

            if (myRankValue < 1)
            {
                showself = false;
            }
            else
            {
                for (int i = 0; i < count; ++i)
                {
                    LoopListViewItem2 item = list_LoopListView2.GetShownItemByIndex(i);
                    if (item.ItemIndex == Info.All.Count)
                    {
                        continue;
                    }
                    var witem = item.GetComponent<UGUIWidget>().Widget;
                    if (witem is ScoreRankItem rankItem)
                    {
                        if (rankItem.Rank == myRankValue)
                        {
                            var p = item.ParentListView.transform.InverseTransformPoint(item.transform.position);
                            if (p.y < 500 && p.y > -350f)
                            {
                                showself = false;
                            }
                            break;
                        }
                    }
                }
            }

        }
        else
        {
            if (myRankValue < 1)
            {
                showself = false;
            }
        }
        Utils.Unity.SetActive(myRankItem.Widget.gameObject, showself);

        _secTime += deltaTime;
        if (_secTime > 1f)
        {
            _secTime -= 1f;
            updateTime();
        }
    }
    private void updateTime()
    {
        var endTime = TimeUtils.GetDateTimeByMilliseconds(Info.NextUpdateTime);
        var diff = TimeUtils.GetDiff(GameEntry.OfflineManager.GetServerDateTime(), endTime);
        if (diff.TotalSeconds > 0)
        {
            this.time_TextMeshProUGUI.text = GameEntry.Table.Lang.Get(1134, TimeUtils.GetRemainder(diff));
            Utils.Unity.SetActive(this.time_TextMeshProUGUI, true);
        }
        else
        {
            this.time_TextMeshProUGUI.text = "";
            Utils.Unity.SetActive(this.time_TextMeshProUGUI, false);
        }
    }
}