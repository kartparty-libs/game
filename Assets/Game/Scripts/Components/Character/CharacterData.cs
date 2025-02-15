

using System.Collections.Generic;
using Framework;
using UnityEngine;

/// <summary>
/// 角色创建数据
/// </summary>
public class CharacterCreateData
{
    public long uid;
    public string playerName;
    public int playerTrackIdx;
    public int characterRoleCfgId;
    public int characterCarCfgId;
    public int head;
    public bool IsRoomOwner;
    public bool isReady;
    public int RankTiersId;
    
    public EnumDefine.CharacterTypeEnum characterTypeEnum;
    public int UIShowIndex;
    public bool OwnerHide;
}

/// <summary>
/// 角色同步数据
/// </summary>
public class CharacterStateData
{
    public CharacterStateData()
    {
        foreach (var item in EnumDefine.AnimatorHashIdMappingIdx)
        {
            animatorDatas.Add(item.Key, 0);
        }
    }
    public long roleId ;
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 velocity = Vector3.zero;
    public Dictionary<int, float> animatorDatas = new Dictionary<int, float>();
}