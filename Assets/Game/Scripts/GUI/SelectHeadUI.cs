
using Framework;
using SuperScrollView;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public partial class SelectHeadUI : UIWindowBase
{
    private int selectType;
    private List<int> dataList;
    private SelectHeadItem selectItem;
    private int selectId;
    protected override void OnAwake()
    {
        base.OnAwake();
        selectId = -1;
        dataList = new List<int>();
        this.list_LoopListView2.InitListView(0, OnGetItemByIndex);
        this.dropdown_TMP_Dropdown.onValueChanged.AddListener((index) =>
        {
            if (selectType != index)
            {
                selectType = index;
                updateListData();
                list_LoopListView2.RefreshAllShownItem();
            }

        });

    }
    protected override void OnOpen()
    {
        base.OnOpen();
        selectId = PlayerSystem.Instance.GetHead();
        updateListData();
        this.id_TextMeshProUGUI.text = (GameDefine.IsPreRelease() ? "#" : "") + GameEntry.Context.Account;

    }
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        //index %= dataList.Count;
        //if(index<0)
        //{
        //    index += dataList.Count;
        //}
        if (index < 0 || index >= dataList.Count + 2)
        {
            return null;
        }
        if (index < 1 || index >= dataList.Count + 1)
        {
            return listView.NewListViewItem("empty");
        }
        LoopListViewItem2 item = listView.NewListViewItem("SelectHeadItem");
        var w = item.GetComponent<UGUIWidget>().Widget;
        if (w != null)
        {
            if (w is SelectHeadItem info)
            {
                var id = dataList[index - 1];
                info.UpdateData(id, onSelectItem);
                info.SetSelect(id == selectId);
                if (id == selectId)
                {
                    selectItem = info;
                }
            }
        }
        return item;
    }
    protected override void OnButtonClick(Button target)
    {
        base.OnButtonClick(target);
        if (target == this.ok_Button)
        {
            if (!GameEntry.Context.OfflineMode)
            {
                PlayerSystem.Instance.SetHead(selectId);
            }
            else
            {
                GameEntry.OfflineManager.SetHeadId(selectId);
            }
            GameEntry.Context.NeedCreateHead = false;
            Close();
        }
        else if (target == this.back_Button)
        {
            Close();
        }
        else if (target == this.prerelase_Button)
        {
            if (GameDefine.IsPreRelease())
            {
                PlayerPrefs.SetString(GameDefine.PreRelease, "0");
            }
            else
            {
                PlayerPrefs.SetString(GameDefine.PreRelease, "1");
            }
            this.id_TextMeshProUGUI.text = (GameDefine.IsPreRelease() ? "#" : "") + GameEntry.Context.Account;
            JavascriptBridge.Call(JavascriptBridge.UnityPreRelease, GameDefine.IsPreRelease() ? "1" : "0");
        }
    }
    private void updateListData()
    {
        dataList.Clear();
        var len = GameEntry.Table.Head.ItemCount;
        for (int i = 0; i < len; i++)
        {
            var data = GameEntry.Table.Head.GetItem(i);
            if (selectType == 0)
            {
                if (data.Sex)
                {
                    dataList.Add(data.Id);
                }
            }
            else
            {
                if (!data.Sex)
                {
                    dataList.Add(data.Id);
                }
            }
        }

        list_LoopListView2.SetListItemCount(dataList.Count + 2);

    }
    private void onSelectItem(SelectHeadItem item)
    {
        selectId = item.Data;
        if (selectItem != null)
        {
            selectItem.SetSelect(false);
        }
        selectItem = item;
        if (selectItem != null)
        {
            selectItem.SetSelect(true);
            selectItem.Widget.transform.SetAsFirstSibling();
        }


    }

}