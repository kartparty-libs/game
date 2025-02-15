using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 角色移动控制器接口
    /// </summary>
    public interface ICharacterMovement
    {
        public CharacterMovementController GetCharacterMovementController();
    }

    /// <summary>
    /// 角色移动控制器
    /// </summary>
    public class CharacterMovementController : MonoBehaviour
    {
        public float m_MovementSpeed = 6f;
        public float m_MovementAcceleratedSpeed = 1f;
        public float m_RotateSpeed = 180f;
        public float m_JumpSpeed = 5f;
        public bool useGravity = true;

        public BaseCharacter m_Character;

        private Vector3 m_Velocity = Vector3.zero;
        public float m_VelocityMagnitude { get; set; }
        private Vector3 m_OffsetVelocity = Vector3.zero;
        public float m_OffsetVelocityMagnitude { get; set; }
        public Vector3 m_ForceMovementVelocity = Vector3.zero;
        private Vector3 m_MovementVelocity = Vector3.zero;
        public Vector3 m_LastMovementVelocity = Vector3.zero;
        private Vector3 m_MovementDirection = Vector3.zero;
        private float m_JumpVelocity;
        private bool m_IsJump = false;

        public Vector3 m_RotateCrossDir;
        public float m_Speed { get; private set; }
        private float m_InertiaSpeed;
        private float m_SpeedUpLerpTime;
        private float m_SpeedDownLerpTime;

        void Update()
        {
            if (this.m_Character.m_IsDizzy)
            {
                this.SetVelocity(Vector3.zero);
            }

            if (this.m_Character.m_IsGround)
            {
                this.m_MovementDirection = Vector3.ProjectOnPlane(this.transform.forward, this.m_Character.m_GroundNormal).normalized;

            }
            else
            {
                if (this.m_MovementDirection != this.transform.forward)
                {
                    this.m_MovementDirection = Vector3.Lerp(this.m_MovementDirection, this.transform.forward, Time.deltaTime * 10f).normalized;
                    float angle = Vector3.Angle(this.m_MovementDirection, this.transform.forward);
                    if (angle < 0.5f)
                    {
                        this.m_MovementDirection = this.transform.forward;
                    }
                }
            }

            this.RotateUpdate();
            this.VelocityUpdate();

            this.m_Character.CharacterAnimatorController.SetFloat(AnimatorHashIDs.SpeedFloat, this.m_Speed);
        }

        // -----------------------------------------------------------------------------------------------------------------------------

        private void RotateUpdate()
        {
            if (this.m_VelocityMagnitude > 0.1f)
            {
                Vector3 rotate = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * this.m_Velocity;
                Quaternion targetRotation = Quaternion.LookRotation(rotate);
                float nRotateSpeed = this.m_Character.m_IsGround ? this.m_RotateSpeed * 0.8f : this.m_RotateSpeed;
                float maxRotationSpeed = nRotateSpeed * Time.deltaTime;
                Quaternion currRotation = this.transform.rotation;
                Quaternion newRotation = Quaternion.RotateTowards(currRotation, targetRotation, maxRotationSpeed);
                this.transform.rotation = newRotation;

                var targetDir = targetRotation * Vector3.forward;
                var nowDir = transform.rotation * Vector3.forward;
                this.m_RotateCrossDir = Vector3.Cross(targetDir, nowDir); // y > 0 左转  y < 0 右转
                this.m_Character.CharacterAnimatorController.SetFloat(AnimatorHashIDs.SteerFloat, this.m_RotateCrossDir.y);
            }
            else
            {
                this.m_Character.CharacterAnimatorController.SetFloat(AnimatorHashIDs.SteerFloat, 0);
            }
        }

        private void VelocityUpdate()
        {
            if (this.m_ForceMovementVelocity != Vector3.zero)
            {
                this.m_Character.CharacterController.Move(this.m_ForceMovementVelocity * Time.deltaTime);
                this.m_LastMovementVelocity = this.m_MovementVelocity;
                return;
            }
            this.GravityVelocity();
            this.JumpVelocity();
            this.HitVelocity();
            this.MovementVelocity();

            this.m_Character.CharacterController.Move(this.m_MovementVelocity * Time.deltaTime);
            this.m_LastMovementVelocity = this.m_MovementVelocity;
            // Debug.DrawRay(this.transform.position, this.m_MovementVelocity * Time.deltaTime, Color.red, 60f);
            this.m_MovementVelocity.Set(0, 0, 0);
        }

        private void MovementVelocity()
        {
            if (m_Character.m_IsDizzy)
            {
                return;
            }
            if (this.m_VelocityMagnitude > 0.1f)
            {
                this.m_VelocityMagnitude = Mathf.Min(1, this.m_VelocityMagnitude);
                // if (!this.m_Character.m_IsGround)
                // {
                //     this.m_VelocityMagnitude = this.m_VelocityMagnitude * 0.8f;
                // }

                // 斜坡速度计算
                float includedAngle = this.m_Character.m_CharacterIncludedAngleByGround - 90;
                float includedAngleAbs = Mathf.Abs(includedAngle);
                if (includedAngleAbs > 0.1)
                {
                    float angleSpeed = Mathf.Clamp(-includedAngle * 0.2f, 0, 30);
                    this.m_InertiaSpeed = Mathf.Lerp(this.m_InertiaSpeed, angleSpeed, Time.deltaTime * (angleSpeed > 0 ? 5 : 0.5f));
                }
                else
                {
                    this.m_InertiaSpeed = Mathf.Lerp(this.m_InertiaSpeed, 0, Time.deltaTime * 0.5f);
                }

                // 加速度计算
                float targetSpeed = this.m_MovementSpeed * this.m_VelocityMagnitude + this.m_InertiaSpeed;
                this.m_SpeedDownLerpTime = 0;
                this.m_SpeedUpLerpTime = Mathf.Min(1, this.m_SpeedUpLerpTime + this.m_MovementAcceleratedSpeed * Time.deltaTime);
                this.m_Speed = this.m_SpeedUpLerpTime >= 1 ? targetSpeed : Mathf.Lerp(this.m_Speed, targetSpeed, this.m_SpeedUpLerpTime);
                this.m_MovementVelocity += this.m_MovementDirection * this.m_Speed;
            }
            else
            {
                // 减速度计算
                this.m_SpeedUpLerpTime = 0;
                if (this.m_Speed > 0)
                {
                    this.m_SpeedDownLerpTime += Mathf.Min(1, this.m_SpeedDownLerpTime + this.m_MovementAcceleratedSpeed * Time.deltaTime);
                    this.m_Speed = this.m_SpeedDownLerpTime >= 1 ? 0 : Mathf.Lerp(this.m_Speed, 0, this.m_SpeedDownLerpTime);
                    this.m_MovementVelocity += this.m_MovementDirection * this.m_Speed;
                }
            }

            // buff速度计算
            if (this.m_Character.m_SpeedUpBuffs.Count > 0)
            {
                float speedUp = 0;
                for (int i = this.m_Character.m_SpeedUpBuffs.Count - 1; i >= 0; i--)
                {
                    speedUp += this.m_Character.m_SpeedUpBuffs[i].SpeedUpValue;
                    this.m_Character.m_SpeedUpBuffs[i].Timer -= Time.deltaTime;
                    if (this.m_Character.m_SpeedUpBuffs[i].Timer <= 0)
                    {
                        this.m_Character.m_SpeedUpBuffs.RemoveAt(i);
                    }
                }
                if (this.m_Character.m_SpeedUpBuffs.Count == 0)
                {
                    this.m_Character.CharacterAnimatorController.SetBool(AnimatorHashIDs.SprintBool, false);
                }
                this.m_MovementVelocity += this.m_MovementDirection * speedUp;
            }

            // 偏移速度
            if (this.m_OffsetVelocityMagnitude > 0)
            {
                this.m_MovementVelocity += this.m_OffsetVelocity;
                this.m_OffsetVelocity.Set(0, 0, 0);
                this.m_OffsetVelocityMagnitude = 0;
            }
        }

        private void JumpVelocity()
        {
            if (this.m_IsJump)
            {
                this.m_IsJump = false;
                this.m_JumpVelocity = this.m_JumpSpeed;
                this.m_MovementVelocity.y = this.m_JumpVelocity;
            }
        }

        private void GravityVelocity()
        {
            if (!useGravity)
            {
                return;
            }
            if (!this.m_Character.m_IsGround || this.m_IsJump)
            {
                this.m_JumpVelocity += GameDefine.Gravity * Time.deltaTime;
            }
            else
            {
                this.m_JumpVelocity = 0;
            }
            this.m_MovementVelocity.y = this.m_JumpVelocity;
        }

        private void HitVelocity()
        {
            m_MovementVelocity += this.m_Character.BuffContext.Velocity;
            /*
            if (this.m_Character.m_VelocityBuffs.Count > 0)
            {
                for (int i = this.m_Character.m_VelocityBuffs.Count - 1; i >= 0; i--)
                {
                    float VelocityValue = this.m_Character.m_VelocityBuffs[i].VelocityValue * this.m_Character.m_VelocityBuffs[i].Timer / this.m_Character.m_VelocityBuffs[i].Time;
                    this.m_MovementVelocity += this.m_Character.m_VelocityBuffs[i].VelocityDirection * VelocityValue;
                    this.m_Character.m_VelocityBuffs[i].Timer -= Time.deltaTime;
                    if (this.m_Character.m_VelocityBuffs[i].Timer <= 0)
                    {
                        this.m_Character.m_VelocityBuffs.RemoveAt(i);
                    }
                }
            }
            */
        }

        /// <summary>
        /// 设置角色线性速度
        /// </summary>
        /// <param name="velocity"></param>
        public virtual void SetVelocity(Vector3 velocity)
        {
            this.m_Velocity = velocity;
            this.m_VelocityMagnitude = this.m_Velocity.magnitude;
        }

        /// <summary>
        /// 持续偏移速度
        /// </summary>
        /// <param name="velocity"></param>
        public virtual void ContinuousOffsetVelocity(Vector3 velocity)
        {
            this.m_OffsetVelocity += velocity;
            this.m_OffsetVelocityMagnitude = this.m_OffsetVelocity.magnitude;
        }

        /// <summary>
        /// 角色跳跃
        /// </summary>
        /// <param name="velocityBuff"></param>
        public virtual void Jump()
        {
            if (GameEntry.Context != null && !GameEntry.Context.Gameplay.PlayerActive)
            {
                return;
            }
            if (!this.m_IsJump && (this.m_Character.m_IsGround || Physics.Raycast(this.transform.position + Vector3.up * 0.3f, Vector3.down, out RaycastHit hit, 2 * 0.3f, this.m_Character.m_GroundLayer)))
            {
                this.m_IsJump = true;
                this.m_Character.CharacterAnimatorController.SetTrigger(AnimatorHashIDs.JumpTrigger);
                this.m_Character.m_Effectctrl?.OnJump();
            }
        }

        /// <summary>
        /// 角色冲刺
        /// </summary>
        public virtual void Sprint()
        {
            if (GameEntry.Context != null && !GameEntry.Context.Gameplay.PlayerActive)
            {
                return;
            }
            foreach (var buff in this.m_Character.m_SpeedUpBuffs)
            {
                if (buff.Id == this.GetInstanceID()) return;
            }
            SpeedUpBuff speedUpBuff = new SpeedUpBuff();
            speedUpBuff.Id = this.GetInstanceID();
            speedUpBuff.Time = 1f;
            speedUpBuff.Timer = speedUpBuff.Time;
            speedUpBuff.SpeedUpValue = CarCultivateSystem.Instance.GetNosSpeed();
            this.m_Character.AddSpeedUpBuff(speedUpBuff);
            this.m_Character.m_Effectctrl?.OnAccelerate();
            this.m_Character.CharacterAnimatorController.SetBool(AnimatorHashIDs.SprintBool, true);
        }
    }
}