using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using UnityEngine;

namespace Framework
{
    public interface ICharacterAnimatorController
    {
        public void OnChangeAniTrigger(int hashId);
        public void OnChangeAniFloat(int hashId, float value);
        public void OnChangeAniBool(int hashId, bool value);
    }

    /// <summary>
    /// 角色动画控制器
    /// </summary>
    public class CharacterAnimatorController : MonoBehaviour
    {
        public ICharacterAnimatorController m_Character;
        public Animator _animatorRole;
        public Animator _animatorCar;

        public void SetTrigger(int hashId)
        {
            if (_animatorRole == null || _animatorCar == null) return;

            _animatorCar.SetTrigger(hashId);
            _animatorRole.SetTrigger(hashId);

            this.m_Character.OnChangeAniTrigger(hashId);
        }

        public void SetFloat(int hashId, float value)
        {
            if (_animatorRole == null || _animatorCar == null) return;
            if (_animatorCar.GetFloat(hashId) == value) return;

            _animatorCar.SetFloat(hashId, value);
            _animatorRole.SetFloat(hashId, value);

            this.m_Character.OnChangeAniFloat(hashId, value);
        }

        public void SetBool(int hashId, bool value)
        {
            if (_animatorRole == null || _animatorCar == null) return;
            if (_animatorCar.GetBool(hashId) == value) return;

            _animatorCar.SetBool(hashId, value);
            _animatorRole.SetBool(hashId, value);

            this.m_Character.OnChangeAniBool(hashId, value);
        }

        public void SetValueByCharacterStateData(CharacterStateData i_pCharacterStateData)
        {
            foreach (var item in i_pCharacterStateData.animatorDatas)
            {
                switch (AnimatorHashIDs.AnimatorHashIDTypes[item.Key])
                {
                    case EnumDefine.AnimatorValueTypeEnum.Trigger:
                        if (item.Value == 1) this.SetTrigger(item.Key);
                        break;
                    case EnumDefine.AnimatorValueTypeEnum.Float:
                        this.SetFloat(item.Key, item.Value);
                        break;
                    case EnumDefine.AnimatorValueTypeEnum.Bool:
                        this.SetBool(item.Key, item.Value == 1);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 动画HashID
    /// </summary>
    public static class AnimatorHashIDs
    {
        /// <summary>
        /// 速度值
        /// </summary>
        public static int SpeedFloat;
        /// <summary>
        /// 转向值
        /// </summary>
        public static int SteerFloat;
        /// <summary>
        /// 触发跳跃
        /// </summary>
        public static int JumpTrigger;
        /// <summary>
        /// 触发胜利
        /// </summary>
        public static int VictoryTrigger;
        /// <summary>
        /// 是否眩晕中
        /// </summary>
        public static int DizzyBool;
        /// <summary>
        /// 是否冲刺中
        /// </summary>
        public static int SprintBool;
        /// <summary>
        /// 是否在地面
        /// </summary>
        public static int GroundBool;
        /// <summary>
        /// 轻微碰撞
        /// </summary>
        public static int HitTrigger;

        /// <summary>
        /// 动画HashID参数类型
        /// </summary>
        public static Dictionary<int, EnumDefine.AnimatorValueTypeEnum> AnimatorHashIDTypes = new Dictionary<int, EnumDefine.AnimatorValueTypeEnum>();

        public static void Initialization()
        {
            SpeedFloat = Animator.StringToHash("Speed");
            AnimatorHashIDTypes.Add(SpeedFloat, EnumDefine.AnimatorValueTypeEnum.Float);

            SteerFloat = Animator.StringToHash("Steer");
            AnimatorHashIDTypes.Add(SteerFloat, EnumDefine.AnimatorValueTypeEnum.Float);

            JumpTrigger = Animator.StringToHash("Jump");
            AnimatorHashIDTypes.Add(JumpTrigger, EnumDefine.AnimatorValueTypeEnum.Trigger);

            VictoryTrigger = Animator.StringToHash("Victory");
            AnimatorHashIDTypes.Add(VictoryTrigger, EnumDefine.AnimatorValueTypeEnum.Trigger);

            DizzyBool = Animator.StringToHash("Dizzy");
            AnimatorHashIDTypes.Add(DizzyBool, EnumDefine.AnimatorValueTypeEnum.Bool);

            SprintBool = Animator.StringToHash("Sprint");
            AnimatorHashIDTypes.Add(SprintBool, EnumDefine.AnimatorValueTypeEnum.Bool);

            GroundBool = Animator.StringToHash("Ground");
            AnimatorHashIDTypes.Add(GroundBool, EnumDefine.AnimatorValueTypeEnum.Bool);

            HitTrigger = Animator.StringToHash("Hit");
            AnimatorHashIDTypes.Add(HitTrigger, EnumDefine.AnimatorValueTypeEnum.Trigger);
        }
        
        public static EnumDefine.AnimatorValueTypeEnum GetAnimatorHashIDType(int i_nAnimatorHashID)
        {
            return AnimatorHashIDTypes[i_nAnimatorHashID];
        }
    }
}
