using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 角色网络接收器接口
    /// </summary>
    public interface ICharacterNetWorkReceiver
    {
        public CharacterNetWorkReceiverController GetCharacterNetWorkReceiverController();
    }

    /// <summary>
    /// 角色网络接收器
    /// </summary>
    public class CharacterNetWorkReceiverController : MonoBehaviour
    {
        public NetWorkCharacter m_Character;
        private CharacterStateData m_CharacterStateData = new CharacterStateData();
        private Vector3 m_SyncPosition;
        private Quaternion m_SyncRotation;
        private float m_VelocityMagnitude;

        void Update()
        {
            if (this.m_CharacterStateData.roleId != this.m_Character.GetServerInstId())
            {
                return;
            }
            if (m_Character.fakeMove)
            {
                return;
            }
            if (this.m_SyncPosition != this.m_Character.GetPosition())
            {
                if ((this.m_Character.GetPosition() - this.m_SyncPosition).sqrMagnitude < 0.01f)
                {
                    this.m_Character.SetPosition(this.m_SyncPosition);
                }
                else
                {
                    float magnitude = this.m_CharacterStateData.velocity.magnitude * 0.9f;
                    if (magnitude > 0)
                    {
                        this.m_VelocityMagnitude = Mathf.Max(magnitude, 3f);
                        this.m_Character.SetPosition(Vector3.MoveTowards(this.m_Character.GetPosition(), this.m_SyncPosition, this.m_VelocityMagnitude * Time.deltaTime));
                    }
                    else
                    {
                        this.m_Character.SetPosition(Vector3.Lerp(this.m_Character.GetPosition(), this.m_SyncPosition, Time.deltaTime * 10));
                    }
                }
            }

            if (this.m_SyncRotation != this.m_Character.GetRotation())
            {
                if ((this.m_Character.GetRotation().eulerAngles - this.m_SyncRotation.eulerAngles).sqrMagnitude < 0.01f)
                {
                    this.m_Character.SetRotation(this.m_SyncRotation);
                }
                else
                {
                    this.m_Character.SetRotation(Quaternion.Lerp(this.m_Character.GetRotation(), this.m_SyncRotation, Time.deltaTime * 10));
                }
            }

            // this.m_Character.CharacterMovementController.m_ForceMovementVelocity = this.m_CharacterStateData.velocity;
        }
        /* 
        /// <summary>
        /// 接收角色状态同步数据
        /// </summary>
        /// <param name="i_tCharacterStateData"></param>
        public void ReceiverCharacterStateData(JArray i_tCharacterStateData)
        {
            // 角色Id
            this.m_CharacterStateData.roleId = i_tCharacterStateData[EnumDefine.CharacterStateDataIdx.RoleId].ToString();

            // 空间信息
            JArray tTransformData = i_tCharacterStateData[EnumDefine.CharacterStateDataIdx.Transform] as JArray;
            float nTransformPosX = this.ConvertSyncServerValue((int)tTransformData[EnumDefine.CharacterStateDataIdx.TransformPosX]);
            float nTransformPosY = this.ConvertSyncServerValue((int)tTransformData[EnumDefine.CharacterStateDataIdx.TransformPosY]);
            float nTransformPosZ = this.ConvertSyncServerValue((int)tTransformData[EnumDefine.CharacterStateDataIdx.TransformPosZ]);
            float nTransformRotX = this.ConvertSyncServerValue((int)tTransformData[EnumDefine.CharacterStateDataIdx.TransformRotX]);
            float nTransformRotY = this.ConvertSyncServerValue((int)tTransformData[EnumDefine.CharacterStateDataIdx.TransformRotY]);
            float nTransformRotZ = this.ConvertSyncServerValue((int)tTransformData[EnumDefine.CharacterStateDataIdx.TransformRotZ]);
            float nTransformRotW = this.ConvertSyncServerValue((int)tTransformData[EnumDefine.CharacterStateDataIdx.TransformRotW]);
            this.m_CharacterStateData.position.Set(nTransformPosX, nTransformPosY, nTransformPosZ);
            this.m_CharacterStateData.rotation.Set(nTransformRotX, nTransformRotY, nTransformRotZ, nTransformRotW);

            // 速度信息
            JArray tVelocityData = i_tCharacterStateData[EnumDefine.CharacterStateDataIdx.Velocity] as JArray;
            float nVelocityX = this.ConvertSyncServerValue((int)tVelocityData[EnumDefine.CharacterStateDataIdx.VelocityX]);
            float nVelocityY = this.ConvertSyncServerValue((int)tVelocityData[EnumDefine.CharacterStateDataIdx.VelocityY]);
            float nVelocityZ = this.ConvertSyncServerValue((int)tVelocityData[EnumDefine.CharacterStateDataIdx.VelocityZ]);
            this.m_CharacterStateData.velocity.Set(nVelocityX, nVelocityY, nVelocityZ);

            // 动画信息
            JArray tAnimatorData = i_tCharacterStateData[EnumDefine.CharacterStateDataIdx.Animator] as JArray;
            for (int i = 0; i < tAnimatorData.Count; i++)
            {
                this.m_CharacterStateData.animatorDatas[EnumDefine.AnimatorIdxMappingHashId[i]] = this.ConvertSyncServerValue((int)tAnimatorData[i]);
            }

            this.SyncCharacterState();
        } */
        public void SetRoleId(long value)
        {
            m_CharacterStateData.roleId = value;
        }
        public void SetPosition(int x, int y, int z)
        {
            this.m_CharacterStateData.position.Set(ConvertSyncServerValue(x), ConvertSyncServerValue(y), ConvertSyncServerValue(z));
            this.m_SyncPosition = this.m_CharacterStateData.position;

        }
        public void SetRotation(int x, int y, int z, int w)
        {
            this.m_CharacterStateData.rotation.Set(ConvertSyncServerValue(x), ConvertSyncServerValue(y), ConvertSyncServerValue(z), ConvertSyncServerValue(w));
            this.m_SyncRotation = this.m_CharacterStateData.rotation;
        }
        public void SetVelocity(int x, int y, int z)
        {
            this.m_CharacterStateData.velocity.Set(ConvertSyncServerValue(x), ConvertSyncServerValue(y), ConvertSyncServerValue(z));
        }
        public void SetAnimation(int hashId, float value)
        {
            var type = AnimatorHashIDs.AnimatorHashIDTypes[hashId];
            if (type == EnumDefine.AnimatorValueTypeEnum.Trigger)
            {
                if (value > 0.1f)
                {
                    this.m_Character.CharacterAnimatorController.SetTrigger(hashId);
                }
            }
            else if (type == EnumDefine.AnimatorValueTypeEnum.Float)
            {
                this.m_Character.CharacterAnimatorController.SetFloat(hashId, value);
            }
            else if (type == EnumDefine.AnimatorValueTypeEnum.Bool)
            {
                this.m_Character.CharacterAnimatorController.SetBool(hashId, value > 0.1f);
            }
        }

        /// <summary>
        /// 同步角色状态
        /// </summary>
        public void SyncCharacterState()
        {
            if (this.m_CharacterStateData.roleId != this.m_Character.GetServerInstId())
            {
                return;
            }
            this.m_SyncPosition = this.m_CharacterStateData.position;
            this.m_SyncRotation = this.m_CharacterStateData.rotation;
            this.m_Character.CharacterAnimatorController.SetValueByCharacterStateData(this.m_CharacterStateData);
        }

        /// <summary>
        /// 转换同步数值
        /// </summary>
        /// <returns></returns>
        private float ConvertSyncServerValue(int i_sValue)
        {
            return i_sValue / GameDefine.SyncValueMultiple;
        }
    }
}