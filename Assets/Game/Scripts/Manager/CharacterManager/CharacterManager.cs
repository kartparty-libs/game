using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Proto;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 角色管理器
    /// </summary>
    public class CharacterManager : BaseManager<CharacterManager>
    {
        // -----------------------------------------------------------------------------------------------------------------------
        // 角色
        public bool Enabled;
        private Dictionary<long, ICharacter> m_pCharacters = new Dictionary<long, ICharacter>();
        private Dictionary<long, ResResult> m_pCharacterResResults = new Dictionary<long, ResResult>();

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="i_sRoleId"></param>
        /// <returns></returns>
        public ICharacter GetCharacter(long i_sRoleId)
        {
            return this.m_pCharacters[i_sRoleId];
        }

        /// <summary>
        /// 是否有网络角色
        /// </summary>
        /// <returns></returns>
        public bool IsHasNetWorkCharacter()
        {
            foreach (var item in this.m_pCharacters)
            {
                if (item.Value is ICharacterNetWorkReceiver)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<long, ICharacter> GetCharacters()
        {
            return this.m_pCharacters;
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="i_pCharacterData"></param>
        public void CreateCharacter(CharacterCreateData i_pCharacterData, Action i_pCallBack = null)
        {
            var uid=i_pCharacterData.uid;
            if (this.m_pCharacters.ContainsKey(uid) || this.m_pCharacterResResults.ContainsKey(uid))
            {
                return;
            }
            string sLoadPath = $"Assets/Res/Prefabs/Player/{i_pCharacterData.characterTypeEnum}.prefab";
            ResResult pResult = GameEntry.AssetsLoader.LoadAsset(sLoadPath, (result) =>
            {
                if (this.m_pCharacterResResults.ContainsKey(i_pCharacterData.uid))
                {
                    if (result.Asset is GameObject obj)
                    {
                        ICharacter pCharacter = obj.GetComponent(Type.GetType("Framework." + i_pCharacterData.characterTypeEnum.ToString())) as ICharacter;
                        pCharacter.Init(i_pCharacterData);

                        this.m_pCharacters.Add(i_pCharacterData.uid, pCharacter);
                    }
                    this.m_pCharacterResResults.Remove(i_pCharacterData.uid);
                    if (i_pCallBack != null)
                    {
                        i_pCallBack();
                    }
                }
                else
                {
                    if (result.Asset is GameObject obj)
                    {
                        GameObject.Destroy(obj);
                    }
                }
            });
            pResult.Result.IsGameObject = true;
            this.m_pCharacterResResults.Add(i_pCharacterData.uid, pResult);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="i_sRoleId"></param>
        public void RemoveCharacter(long i_sRoleId)
        {
            ICharacter pCharacter;
            if (this.m_pCharacters.TryGetValue(i_sRoleId, out pCharacter))
            {
                pCharacter.Destroy();
                this.m_pCharacters.Remove(i_sRoleId);
            }
            ResResult pResResult;
            if (this.m_pCharacterResResults.TryGetValue(i_sRoleId, out pResResult))
            {
                this.m_pCharacterResResults.Remove(i_sRoleId);
            }
        }

        /// <summary>
        /// 清理角色
        /// </summary>
        public void ClearCharacter()
        {
            foreach (var item in this.m_pCharacters)
            {
                item.Value.Destroy();
            }
            this.m_pCharacters.Clear();
            this.m_pCharacterResResults.Clear();
        }

        /// <summary>
        /// 同步角色状态数据
        /// </summary>
        /// <param name="i_tCharacterStateData"></param>
        // public void SyncCharacterStateData(JArray i_tCharacterStateData)
        // {
        //     string sRoleId = i_tCharacterStateData[EnumDefine.CharacterStateDataIdx.RoleId].ToString();

        //     ICharacter pCharacter;
        //     if (this.m_pCharacters.TryGetValue(sRoleId, out pCharacter))
        //     {
        //         if (pCharacter is ICharacterNetWorkReceiver characterNetWorkReceiver)
        //         {
        //             characterNetWorkReceiver.GetCharacterNetWorkReceiverController().ReceiverCharacterStateData(i_tCharacterStateData);
        //         }
        //     }
        // }

        // -----------------------------------------------------------------------------------------------------------------------
        // 匹配角色

        private Dictionary<string, MatchCharacter> m_pMatchCharacters = new Dictionary<string, MatchCharacter>();
        private Dictionary<string, ResResult> m_pMatchCharacterResResults = new Dictionary<string, ResResult>();


        /// <summary>
        /// 删除匹配角色
        /// </summary>
        /// <param name="i_sRoleId"></param>
        public void RemoveMatchCharacter(string i_sRoleId)
        {
            MatchCharacter pMatchCharacter;
            if (this.m_pMatchCharacters.TryGetValue(i_sRoleId, out pMatchCharacter))
            {
                GameObject.Destroy(pMatchCharacter.gameObject);
                this.m_pMatchCharacters.Remove(i_sRoleId);
            }
            ResResult pResResult;
            if (this.m_pMatchCharacterResResults.TryGetValue(i_sRoleId, out pResResult))
            {
                this.m_pMatchCharacterResResults.Remove(i_sRoleId);
            }
        }

        /// <summary>
        /// 清理匹配角色
        /// </summary>
        public void ClearMatchCharacter()
        {
            foreach (var item in this.m_pMatchCharacters)
            {
                GameObject.Destroy(item.Value.gameObject);
            }
            this.m_pMatchCharacters.Clear();
            this.m_pMatchCharacterResResults.Clear();
        }

        public void PlayerStateSync(long roleId,Vector3 position, Quaternion rotaion, Vector3 velocity, List<float> animation)
        {
            GameEntry.Match.Handler.PlayerStateSync(roleId,position, rotaion, velocity, animation);
        }
        public void OnPlayerStateSync(ResMsgBodyPlayerStateSync res)
        {
            long sRoleId = res.BattlePlayerStateData.RoleId;

            ICharacter pCharacter;
            if (this.m_pCharacters.TryGetValue(sRoleId, out pCharacter))
            {
                if (pCharacter is ICharacterNetWorkReceiver characterNetWorkReceiver)
                {
                    var Controller = characterNetWorkReceiver.GetCharacterNetWorkReceiverController();
                    if (Controller != null)
                    {
                        Controller.SetRoleId(sRoleId);
                        var ipos = res.BattlePlayerStateData.Position;
                        Controller.SetPosition(ipos.X, ipos.Y, ipos.Z);
                        var irot = res.BattlePlayerStateData.Rotation;
                        Controller.SetRotation(irot.X, irot.Y, irot.Z, irot.W);
                        var ivel = res.BattlePlayerStateData.Velocity;
                        Controller.SetVelocity(ivel.X, ivel.Y, ivel.Z);
                        int k = 0;
                        foreach (var item in res.BattlePlayerStateData.Animator)
                        {
                            int nHashID = EnumDefine.AnimatorIdxMappingHashId[k++];
                            Controller.SetAnimation(nHashID, item * 0.001f);
                        }
                    }
                    // Controller.SyncCharacterState();
                    // characterNetWorkReceiver.GetCharacterNetWorkReceiverController().ReceiverCharacterStateData(i_tCharacterStateData);
                }
            }
        }
    }
}