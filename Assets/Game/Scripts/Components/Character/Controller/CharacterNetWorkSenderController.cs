using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 角色网络发送器接口
    /// </summary>
    public interface ICharacterNetWorkSender
    {
    }

    /// <summary>
    /// 角色网络发送器
    /// </summary>
    public class CharacterNetWorkSenderController : MonoBehaviour
    {
        public ICharacter m_Character;

        private CharacterStateData m_CharacterStateData = new CharacterStateData();
        private bool m_IsChangeCharacterStateData = false;

        // private JArray m_CharacterStateDataJArray = new JArray();
        public float m_Time = 0.02f;
        private float m_CurrTime;

        // private List<JArray> m_DelaySendDatas = new List<JArray>();
        private List<float> m_DelaySendDataTimes = new List<float>();

        // -----------------------------------------------------------------------------------------------------------------------------

        public void OnChangeAniTrigger(int hashId)
        {
            this.m_CharacterStateData.animatorDatas[hashId] = 1;
            this.m_IsChangeCharacterStateData = true;
        }

        public void OnChangeAniFloat(int hashId, float value)
        {
            this.m_CharacterStateData.animatorDatas[hashId] = value;
            this.m_IsChangeCharacterStateData = true;
        }

        public void OnChangeAniBool(int hashId, bool value)
        {
            this.m_CharacterStateData.animatorDatas[hashId] = value ? 1 : 0;
            this.m_IsChangeCharacterStateData = true;
        }

        // -----------------------------------------------------------------------------------------------------------------------------

        void Update()
        {

            this.UpdateCharacterStateTransformData();
            if (m_IsChangeCharacterStateData)
            {
                if (m_CurrTime <= 0f)
                {
                    m_CurrTime = m_Time;
                }
            }
            if (m_CurrTime > 0f)
            {
                m_CurrTime -= Time.deltaTime;
                if (m_CurrTime <= 0)
                {
                    this.SendCharacterStateData();
                }
            }

            // 测试延迟
            // if(this.m_DelaySendDataTimes.Count > 0)
            // {
            //     for (int i = this.m_DelaySendDataTimes.Count - 1; i >= 0; i--)
            //     {
            //         this.m_DelaySendDataTimes[i] -= Time.deltaTime;
            //         if(this.m_DelaySendDataTimes[i] <= 0)
            //         {
            //             CSharpNetworkHandler.Instance.Send(CtoS.K_PlayerStateInfoReq, this.m_DelaySendDatas[i]);
            //             this.m_DelaySendDatas.RemoveAt(i);
            //             this.m_DelaySendDataTimes.RemoveAt(i);
            //         }
            //     }
            // }
        }

        /// <summary>
        /// 检测角色空间信息数据变化
        /// </summary>
        /// <returns></returns>
        private void UpdateCharacterStateTransformData()
        {
            if (this.m_CharacterStateData.position != this.m_Character.GetPosition())
            {
                this.m_CharacterStateData.position = this.m_Character.GetPosition();
                this.m_IsChangeCharacterStateData = true;
            }

            if (this.m_CharacterStateData.rotation != this.m_Character.GetRotation())
            {
                this.m_CharacterStateData.rotation = this.m_Character.GetRotation();
                this.m_IsChangeCharacterStateData = true;
            }

            if (this.m_CharacterStateData.velocity != this.m_Character.GetVelocity())
            {
                this.m_CharacterStateData.velocity = this.m_Character.GetVelocity();
                this.m_IsChangeCharacterStateData = true;
            }
        }

        /// <summary>
        /// 发送角色状态同步数据
        /// </summary>\
        private List<float> animationDatas = new List<float>();
        private void SendCharacterStateData()
        {
            if (!GameEntry.Context.IsHasNetWorkCharacter)
            {
                return;
            }

            // if (GameEntry.Context.ServerCompetitionMapState == EnumDefine.CompetitionMapState.EndGame)
            // {
            //     return;
            // }

            if (!this.m_IsChangeCharacterStateData)
            {
                return;
            }

            this.m_IsChangeCharacterStateData = false;
            animationDatas.Clear();
            for (int i = 0; i < EnumDefine.AnimatorIdxMappingHashId.Length; i++)
            {
                int nHashID = EnumDefine.AnimatorIdxMappingHashId[i];

                float anivalue = this.m_CharacterStateData.animatorDatas[nHashID];
                animationDatas.Add(anivalue);
                if (AnimatorHashIDs.GetAnimatorHashIDType(nHashID) == EnumDefine.AnimatorValueTypeEnum.Trigger)
                {
                    this.m_CharacterStateData.animatorDatas[nHashID] = 0;
                }
            }

            CharacterManager.Instance.PlayerStateSync(m_Character.GetServerInstId(), m_CharacterStateData.position, m_CharacterStateData.rotation, m_CharacterStateData.velocity, animationDatas);
            /* 
            // JArray CharacterStateDataJArray = new JArray();
            // 角色Id
            this.m_CharacterStateData.roleId = this.m_Character.GetServerInstId();
            this.m_CharacterStateDataJArray.Add(this.m_CharacterStateData.roleId);

            // 空间信息
            JArray tTransformJArray = new JArray();
            tTransformJArray.Add(this.ConvertSyncClientValue(m_CharacterStateData.position.x));
            tTransformJArray.Add(this.ConvertSyncClientValue(m_CharacterStateData.position.y));
            tTransformJArray.Add(this.ConvertSyncClientValue(m_CharacterStateData.position.z));
            tTransformJArray.Add(this.ConvertSyncClientValue(m_CharacterStateData.rotation.x));
            tTransformJArray.Add(this.ConvertSyncClientValue(m_CharacterStateData.rotation.y));
            tTransformJArray.Add(this.ConvertSyncClientValue(m_CharacterStateData.rotation.z));
            tTransformJArray.Add(this.ConvertSyncClientValue(m_CharacterStateData.rotation.w));
            this.m_CharacterStateDataJArray.Add(tTransformJArray);

            // 速度信息
            JArray tVelocityJArray = new JArray();
            tVelocityJArray.Add(this.ConvertSyncClientValue(m_CharacterStateData.velocity.x));
            tVelocityJArray.Add(this.ConvertSyncClientValue(m_CharacterStateData.velocity.y));
            tVelocityJArray.Add(this.ConvertSyncClientValue(m_CharacterStateData.velocity.z));
            this.m_CharacterStateDataJArray.Add(tVelocityJArray);

            // 动画信息
            JArray tAnimatorJArray = new JArray();
            for (int i = 0; i < EnumDefine.AnimatorIdxMappingHashId.Length; i++)
            {
                int nHashID = EnumDefine.AnimatorIdxMappingHashId[i];
                tAnimatorJArray.Add(this.ConvertSyncClientValue(this.m_CharacterStateData.animatorDatas[nHashID]));
                if (AnimatorHashIDs.GetAnimatorHashIDType(nHashID) == EnumDefine.AnimatorValueTypeEnum.Trigger)
                {
                    this.m_CharacterStateData.animatorDatas[nHashID] = 0;
                }
            }

            this.m_CharacterStateDataJArray.Add(tAnimatorJArray);

            GameEntry.Net.Send(CtoS.K_PlayerStateInfoReq, this.m_CharacterStateDataJArray);
            // this.m_DelaySendDatas.Add(CharacterStateDataJArray);
            // this.m_DelaySendDataTimes.Add(Random.Range(0.05f, 0.2f));
            this.m_CharacterStateDataJArray.Clear();
             */
        }

        /// <summary>
        /// 转换同步数值
        /// </summary>
        /// <returns></returns>
        private int ConvertSyncClientValue(float i_nValue)
        {
            return (int)(i_nValue * GameDefine.SyncValueMultiple);
        }
    }
}