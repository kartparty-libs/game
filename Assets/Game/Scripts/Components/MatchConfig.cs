using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class MatchConfig : MonoBehaviour
{
    public Transform[] OriginPoints;
    private bool IsLoad;
    private void Start()
    {
        if (GameEntry.Context != null)
        {
            GameEntry.Context.MatchConfig = this;
        }

        this.OriginPoints = new Transform[8];
        Transform pOriginPoints = transform.Find("originPoints");
        if (pOriginPoints != null)
        {
            for (int i = 0; i < pOriginPoints.childCount; i++)
            {
                this.OriginPoints[i] = pOriginPoints.GetChild(i);
                this.OriginPoints[i].GetChild(0)?.gameObject.SetActive(false);
            }
        }

        this.IsLoad = false;
    }

    /// <summary>
    /// 获取匹配站点
    /// </summary>
    /// <param name="i_nTrackIdx"></param>
    /// <returns></returns>
    public Transform GetOriginPoint(int i_nTrackIdx)
    {
        if (this.OriginPoints.Length <= i_nTrackIdx)
        {
            return this.OriginPoints[0];
        }

        return this.OriginPoints[i_nTrackIdx];
    }

    // public void OnLoad()
    // {
    //     this.IsLoad = true;

    //     Dictionary<string, MatchPlayerData> tMatchPlayerDatas = MatchSystem.Instance.GetMatchPlayerDatas();
    //     foreach (var item in tMatchPlayerDatas)
    //     {
    //         this.LoadMarchPlayer(item.Value);
    //     }
    // }

    // public void LoadMarchPlayer(MatchPlayerData i_pCharacterData)
    // {
    //     GameEntry.AssetsLoader.LoadAsset("Assets/Res/Prefabs/Player/MatchPlayer.prefab", (result) =>
    //     {
    //         result.SetSceneActive();
    //         result.Result.IsGameObject = true;
    //         if (result.Asset is GameObject obj)
    //         {
    //             MatchCharacter pMatchCharacter = obj.GetComponent<MatchCharacter>();
    //             pMatchCharacter.SetPosition(GameEntry.Context.MatchConfig.GetOriginPoint(i_pCharacterData.playerTrackIdx).position);
    //             pMatchCharacter.SetRotation(GameEntry.Context.MatchConfig.GetOriginPoint(i_pCharacterData.playerTrackIdx).rotation);
    //             pMatchCharacter.LoadSkin(i_pCharacterData.characterRoleCfgId, i_pCharacterData.characterCarCfgId);
    //         }
    //     });
    // }
}
