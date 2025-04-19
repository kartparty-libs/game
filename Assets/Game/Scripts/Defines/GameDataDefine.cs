using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCharacterData
{
    public MatchCharacterData()
    {

    }
    public int Time;
    public long RoleID;
    public string Name;
    public int RankTiersId=1;
    /// 
    public int Head;
    public int Rank;
    public bool Goal;
}

public class TrophyItemData
{
    public TreasureChestData Data;
    public TreasureChest_Data Tpl;
    public bool Selected;
    public bool Dot;
    public bool MargeSelect;
    public void Reset()
    {
        Selected = false;
        MargeSelect = false;
    }

}